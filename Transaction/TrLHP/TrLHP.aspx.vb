Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrLHP_TrLHP
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, Nmbr, TransDate, Status, LHPNo, WorkCtr, WorkCtr_Name, ProductionDate, Shift, Shift_Name, WorkHour, Remark, UserPrep, DatePrep, UserAppr, DateAppr, UserId FROM V_PROResultHdUser"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                'If ViewState("Sender") = "btnLHPNo" Then
                '    BindToText(tbLHPNo, Session("Result")(0).ToString)
                'End If
                If ViewState("Sender") = "btnWONo" Then
                    '"WO_No, ItemNo, Product_Code, Product_Name, Qty_WO, Qty_Outstanding, Unit, LotNo, OutputType"
                    BindToText(tbWONo, Session("Result")(0).ToString)
                    lbItemNo.Text = Session("Result")(1).ToString
                    BindToText(tbProductCode, Session("Result")(2).ToString)
                    BindToText(tbProductName, Session("Result")(3).ToString)
                    BindToText(tbQtyWO, Session("Result")(4).ToString)
                    BindToText(tbQtyGood, Session("Result")(5).ToString)
                    BindToDropList(ddlUnit, Session("Result")(6).ToString)
                    BindToText(tbLotNo, Session("Result")(7).ToString)
                    lbOutputType.Text = Session("Result")(8).ToString
                    If lbOutputType.Text = "FG" Then
                        ddlWarehouse.SelectedValue = ViewState("FGWarehouse").ToString
                        FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ViewState("FGWarehouse").ToString), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                        ddlLocation.SelectedValue = ViewState("FGLocation").ToString
                    Else
                        ddlWarehouse.SelectedValue = ""
                        ddlLocation.SelectedValue = ""
                    End If
                    BindToDropList(ddlMachine, Session("Result")(9).ToString)
                    ddlWarehouse.Enabled = (lbOutputType.Text = "FG")
                    ddlLocation.Enabled = (lbOutputType.Text = "FG")
                    AttachScript("setformatdt();", Page, Me.GetType())
                End If

                If ViewState("Sender") = "btnGetWaste" Then
                    Dim drResult As DataRow
                    ' Dim ExistRow As DataRow()
                    Dim MaxItem, Waste As String

                    For Each drResult In Session("Result").Rows
                        Waste = drResult("Waste_Code").ToString.Trim
                        MaxItem = GetNewItemNo(ViewState("Dt3"))
                        'ExistRow = ViewState("Dt3").select("Material = " + QuotedStr(Waste))

                        ' If ExistRow.Count = 0 Then
                        Dim dr As DataRow
                        dr = ViewState("Dt3").NewRow
                        dr("ItemNo") = MaxItem
                        dr("Material") = drResult("Waste_Code").ToString.Trim
                        dr("Material_Name") = drResult("Waste_Name").ToString.Trim
                        dr("Qty") = 0
                        dr("Unit") = ""
                        dr("CauseWaste") = ""
                        dr("Remark") = ""
                        ViewState("Dt3").Rows.Add(dr)
                        ' End If

                    Next
                    BindGridDt(ViewState("Dt3"), GridDt3)

                End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim WONo, ItemNo As String
                    For Each drResult In Session("Result").Rows
                        WONo = drResult("WO_No").ToString.Trim
                        ItemNo = drResult("ItemNo").ToString.Trim

                        ExistRow = ViewState("Dt").Select("WONo = " + QuotedStr(WONo) + " AND ItemNo = " + ItemNo)
                        If ExistRow.Count = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("WONo") = drResult("WO_No").ToString.Trim
                            dr("ItemNo") = drResult("ItemNo").ToString.Trim
                            dr("Product") = drResult("Product_Code").ToString.Trim
                            dr("Product_Name") = TrimStr(drResult("Product_Name").ToString)
                            dr("LotNo") = TrimStr(drResult("LotNo").ToString)
                            dr("QtyWO") = FormatFloat(drResult("Qty_WO").ToString.Trim, ViewState("DigitQty"))
                            dr("QtyOutput") = FormatFloat(drResult("Qty_Outstanding").ToString.Trim, ViewState("DigitQty"))
                            dr("QtyOK") = FormatFloat(drResult("Qty_Outstanding").ToString.Trim, ViewState("DigitQty"))
                            'dr("QtyRepair") = FormatFloat("0", ViewState("DigitQty"))
                            dr("QtyReject") = FormatFloat("0", ViewState("DigitQty"))
                            dr("Unit") = TrimStr(drResult("Unit").ToString)
                            dr("CauseReject") = ""
                            If TrimStr(drResult("OutputType").ToString) = "FG" Then
                                dr("Warehouse") = ViewState("FGWarehouse").ToString
                                dr("Warehouse_Name") = ViewState("FGWarehouseName").ToString
                                dr("Location") = ViewState("FGLocation").ToString
                                dr("Location_Name") = ViewState("FGLocationName").ToString
                            Else
                                dr("Warehouse") = ""
                                dr("Warehouse_Name") = ""
                                dr("Location") = ""
                                dr("Location_Name") = ""
                            End If
                            dr("Machine") = drResult("Machine").ToString
                            dr("Machine_Name") = drResult("Machine_Name").ToString
                            'dr("MachineHour") = tbWorkHour.Text
                            'dr("ManPower") = "0"
                            dr("OutputType") = TrimStr(drResult("OutputType").ToString)
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    '    'Session("ResultSame") = Nothing
                End If
                If ViewState("Sender") = "btnGetOperator" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim WONo As String
                    For Each drResult In Session("Result").Rows
                        WONo = drResult("Operator").ToString.Trim

                        ExistRow = ViewState("Dt5").select("EmpNumb = " + QuotedStr(WONo))
                        If ExistRow.Count = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("Dt5").NewRow
                            dr("EmpNumb") = drResult("Operator").ToString.Trim
                            dr("Emp_Name") = drResult("Operator_Name").ToString.Trim
                            dr("JobTitle") = drResult("JobTitle").ToString.Trim
                            dr("JobTtlName") = TrimStr(drResult("JobTtlName").ToString)
                            ViewState("Dt5").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt5"), GridDt5)
                    'EnableHd(GetCountRecord(ViewState("Dt5")) = 0)
                End If
                'If ViewState("Sender") = "btnWrhs" Then
                '    BindToText(tbWrhsCode, Session("Result")(0).ToString)
                '    BindToText(tbWrhsName, Session("Result")(1).ToString)
                '    FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(tbWrhsCode.Text), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                'End If
                If ViewState("Sender") = "btnMaterialDt2" Then
                    BindToText(tbMaterialDt2, Session("Result")(0).ToString)
                    BindToText(tbMaterialNameDt2, Session("Result")(1).ToString)
                    BindToText(tbQtyDt2, Session("Result")(2).ToString)
                    BindToDropList(ddlUnitDt2, Session("Result")(3).ToString)
                End If
                'If ViewState("Sender") = "btnMachine" Then
                '    BindToText(tbMachineCode, Session("Result")(0).ToString)
                '    BindToText(tbMachineName, Session("Result")(1).ToString)
                'End If
                If ViewState("Sender") = "btnMachineDt4" Then
                    BindToText(tbMachineCodeDt4, Session("Result")(0).ToString)
                    BindToText(tbMachineNameDt4, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMaterialDt3" Then
                    BindToText(tbMaterialDt3, Session("Result")(0).ToString)
                    BindToText(tbMaterialNameDt3, Session("Result")(1).ToString)
                    'BindToText(tbQtyDt3, Session("Result")(2).ToString)
                    'BindToDropList(ddlUnitDt3, Session("Result")(3).ToString)
                End If
                If ViewState("Sender") = "btnEmp" Then
                    BindToText(tbEmp, Session("Result")(0).ToString)
                    BindToText(tbEmpName, Session("Result")(1).ToString)
                    BindToText(tbJobTitle, Session("Result")(2).ToString)
                    BindToText(tbJobTitleName, Session("Result")(3).ToString)
                End If
                If ViewState("Sender") = "btnSubLed" Then
                    tbSubLed.Text = Session("Result")(0).ToString
                    tbSubLedName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnDownTime" Then
                    BindToText(tbDownTime, Session("Result")(0).ToString)
                    BindToText(tbDownTimeName, Session("Result")(1).ToString)
                End If
                Session("filter") = Nothing
                Session("Column") = Nothing
                Session("Result") = Nothing
            End If

            'dsUseRollNo.ConnectionString = ViewState("DBConnection")
            lbStatus.Text = ""
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            'If MultiView1.ActiveViewIndex = 1 Then
            '    pnlDt2.Visible = True
            '    pnlEditDt2.Visible = False
            'Else
            If MultiView1.ActiveViewIndex = 2 Then
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                GridDt3.Columns(0).Visible = GetCountRecord(ViewState("Dt3")) > 0
                'BindDataDt3(ViewState("TransNmbr"))
            ElseIf MultiView1.ActiveViewIndex = 3 Then
                pnlDt4.Visible = True
                pnlEditDt4.Visible = False
                GridDt4.Columns(0).Visible = GetCountRecord(ViewState("Dt4")) > 0
                'BindDataDt4(ViewState("TransNmbr"))
            Else
                PnlDt.Visible = True
                pnlEditDt.Visible = False
            End If
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitDt2, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitDt3, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlShift, "SELECT * FROM VMsShiftProduction", False, "Shift", "ShiftName", ViewState("DBConnection"))
            'FillCombo(ddlWorkCtr, "EXEC S_GetWorkCtr", False, "WorkCtr_Code", "WorkCtr_Name", ViewState("DBConnection"))
            FillCombo(ddlWorkCtr, "Select WorkCtr, WorkCtr_Name from VMsWorkCtrUser WHERE UserId = " + QuotedStr(ViewState("UserId")), False, "WorkCtr", "WorkCtr_Name", ViewState("DBConnection"))
            'FillCombo(ddlWarehouse, "EXEC S_FindWorkCtrWrhs " + QuotedStr(ddlWorkCtr.SelectedValue), True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
            FillCombo(ddlMachine, "SELECT Machine_Code, Machine_Name, MachineCodeName FROM VMsMachine", True, "Machine_Code", "MachineCodeName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("SortExpressionOut") = Nothing
            ViewState("SortExpressionUse") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbQtyWO.Attributes.Add("ReadOnly", "True")
            tbQtyWO.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyOutput.Attributes.Add("ReadOnly", "True")
            tbQtyOutput.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyGood.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyGood.Attributes.Add("OnBlur", "setformatdt();")
            'tbQtyRepair.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbQtyRepair.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyReject.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyReject.Attributes.Add("OnBlur", "setformatdt();")

            tbGood.Attributes.Add("ReadOnly", "True")
            tbReject.Attributes.Add("ReadOnly", "True")
            tbBlok.Attributes.Add("ReadOnly", "True")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim SQLString, StrFilter As String
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
            SQLString = GetStringHd + " WHERE UserId = " + QuotedStr(ViewState("UserId"))
            DT = BindDataTransaction(SQLString , StrFilter, ViewState("DBConnection").ToString)
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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROResultDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROResultDtRM WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROResultWaste WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt4(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROResultDown WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt5(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROResultEmp WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Sub bindDataGridLot(ByVal Nmbr As String, ByVal WOItemNo As String)
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            SqlString = "SELECT TransNmbr, WONo, ItemNo, WOItemNo, RollNo FROM V_PROResultRollUse WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND WOItemNo = " + QuotedStr(WOItemNo)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridRollUse)
            Else
                DV.Sort = ViewState("SortExpressionUse")
                If ViewState("SortExpressionUse") = Nothing Then
                    ViewState("SortExpressionUse") = "RollNo DESC"
                End If
                GridRollUse.DataSource = DV
                GridRollUse.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = "bindDataGridLot Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Private Sub bindDataGridOutput(ByVal Nmbr As String, ByVal WOItemNo As String)
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim GVR As GridViewRow
        Dim useRollNo, Status As DropDownList
        Dim lbluseRollNo, lblStatus As Label
        Dim Weight As TextBox

        tbGood.Text = 0
        tbReject.Text = 0
        tbBlok.Text = 0

        Try
            SqlString = "SELECT TransNmbr, WONo, ItemNo, NoUrut, RollNo, Length, Width, GSM, QtyRoll, WeightRoll, UseRollNo, Status FROM V_PROResultRollOutput WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND WOItemNo = " + QuotedStr(WOItemNo) + " ORDER BY NoUrut"

            'lbStatus.Text = SqlString
            'Exit Sub

            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridRollOutput)
            Else
                DV.Sort = ViewState("SortExpressionOut")
                If ViewState("SortExpressionOut") = Nothing Then
                    ViewState("SortExpressionOut") = "NoUrut ASC"
                End If
                GridRollOutput.DataSource = DV
                GridRollOutput.DataBind()

                For Each GVR In GridRollOutput.Rows
                    useRollNo = GVR.FindControl("UseRollNoEdit")
                    lbluseRollNo = GVR.FindControl("UseRollNo")
                    Weight = GVR.FindControl("WeightRollEdit")

                    Status = GVR.FindControl("StatusEdit")
                    lblStatus = GVR.FindControl("Status")

                    FillCombo(useRollNo, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
                    useRollNo.SelectedValue = lbluseRollNo.Text

                    Status.SelectedValue = lblStatus.Text

                    If Status.SelectedItem.Text = "Good" Then
                        tbGood.Text += CFloat(Weight.Text)
                    ElseIf Status.SelectedItem.Text = "Reject" Then
                        tbReject.Text += CFloat(Weight.Text)
                    ElseIf Status.SelectedItem.Text = "Blok" Then
                        tbBlok.Text += CFloat(Weight.Text)
                    End If
                Next
            End If

            'Dim dt As New DataTable
            'Dim Drow As DataRow()
            'ViewState("DtLotNo") = Nothing
            'dt = SQLExecuteQuery(GetStringLot(Nmbr, WOItemNo), ViewState("DBConnection").ToString).Tables(0)
            'ViewState("DtLotNo") = dt
            ''BindGridDt(dt, GridDt2)

            'Drow = ViewState("DtLotNo").Select("WONo+'-'+ItemNo=" + QuotedStr(ddlLotWOItemNo.SelectedValue))
            'If Drow.Length > 0 Then
            '    BindGridDt(Drow.CopyToDataTable, GridRollOutput)
            '    GridRollOutput.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("DtLotNo").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridRollOutput.DataSource = DtTemp
            '    GridRollOutput.DataBind()
            '    GridRollOutput.Columns(0).Visible = False
            'End If
        Catch ex As Exception
            lbStatus.Text = "bindDataGridOutput Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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

            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_PDResult", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpand.Click
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'tbDate.Enabled = State W-2015-04-04-0099
            'ddlLHPType.Enabled = State
            'lblLHPNo.Visible = ddlLHPType.SelectedIndex = 1
            'btnLHPNo.Visible = ddlLHPType.SelectedIndex = 1
            ddlWorkCtr.Enabled = State
            'ddlShift.Enabled = State W-2015-04-04-0098
            'tbProductionDate.Enabled = State W-2015-04-04-0097
            btnGetDt.Visible = State
            'tbWorkHour.Enabled = State
            'tbRemark.Enabled = State W-2015-04-04-0100
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt3(ByVal State As Boolean)
        Try
            tbMaterialDt3.Enabled = State            
        Catch ex As Exception
            Throw New Exception("Enable Dt3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt4(ByVal State As Boolean)
        Try
            tbMachineCodeDt4.Enabled = State
            'diremark tbStartTime.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt4 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt5(ByVal State As Boolean)
        Try
            tbEmp.Enabled = State
            btnEmp.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt5 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            'BindGridDt(dt, GridDt2)
            Drow = ViewState("Dt2").Select("WONo=" + QuotedStr(lbWODt2.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt4(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt4") = Nothing
            dt = SQLExecuteQuery(GetStringDt4(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt4") = dt
            BindGridDt(dt, GridDt4)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt4 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt5(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt5") = Nothing
            dt = SQLExecuteQuery(GetStringDt5(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt5") = dt
            BindGridDt(dt, GridDt5)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt5 Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindGridDt2(ByVal source As DataTable, ByVal gv As GridView)
        Dim IsEmpty As Boolean
        Dim DtTemp As DataTable
        Dim dr As DataRow()
        Try
            IsEmpty = False
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                gv.DataSource = DtTemp
            Else
                gv.DataSource = source
            End If
            gv.DataBind()           
            If IsEmpty = True Then
                gv.Columns(0).Visible = False
            Else
                gv.Columns(0).Visible = True
            End If

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            'ddlLHPType.SelectedIndex = 0
            'lblLHPNo.Visible = ddlLHPType.SelectedIndex = 1
            'tbLHPNo.Text = ""
            tbWorkHour.Text = "8"
            tbProductionDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbRemark.Text = ""
            GetWrhsLocation()
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbWONo.Text = ""
            lbItemNo.Text = ""
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbLotNo.Text = ""
            tbQtyWO.Text = "0"
            tbQtyOutput.Text = "0"
            tbQtyGood.Text = "0"
            'tbQtyRepair.Text = "0"
            tbQtyReject.Text = "0"
            ddlUnit.SelectedValue = ""
            tbCauseReject.Text = ""
            'tbWrhsCode.Text = ""
            'tbWrhsName.Text = ""
            FillCombo(ddlWarehouse, "EXEC S_FindWorkCtrWrhs " + QuotedStr(ddlWorkCtr.SelectedValue), True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
            FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWarehouse.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            ddlLocation.SelectedValue = ""
            ddlMachine.SelectedValue = ""
            'tbMachineCode.Text = ""
            'tbMachineName.Text = ""
            'tbMachineHour.Text = tbWorkHour.Text
            'tbManPower.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbMaterialDt2.Text = ""
            tbMaterialNameDt2.Text = ""
            tbQtyDt2.Text = "0"
            ddlUnitDt2.SelectedValue = ""
            FillCombo(ddlwrhsDt2, "EXEC S_FindWorkCtrWrhs " + QuotedStr(ddlWorkCtr.SelectedValue), True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
            ddlwrhsDt2.SelectedValue = ""
            tbFgSubLed.Text = "N"
            tbSubLed.Text = ""
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            FillCombo(ddlLocationDt2, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhsDt2.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            ddlLocationDt2.SelectedValue = ""
            tbRemarkDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            tbMaterialDt3.Text = ""
            tbMaterialNameDt3.Text = ""
            tbQtyDt3.Text = "0"
            ddlUnitDt3.SelectedValue = ""
            tbCauseWaste.Text = ""
            tbRemarkDt3.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt4()
        Try
            tbMachineCodeDt4.Text = ""
            tbMachineNameDt4.Text = ""
            tbStartTime.Text = "00:00"
            tbEndTime.Text = "00:00"
            tbDuration.Text = "0"
            tbCauseDownTime.Text = ""
            tbDownTime.Text = ""
            tbDownTimeName.Text = ""
            tbRemarkDt4.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 4 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearDt5()
        Try
            tbEmp.Text = ""
            tbEmpName.Text = ""
            tbJobTitle.Text = ""
            tbJobTitleName.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 5 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub ClearLotNo()
        Try
            tbGenerateNoEnd.Text = "0"
            tbGenerateNoStart.Text = "0"
            tbGenerateSize.Text = "0"
            tbGenerateLength.Text = "0"
            tbGenerateWeight.Text = "0"
            tbGenerateGSM.Text = "0"
            tbGeneratePrefix.Text = ""
            tbGenerateSufix.Text = ""
            tbGeneretDigit.Text = "0"
        Catch ex As Exception
            Throw New Exception("ClearLotNo Error " + ex.ToString)
        End Try
    End Sub
    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            'If (ddlLHPType.SelectedIndex = 1) And (tbLHPNo.Text.Trim = "") Then
            '    lbStatus.Text = MessageDlg("LHP No must have value")
            '    btnLHPNo.Focus()
            '    Return False
            'End If
            If ddlShift.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Shift must have value")
                ddlShift.Focus()
                Return False
            End If
            'If CFloat(tbWorkHour.Text) <= 0 Then
            '    tbWorkHour.Text = MessageDlg("Work Hour must have value")
            '    tbWorkHour.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("WONo").ToString = "" Then
                    lbStatus.Text = MessageDlg("WO No Must Have Value")
                    Return False
                End If
                If TrimStr(Dr("Product").ToString) = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If TrimStr(Dr("OutputType").ToString) = "FG" Then
                    If TrimStr(Dr("Warehouse").ToString) = "" Then
                        lbStatus.Text = MessageDlg("Warehouse Must Have Value")
                        Return False
                    End If
                    If TrimStr(Dr("Location").ToString) = "" Then
                        lbStatus.Text = MessageDlg("Location Must Have Value")
                        Return False
                    End If
                End If
                If TrimStr(Dr("Machine").ToString) = "" Then
                    lbStatus.Text = MessageDlg("Machine Must Have Value")
                    Return False
                End If
            Else
                If tbWONo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("WO No Must Have Value")
                    tbWONo.Focus()
                    Return False
                End If
                If tbProductName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If lbOutputType.Text = "FG" Then
                    If ddlWarehouse.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("warehouse Must Have Value")
                        ddlWarehouse.Focus()
                        Return False
                    End If
                    If ddlLocation.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Location Must Have Value")
                        ddlLocation.Focus()
                        Return False
                    End If
                End If
                If ddlMachine.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Machine Must Have Value")
                    ddlMachine.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Warehouse").ToString = "" Then
                    lbStatus.Text = MessageDlg("Warehouse Must Have Value")
                    Return False
                End If
                If Dr("Subled").ToString = "" Then
                    lbStatus.Text = MessageDlg("Subled Must Have Value")
                    Return False
                End If

            Else
                If ddlwrhsDt2.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Warehouse must have value")
                    ddlwrhsDt2.Focus()
                    Return False
                End If
                If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                    lbStatus.Text = MessageDlg("SubLed must have value")
                    tbSubLed.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt4(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Machine").ToString = "" Then
                    lbStatus.Text = MessageDlg("Machine Must Have Value")
                    Return False
                End If
                'If Dr("StartTime").ToString = "00:00" Then
                '    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                '    Return False
                'End If
                If CFloat(Dr("Duration").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Duration Must Have Value")
                    Return False
                End If
                

            Else
                If tbMachineNameDt4.Text = "" Then
                    lbStatus.Text = MessageDlg("Machine must have value")
                    tbMachineCodeDt4.Focus()
                    Return False
                End If
                'If tbStartTime.Text.Trim = "00:00" Then
                '    lbStatus.Text = MessageDlg("Start Time must have value")
                '    tbStartTime.Focus()
                '    Return False
                'End If
                If CFloat(tbDuration.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Duration Must Have Value")
                    tbDuration.Focus()
                    Return False
                End If
                
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt4 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlLHPType, Dt.Rows(0)("Type").ToString)
            'BindToText(tbLHPNo, Dt.Rows(0)("LHPNo").ToString)
            BindToDropList(ddlWorkCtr, Dt.Rows(0)("WorkCtr").ToString)
            BindToDropList(ddlShift, Dt.Rows(0)("Shift").ToString)
            BindToDate(tbProductionDate, Dt.Rows(0)("ProductionDate").ToString)
            BindToText(tbWorkHour, Dt.Rows(0)("WorkHour").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            FillCombo(ddlWarehouse, "EXEC S_FindWorkCtrWrhs " + QuotedStr(ddlWorkCtr.SelectedValue), True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
            GetWrhsLocation()
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String, ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("WONo = " + QuotedStr(RRNo) + " AND ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                BindToText(tbWONo, Dr(0)("WONo").ToString)
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbLotNo, Dr(0)("LotNo").ToString)
                BindToText(tbQtyWO, Dr(0)("QtyWO").ToString)
                BindToText(tbQtyOutput, Dr(0)("QtyOutput").ToString)
                BindToText(tbQtyGood, Dr(0)("QtyOK").ToString)
                'BindToText(tbQtyRepair, Dr(0)("QtyRepair").ToString)
                BindToText(tbQtyReject, Dr(0)("QtyReject").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                BindToText(tbCauseReject, Dr(0)("CauseReject").ToString)
                BindToDropList(ddlWarehouse, Dr(0)("Warehouse").ToString)
                FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWarehouse.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                BindToDropList(ddlLocation, Dr(0)("Location").ToString)
                BindToDropList(ddlMachine, Dr(0)("Machine").ToString)
                'BindToText(tbMachineName, Dr(0)("Machine_Name").ToString)
                'BindToText(tbMachineHour, Dr(0)("MachineHour").ToString)
                'BindToText(tbManPower, Dr(0)("ManPower").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                lbOutputType.Text = TrimStr(Dr(0)("OutputType").ToString)
                ddlWarehouse.Enabled = (lbOutputType.Text = "FG")
                ddlLocation.Enabled = (lbOutputType.Text = "FG")
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String, ByVal Material As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("WONo = " + QuotedStr(ItemNo) + " AND Material = " + QuotedStr(Material))
            If Dr.Length > 0 Then
                lbWODt2.Text = Dr(0)("WONo").ToString
                BindToText(tbMaterialDt2, Dr(0)("Material").ToString)
                BindToText(tbMaterialNameDt2, Dr(0)("Material_Name").ToString)
                BindToText(tbQtyDt2, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitDt2, Dr(0)("Unit").ToString)
                BindToDropList(ddlwrhsDt2, Dr(0)("Warehouse").ToString)
                BindToText(tbFgSubLed, Dr(0)("FgSubLed").ToString)
                BindToText(tbSubLed, Dr(0)("SubLed").ToString)
                BindToText(tbSubLedName, Dr(0)("SubLed_Name").ToString)
                BindToDropList(ddlLocationDt2, Dr(0)("Location").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemDt3.Text = Dr(0)("ItemNo").ToString
                BindToText(tbMaterialDt3, Dr(0)("Material").ToString)
                BindToText(tbMaterialNameDt3, Dr(0)("Material_Name").ToString)
                BindToText(tbQtyDt3, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitDt3, Dr(0)("Unit").ToString)
                BindToText(tbCauseWaste, Dr(0)("CauseWaste").ToString)
                BindToText(tbRemarkDt3, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt4(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt4").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNoDt4.Text = ItemNo.ToString
                BindToText(tbMachineCodeDt4, Dr(0)("Machine").ToString)
                BindToText(tbMachineNameDt4, Dr(0)("Machine_Name").ToString)
                BindToText(tbStartTime, Dr(0)("StartTime").ToString)
                BindToText(tbEndTime, Dr(0)("EndTime").ToString)
                BindToText(tbDuration, Dr(0)("Duration").ToString)
                BindToText(tbDownTime, Dr(0)("DownTime").ToString)
                BindToText(tbDownTimeName, Dr(0)("DownTimeName").ToString)
                BindToText(tbCauseDownTime, Dr(0)("CauseDownTime").ToString)
                BindToText(tbRemarkDt4, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 4 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt5(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt5").select("EmpNumb = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbEmp, Dr(0)("EmpNumb").ToString)
                BindToText(tbEmpName, Dr(0)("Emp_Name").ToString)
                BindToText(tbJobTitle, Dr(0)("JobTitle").ToString)
                BindToText(tbJobTitleName, Dr(0)("JobTtlName").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 5 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDtLotNo(ByVal Nmbr As String)
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)

            tbLotLHPNo.Text = Nmbr
            BindToDate(tbLotLHPDate, Dt.Rows(0)("TransDate").ToString)
            FillCombo(ddlLotWOItemNo, "EXEC S_PROResultWOItemNo " + QuotedStr(Nmbr), True, "WOItemNo", "WOItemNo", ViewState("DBConnection"))
            FillCombo(ddlLotWrhs, "EXEC S_FindWorkCtrWrhs " + QuotedStr(Dt.Rows(0)("WorkCtr").ToString), True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
            FillCombo(ddlLotUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("fill text box detail 5 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("WONo = " + QuotedStr(tbWONo.Text) + " AND ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                Row("WONo") = tbWONo.Text
                Row("ItemNo") = lbItemNo.Text
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("LotNo") = tbLotNo.Text
                Row("QtyWO") = tbQtyWO.Text
                Row("QtyOutput") = tbQtyOutput.Text
                Row("QtyOK") = tbQtyGood.Text
                'Row("QtyRepair") = tbQtyRepair.Text
                Row("QtyReject") = tbQtyReject.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row("CauseReject") = tbCauseReject.Text
                Row("Warehouse") = ddlWarehouse.SelectedValue
                If ddlWarehouse.SelectedValue = "" Then
                    Row("Warehouse_Name") = ""
                Else
                    Row("Warehouse_Name") = ddlWarehouse.SelectedItem.Text
                End If
                Row("Location") = ddlLocation.SelectedValue
                If ddlLocation.SelectedValue = "" Then
                    Row("Location_Name") = ""
                Else
                    Row("Location_Name") = ddlLocation.SelectedItem.Text
                End If

                Row("Machine") = ddlMachine.SelectedValue
                If ddlMachine.SelectedValue = "" Then
                    Row("Machine_Name") = ""
                Else
                    Row("Machine_Name") = ddlMachine.SelectedItem.Text
                End If
                'Row("MachineHour") = tbMachineHour.Text
                'Row("ManPower") = tbManPower.Text
                Row("Remark") = tbRemarkDt.Text
                Row("OutputType") = lbOutputType.Text
                Row.EndEdit()
            Else
                If CekExistData(ViewState("Dt"), "WONo,ItemNo", tbWONo.Text + "|" + TrimStr(lbItemNo.Text)) = True Then
                    lbStatus.Text = "WO No " + tbWONo.Text + " Item " + lbItemNo.Text + " has been already exist"
                    Exit Sub
                End If
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("WONo") = tbWONo.Text
                dr("ItemNo") = lbItemNo.Text
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("LotNo") = tbLotNo.Text
                dr("QtyWO") = tbQtyWO.Text
                dr("QtyOutput") = tbQtyOutput.Text
                dr("QtyOK") = tbQtyGood.Text
                'dr("QtyRepair") = tbQtyRepair.Text
                dr("QtyReject") = tbQtyReject.Text
                dr("Unit") = ddlUnit.SelectedValue
                dr("CauseReject") = tbCauseReject.Text
                dr("Warehouse") = ddlWarehouse.SelectedValue
                If ddlWarehouse.SelectedValue = "" Then
                    dr("Warehouse_Name") = ""
                Else
                    dr("Warehouse_Name") = ddlWarehouse.SelectedItem.Text
                End If
                dr("Location") = ddlLocation.SelectedValue
                If ddlLocation.SelectedValue = "" Then
                    dr("Location_Name") = ""
                Else
                    dr("Location_Name") = ddlLocation.SelectedItem.Text
                End If
                dr("Machine") = ddlMachine.SelectedValue
                If ddlMachine.SelectedValue = "" Then
                    dr("Machine_Name") = ""
                Else
                    dr("Machine_Name") = ddlMachine.SelectedItem.Text
                End If
                'dr("MachineHour") = tbMachineHour.Text
                'dr("ManPower") = tbManPower.Text
                dr("Remark") = tbRemarkDt.Text
                dr("OutputType") = lbOutputType.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            'GridDt.Columns(1).Visible = True
            
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try

            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("WONo = " + QuotedStr(lbWODt2.Text) + " AND Material = " + QuotedStr(tbMaterialDt2.Text))(0)
                Row.BeginEdit()
                Row("WONo") = lbWODt2.Text
                Row("Material") = tbMaterialDt2.Text
                Row("Material_Name") = tbMaterialNameDt2.Text
                Row("Qty") = tbQtyDt2.Text
                Row("Unit") = ddlUnitDt2.SelectedValue
                Row("Warehouse") = ddlwrhsDt2.SelectedValue
                Row("Warehouse_Name") = ddlwrhsDt2.SelectedItem.Text
                Row("FgSubled") = tbFgSubLed.Text
                Row("Subled") = tbSubLed.Text
                Row("SubLed_Name") = tbSubLedName.Text
                Row("Location") = ddlLocationDt2.SelectedValue
                Row("Location_Name") = ddlLocationDt2.SelectedItem.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("WONo") = lbWODt2.Text
                dr("Material") = tbMaterialDt2.Text
                dr("Material_Name") = tbMaterialNameDt2.Text
                dr("Qty") = tbQtyDt2.Text
                dr("Unit") = ddlUnitDt2.SelectedValue
                dr("Warehouse") = ddlwrhsDt2.SelectedValue
                dr("Warehouse_Name") = ddlwrhsDt2.SelectedItem.Text
                dr("FgSubled") = tbFgSubLed.Text
                dr("Subled") = tbSubLed.Text
                dr("SubLed_Name") = tbSubLedName.Text
                dr("Location") = ddlLocationDt2.SelectedValue
                dr("Location_Name") = ddlLocationDt2.SelectedItem.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            'BindGridDt(ViewState("Dt2"), GridDt2)
            Dim drow As DataRow()
            Drow = ViewState("Dt2").Select("WONo=" + QuotedStr(lbWODt2.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("LHP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlWorkCtr.SelectedValue, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PROResultHd (TransNmbr, TransDate, STATUS, " + _
                "WorkCtr, ProductionDate, Shift, WorkHour, Remark, " + _
                "UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(ddlWorkCtr.SelectedValue) + "," + _
                QuotedStr(Format(tbProductionDate.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(ddlShift.SelectedValue) + "," + tbWorkHour.Text + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PROResultHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PROResultHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", WorkCtr = " + QuotedStr(ddlWorkCtr.SelectedValue) + ", ProductionDate = " + QuotedStr(Format(tbProductionDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Shift = " + QuotedStr(ddlShift.SelectedValue) + ", WorkHour = " + tbWorkHour.Text + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = Replace(SQLString, "''", "NULL")
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt4").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt5").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, WONo, ItemNo, Product, LotNo, QtyWO, QtyOutput, QtyOK, QtyReject, Unit, CauseReject, Warehouse, Location, Machine, OutputType, Remark FROM PROResultDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PROResultDt SET WONo = @WONo, ItemNo = @ItemNo, Product = @Product, LotNo = @LotNo, QtyWO = @QtyWO, QtyOutput = @QtyOutput, QtyOK = @QtyOK, QtyReject = @QtyReject, Unit = @Unit,  " + _
                    " CauseReject = @CauseReject, Warehouse = @Warehouse, Location = @Location, Machine = @Machine, OutputType =@OutputType, Remark = @Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND WONo = @OldWONo AND ItemNo = @OldItemNo ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@LotNo", SqlDbType.VarChar, 50, "LotNo")
            Update_Command.Parameters.Add("@QtyWO", SqlDbType.Float, 18, "QtyWO")
            Update_Command.Parameters.Add("@QtyOutput", SqlDbType.Float, 18, "QtyOutput")
            Update_Command.Parameters.Add("@QtyOK", SqlDbType.Float, 18, "QtyOK")
            Update_Command.Parameters.Add("@QtyReject", SqlDbType.Float, 18, "QtyReject")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@CauseReject", SqlDbType.VarChar, 255, "CauseReject")
            Update_Command.Parameters.Add("@Warehouse", SqlDbType.VarChar, 10, "Warehouse")
            Update_Command.Parameters.Add("@Location", SqlDbType.VarChar, 10, "Location")
            Update_Command.Parameters.Add("@Machine", SqlDbType.VarChar, 5, "Machine")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@OutputType", SqlDbType.VarChar, 10, "OutputType")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldWONo", SqlDbType.VarChar, 20, "WONo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PROResultDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND WONo = @WONo AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PROResultDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, WONo, Material, Qty, Unit, Warehouse, FgSubled, SubLed, Location, Remark FROM PROResultDtRM WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PROResultDtRM")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, Material, Qty, Unit, CauseWaste, Remark FROM PROResultWaste WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("PROResultWaste")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            'save dt4
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, Machine, StartTime, EndTime, Duration, DownTime, CauseDownTime, Remark FROM PROResultDown WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt4 As New DataTable("PROResultDown")

            Dt4 = ViewState("Dt4")
            da.Update(Dt4)
            Dt4.AcceptChanges()
            ViewState("Dt4") = Dt4


            'save dt5
            cmdSql = New SqlCommand("SELECT TransNmbr, EmpNumb, JobTitle FROM PROResultEmp WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt5 As New DataTable("PROResultEmp")

            Dt5 = ViewState("Dt5")
            da.Update(Dt5)
            Dt5.AcceptChanges()
            ViewState("Dt5") = Dt5
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'WO' must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Material Waste' must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt4")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Machine Down Time' must have at least 1 record")
            '    Exit Sub
            'End If
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try            
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            ModifyInput2(True, pnlInput, pnlDt4, GridDt4)
            ModifyInput2(True, pnlInput, pnlDt5, GridDt5)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            'GridDt.Columns(1).Visible = False            
            MovePanel(PnlHd, pnlInput)
            EnableHd(True)
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbWONo.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbMaterialDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
            BindDataDt4("")
            BindDataDt5("")
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Report Date"
            FDateValue = "TransDate"
            FilterName = "LHP No, LHP Date, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Remark"
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
                    BindDataDt2(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))
                    BindDataDt4(ViewState("TransNmbr"))
                    BindDataDt5(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    btnGetDt.Visible = False
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    ModifyInput2(False, pnlInput, pnlDt4, GridDt4)
                    ModifyInput2(False, pnlInput, pnlDt5, GridDt5)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    'GridDt.Columns(1).Visible = True
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
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        BindDataDt4(ViewState("TransNmbr"))
                        BindDataDt5(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        ModifyInput2(True, pnlInput, pnlDt4, GridDt4)
                        ModifyInput2(True, pnlInput, pnlDt5, GridDt5)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        'GridDt.Columns(1).Visible = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Lot No" Then
                    Try
                        'If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        FillTextBoxDtLotNo(GVR.Cells(2).Text)
                        MovePanel(PnlHd, pnlLotNo)
                        MultiView2.ActiveViewIndex = 0
                        Menu2.Items.Item(0).Selected = True
                        'End If
                    Catch ex As Exception
            lbStatus.Text = "btn LotNo Error = " + ex.ToString
        End Try
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PEFormCandidate " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPROResult.frx"
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            Dim GVR As GridViewRow
            If e.CommandName = "View" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                lbWODt2.Text = GVR.Cells(2).Text
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("WONo = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try

    '    Catch ex As Exception
    '        lbStatus.Text = "GridDt_RowDataBound Error : " & ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r, drt As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("WONo = " + QuotedStr(GVR.Cells(2).Text) + " AND ItemNo = " + GVR.Cells(3).Text)
            For Each r In dr
                r.Delete()
            Next

            For i = 0 To GetCountRecord(ViewState("Dt2")) - 1
                drt = ViewState("Dt2").Rows(i)
                If Not drt.RowState = DataRowState.Deleted Then
                    drt.Delete()
                End If
            Next

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("WONo = " + QuotedStr(lbWODt2.Text) + " AND Material =" + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            'BindGridDt(ViewState("Dt2"), GridDt2)
            dr = ViewState("Dt2").Select("WONo = " + QuotedStr(lbWODt2.Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
    '    Dim GVR As GridViewRow
    '    Try
    '        GVR = GridDt.Rows(e.NewEditIndex)
    '        FillTextBoxDt(GVR.Cells(2).Text, GVR.Cells(3).Text)
    '        MovePanel(PnlDt, pnlEditDt)
    '        EnableHd(False)
    '        ViewState("StateDt") = "Edit"
    '        btnSaveDt.Focus()
    '        StatusButtonSave(False)
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
    '    End Try
    'End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbWODt2.Text, GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'WO' must have at least 1 record")
                Exit Sub
            End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnLHPNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLHPNo.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT Transnmbr AS LHP_No, dbo.FormatDate(TransDate) AS LHP_Date FROM V_PROResultHd WHERE Status = 'P' AND Type = 'First'"
    '        ResultField = "LHP_No, LHP_Date"
    '        ViewState("Sender") = "btnLHPNo"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnLHPNo_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt3") = "Insert"
            lbItemDt3.Text = GetNewItemNo(ViewState("Dt3"))
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)            
            tbMaterialDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("ItemNo = " + lbItemDt3.Text)(0)
                Row.BeginEdit()
                Row("ItemNo") = lbItemDt3.Text
                Row("Material") = tbMaterialDt3.Text
                Row("Material_Name") = tbMaterialNameDt3.Text
                Row("Qty") = tbQtyDt3.Text
                If ddlUnitDt3.SelectedValue = "" Then
                    Row("Unit") = DBNull.Value
                Else
                    Row("Unit") = ddlUnitDt3.SelectedValue
                End If
                Row("CauseWaste") = tbCauseWaste.Text
                Row("Remark") = tbRemarkDt3.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("ItemNo") = lbItemDt3.Text
                dr("Material") = tbMaterialDt3.Text
                dr("Material_Name") = tbMaterialNameDt3.Text
                dr("Qty") = tbQtyDt3.Text
                If ddlUnitDt3.SelectedValue = "" Then
                    dr("Unit") = DBNull.Value
                Else
                    dr("Unit") = ddlUnitDt3.SelectedValue
                End If
                dr("CauseWaste") = tbCauseWaste.Text
                dr("Remark") = tbRemarkDt3.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
                MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
                BindGridDt(ViewState("Dt3"), GridDt3)
                StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt3 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("MaintenanceItem_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    Return False
                End If
                If Dr("Material_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
            Else
               
                If tbMaterialNameDt3.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    tbMaterialNameDt3.Focus()
                    Return False
                End If
                If CFloat(tbQtyDt3.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQtyDt3.Focus()
                    Return False
                End If
                If ddlUnitDt3.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    ddlUnitDt3.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt5(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                
                If Dr("Emp_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Operator Must Have Value")
                    Return False
                End If
                
            Else

                If tbEmpName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Operator Must Have Value")
                    tbEmpName.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt 5 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(1).Text)
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)            
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnWONo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWONo.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT WO_No, dbo.FormatDate(WO_Date) AS Production_Date, ItemNo, WorkCtr, Reference, Process, Product_Code, Product_Name, dbo.FormatFloat(Qty_WO, dbo.DigitQty()) AS Qty_WO, dbo.FormatFloat(Qty_Outstanding, dbo.DigitQty()) AS Qty_Outstanding, Unit, LotNo, Machine, Machine_Name, dbo.FormatDate(DODate) AS DO_Date, OutputType FROM V_PROResultGetWO WHERE WorkCtr =" + QuotedStr(ddlWorkCtr.SelectedValue)
            ResultField = "WO_No, ItemNo, Product_Code, Product_Name, Qty_WO, Qty_Outstanding, Unit, LotNo, OutputType, Machine, Machine_Name"
            ViewState("Sender") = "btnWONo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())            
        Catch ex As Exception
            lbStatus.Text = "btnWONo Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlShift_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShift.SelectedIndexChanged
    End Sub


    'Protected Sub btnWrhs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWrhs.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "EXEC S_FindWorkCtrWrhs " + QuotedStr(ddlWorkCtr.SelectedValue)
    '        ResultField = "WrhsCode, WrhsName"
    '        ViewState("Sender") = "btnWrhs"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnWrhs_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbWrhsCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbWrhsCode.TextChanged
    '    Dim Dt As DataTable
    '    Dim Dr As DataRow
    '    Try
    '        Dt = SQLExecuteQuery("EXEC S_FindWorkCtrWrhs " + QuotedStr(ddlWorkCtr.SelectedValue), ViewState("DBConnection").ToString).Tables(0)            
    '        If Dt.Rows.Count > 0 Then
    '            Dr = Dt.Rows(0)
    '            tbWrhsCode.Text = Dr("WrhsCode")
    '            tbWrhsName.Text = Dr("WrhsName")
    '            If Dr("WrhsCode") <> tbWrhsCode.Text Then
    '                FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(tbWrhsCode.Text), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
    '            End If
    '        Else
    '            tbWrhsCode.Text = ""
    '            tbWrhsName.Text = ""
    '        End If

    '        tbWrhsCode.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tbWrhsCode_TextChanged Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnMaterialDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterialDt2.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_PROResultGetMatReq WHERE WONo = " + QuotedStr(lbWODt2.Text)
            ResultField = "Product, ProductName, Qty, Unit"
            ViewState("Sender") = "btnMaterialDt2"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterialDt2_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnMachine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMachine.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        'Session("filter") = "SELECT Machine_Code, Machine_Name FROM V_PROResultGetWO WHERE WO_No = " + QuotedStr(lbWODt2.Text)
    '        Session("filter") = "SELECT Machine_Code, Machine_Name FROM VMsMachine"
    '        ResultField = "Machine_Code, Machine_Name"
    '        ViewState("Sender") = "btnMachine"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnMachine_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub tbMaterialDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterialDt2.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("SELECT * FROM V_PROResultGetMatReq WHERE WONo = " + QuotedStr(lbWODt2.Text) + " AND Product = " + QuotedStr(tbMaterialDt2.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMaterialDt2.Text = Dr("Product")
                tbMaterialNameDt2.Text = Dr("ProductName")
                tbQtyDt2.Text = Dr("Qty")
                ddlUnitDt2.SelectedValue = Dr("Unit")
            Else
                tbMaterialDt2.Text = ""
                tbMaterialNameDt2.Text = ""
                tbQtyDt2.Text = "0"
                ddlUnitDt2.SelectedValue = ""
            End If
            tbMaterialDt2.Focus()
        Catch ex As Exception
            Throw New Exception("tbMaterialDt2_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub tbMachineCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMachineCode.TextChanged
    '    Dim Dt As DataTable
    '    Dim Dr As DataRow
    '    Try
    '        'Dt = SQLExecuteQuery("SELECT Machine_Code, Machine_Name FROM V_PROResultGetWO WHERE WO_No = " + QuotedStr(lbWODt2.Text) + " AND Machine_Code = " + QuotedStr(tbMachineCode.Text), ViewState("DBConnection").ToString).Tables(0)
    '        Dt = SQLExecuteQuery("SELECT Machine_Code, Machine_Name FROM VMsMachine", ViewState("DBConnection").ToString).Tables(0)
    '        If Dt.Rows.Count > 0 Then
    '            Dr = Dt.Rows(0)
    '            tbMachineCode.Text = Dr("Machine_Code")
    '            tbMachineName.Text = Dr("Machine_Name")
    '        Else
    '            tbMachineCode.Text = ""
    '            tbMachineName.Text = ""
    '        End If
    '        tbMachineCode.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tbMachineCode_TextChanged Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnMaterialDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterialDt3.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsWaste "
            ResultField = "Waste_Code, Waste_Name"
            ViewState("Sender") = "btnMaterialDt3"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterialDt3_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt4.Click
        Try
            If CekDt4() = False Then
                btnSaveDt4.Focus()
                Exit Sub
            End If

            If ViewState("StateDt4") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt4").Select("ItemNo = " + lbItemNoDt4.Text)(0)
                Row.BeginEdit()
                Row("Machine") = tbMachineCodeDt4.Text
                Row("Machine_Name") = tbMachineNameDt4.Text
                Row("StartTime") = tbStartTime.Text
                Row("EndTime") = tbEndTime.Text
                If tbDuration.Text.Trim = "" Then
                    Row("Duration") = "0"
                Else
                    Row("Duration") = tbDuration.Text
                End If
                Row("DownTime") = tbDownTime.Text
                Row("DownTimeName") = tbDownTimeName.Text
                Row("CauseDownTime") = tbCauseDownTime.Text
                Row("Remark") = tbRemarkDt4.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt4").NewRow
                dr("ItemNo") = lbItemNoDt4.Text
                dr("Machine") = tbMachineCodeDt4.Text
                dr("Machine_Name") = tbMachineNameDt4.Text
                dr("StartTime") = tbStartTime.Text
                dr("EndTime") = tbEndTime.Text
                If tbDuration.Text.Trim = "" Then
                    dr("Duration") = "0"
                Else
                    dr("Duration") = tbDuration.Text
                End If
                dr("DownTime") = tbDownTime.Text
                dr("DownTimeName") = tbDownTimeName.Text
                dr("CauseDownTime") = tbCauseDownTime.Text
                dr("Remark") = tbRemarkDt4.Text
                ViewState("Dt4").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt4, pnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            BindGridDt(ViewState("Dt4"), GridDt4)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt4 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub GridDt4_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt4.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt4.Rows(e.RowIndex)
            dr = ViewState("Dt4").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt4"), GridDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 4 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt4.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt4.Rows(e.NewEditIndex)
            FillTextBoxDt4(GVR.Cells(1).Text)
            MovePanel(pnlDt4, pnlEditDt4)
            EnableHd(False)
            ViewState("StateDt4") = "Edit"
            StatusButtonSave(False)
            EnableDt4(False)
            'btnSaveDt4.Focus()
            tbStartTime.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt4 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt4.Click, btnAddDt4ke2.Click
        Try
            Cleardt4()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt4") = "Insert"
            lbItemNoDt4.Text = GetNewItemNo(ViewState("Dt4"))
            MovePanel(pnlDt4, pnlEditDt4)
            EnableHd(False)
            StatusButtonSave(False)
            EnableDt4(True)
            tbMachineCodeDt4.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt4 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMachineDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMachineDt4.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "SELECT Machine_Code, Machine_Name FROM V_PROResultGetWO WHERE WO_No = " + QuotedStr(lbWODt2.Text)
            Session("filter") = "SELECT Machine_Code, Machine_Name FROM VMsMachine"
            ResultField = "Machine_Code, Machine_Name"
            ViewState("Sender") = "btnMachineDt4"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMachineDt4_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbMachineCodeDt4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMachineCodeDt4.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            'Dt = SQLExecuteQuery("SELECT Machine_Code, Machine_Name FROM V_PROResultGetWO WHERE WO_No = " + QuotedStr(lbWODt2.Text) + " AND Machine_Code = " + QuotedStr(tbMachineCodeDt4.Text), ViewState("DBConnection").ToString).Tables(0)
            Dt = SQLExecuteQuery("SELECT Machine_Code, Machine_Name FROM VMsMachine WHERE Machine_Code = " + QuotedStr(tbMachineCodeDt4.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMachineCodeDt4.Text = Dr("Machine_Code")
                tbMachineNameDt4.Text = Dr("Machine_Name")
            Else
                tbMachineCodeDt4.Text = ""
                tbMachineNameDt4.Text = ""
            End If
            tbMachineCodeDt4.Focus()
        Catch ex As Exception
            Throw New Exception("tbMachineCodeDt4_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlwrhsDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlwrhsDt2.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("EXEC S_FindWorkCtrWrhs " + QuotedStr(ddlWorkCtr.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbFgSubLed.Text = Dr("FgSubLed")
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            Else
                tbFgSubLed.Text = "N"
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            End If
            FillCombo(ddlLocationDt2, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhsDt2.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            ddlwrhsDt2.Focus()
        Catch ex As Exception
            Throw New Exception("ddlwrhsDt2_SelectedIndexChanged Error : " + ex.ToString)
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

    Protected Sub btnSubLed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubLed.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT SubLed_No, SubLed_Name FROM VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubLed.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLed"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSubLed_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlWarehouse_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWarehouse.SelectedIndexChanged
        Try
            FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWarehouse.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
        Catch ex As Exception
            lbStatus.Text = "ddlWarehouse_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GetWrhsLocation()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim SQLString As String

        SQLString = "Select A.Warehouse, COALESCE(W.WrhsName,'') AS Warehouse_Name, A.Location, COALESCE(L.WLocationName,'') AS Location_Name " + _
                             "from V_MsWorkCtrWrhsLocation A LEFT OUTER JOIN MsWarehouse W ON A.Warehouse = W.WrhsCode " + _
                             "LEFT OUTER JOIN MsWrhsLocation L ON A.Location = L.WLocationCode " + _
                             "WHERE A.WorkCtrCode = " + QuotedStr(ddlWorkCtr.SelectedValue)
        dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
        dr = dt.Rows(0)
        ViewState("FGLocation") = TrimStr(dr("Location").ToString)
        ViewState("FGLocationName") = TrimStr(dr("Location_Name").ToString)
        ViewState("FGWarehouse") = TrimStr(dr("Warehouse").ToString)
        ViewState("FGWarehouseName") = TrimStr(dr("Warehouse_Name").ToString)
    End Sub

    Protected Sub ddlWorkCtr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWorkCtr.SelectedIndexChanged
        Try
            GetWrhsLocation()
        Catch ex As Exception
            lbStatus.Text = "ddlWorkCtr_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT WO_No, dbo.FormatDate(WO_Date) AS Production_Date, ItemNo, WorkCtr, Reference, Process, Product_Code, Product_Name, dbo.FormatFloat(Qty_WO, dbo.DigitQty()) AS Qty_WO, dbo.FormatFloat(Qty_Outstanding, dbo.DigitQty()) AS Qty_Outstanding, Unit, LotNo, Machine, Machine_Name, dbo.FormatDate(DODate) AS DO_Date, OutputType FROM V_PROResultGetWO WHERE WorkCtr =" + QuotedStr(ddlWorkCtr.SelectedValue)
            ResultField = "WO_No, ItemNo, Product_Code, Product_Name, Qty_WO, Qty_Outstanding, Unit, LotNo, OutputType, Machine, Machine_Name"
            ViewState("Sender") = "btnGetDt"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetDt Click Error : " + ex.ToString
        End Try
    End Sub
    Private Function CalculateDuration(ByVal StartTime As String, ByVal EndTime As String) As Integer
        Dim StartH, StartM, EndH, EndM, iStart, iEnd As Integer

        If Strings.Mid(StartTime, 3, 1) <> ":" Then
            lbStatus.Text = "Start Time is invalid"
            tbStartTime.Focus()
            Return False
        End If
        If Strings.Mid(EndTime, 3, 1) <> ":" Then
            lbStatus.Text = "End Time is invalid"
            tbEndTime.Focus()
            Return False
        End If
        If Len(StartTime) <> 5 Then
            lbStatus.Text = "Start Time is invalid"
            tbStartTime.Focus()
            Return False
        End If
        If Len(EndTime) <> 5 Then
            lbStatus.Text = "End Time is invalid"
            tbEndTime.Focus()
            Return False
        End If

        StartH = CInt(Strings.Mid(StartTime, 1, 2))
        StartM = CInt(Strings.Mid(StartTime, 4, 2))
        EndH = CInt(Strings.Mid(EndTime, 1, 2))
        EndM = CInt(Strings.Mid(EndTime, 4, 2))

        iStart = (StartH * 60) + StartM
        iEnd = (EndH * 60) + EndM

        'If iStart <= 0 Then
        '    lbStatus.Text = "Start Time must have value"
        '    tbStartTime.Focus()
        '    Return False
        'End If
        'If iEnd <= 0 Then
        '    lbStatus.Text = "End Time must have value"
        '    tbEndTime.Focus()
        '    Return False
        'End If

        If iStart > 1440 Then
            lbStatus.Text = "Start Time is invalid"
            tbStartTime.Focus()
            Return False
        End If
        If iEnd > 1440 Then
            lbStatus.Text = "End Time is invalid"
            tbEndTime.Focus()
            Return False
        End If


        If iStart > 0 Then
            If iEnd > iStart Then
                Return iEnd - iStart
            Else
                Return (1440 - iStart) + iEnd
            End If
        End If
    End Function
    Protected Sub tbStartTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartTime.TextChanged
        Try
            tbDuration.Text = (CalculateDuration(tbStartTime.Text, tbEndTime.Text)).ToString
        Catch ex As Exception
            lbStatus.Text = "tbStartTime_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEndTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndTime.TextChanged
        Try
            'If Len(tbEndTime.Text) <> 5 Then
            '    MessageDlg("End Time is not valid")
            '    Exit Sub
            'End If
            tbDuration.Text = (CalculateDuration(tbStartTime.Text, tbEndTime.Text)).ToString
        Catch ex As Exception
            lbStatus.Text = "tbEndTime_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDownTime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownTime.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "SELECT Machine_Code, Machine_Name FROM V_PROResultGetWO WHERE WO_No = " + QuotedStr(lbWODt2.Text)
            Session("filter") = "SELECT DownTimeCode, DownTimeName FROM MsDownTime"
            ResultField = "DownTimeCode, DownTimeName"
            ViewState("Sender") = "btnDownTime"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnDownTime_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt4.Click
        Try
            MovePanel(pnlEditDt4, pnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt4 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlLotWOItemNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLotWOItemNo.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim dbCode As DropDownList
        Try
            Dt = SQLExecuteQuery("SELECT * FROM V_PROResultReffDt WHERE TransNmbr = " + QuotedStr(tbLotLHPNo.Text) + " AND WOItemNo = " + QuotedStr(ddlLotWOItemNo.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbLotProductCode, Dt.Rows(0)("Product").ToString)
                BindToText(tbLotProductName, Dt.Rows(0)("Product_Name").ToString)
                BindToDropList(ddlLotWrhs, Dt.Rows(0)("Warehouse").ToString)
                BindToText(tbLotQtyOK, Dt.Rows(0)("QtyOK").ToString)
                BindToText(tbLotQtyReject, Dt.Rows(0)("QtyReject").ToString)
                BindToDropList(ddlLotUnit, Dt.Rows(0)("Unit").ToString)
                lbLotWONo.Text = Dt.Rows(0)("WONo").ToString
                lbLotItemNo.Text = Dt.Rows(0)("ItemNo").ToString
            Else
                tbLotProductCode.Text = ""
                tbLotProductName.Text = ""
                ddlLotWrhs.SelectedValue = ""
                tbLotQtyOK.Text = "0"
                tbLotQtyReject.Text = "0"
                ddlLotUnit.SelectedValue = ""
                lbLotWONo.Text = "WONo"
                lbLotItemNo.Text = "ItemNo"
            End If
            bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            'BindDataLotNo(ddlLotWOItemNo.SelectedValue)
            dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
            FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("ddlLotWOItemNo_TextChanged error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridRollUse_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridRollUse.PageIndexChanging
        GridRollUse.PageIndex = e.NewPageIndex
        If GridRollUse.EditIndex <> -1 Then
            GridRollUse_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
    End Sub

    Protected Sub GridRollUse_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridRollUse.RowCancelingEdit
        Try
            GridRollUse.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridRollUse.EditIndex = -1
            bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridRollUse_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridRollUse.RowCommand
        Dim SQLString As String
        Dim dbCode As TextBox
        Try
            If e.CommandName = "Insert" Then
                dbCode = GridRollUse.FooterRow.FindControl("RollNoAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lbStatus.Text = "Roll No must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT RollNo From V_PROResultRollUse WHERE TransNmbr  = " + QuotedStr(tbLotLHPNo.Text) + _
                        " AND WOItemNo = " + QuotedStr(ddlLotWOItemNo.SelectedValue) + _
                        " AND RollNo = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Roll No " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT PROResultRollUse (TransNmbr, WONo, ItemNo, RollNo ) " + _
                "SELECT " + QuotedStr(tbLotLHPNo.Text) + ", " + QuotedStr(lbLotWONo.Text) + _
                "," + QuotedStr(lbLotItemNo.Text) + ", " + QuotedStr(dbCode.Text)

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)

            End If
        Catch ex As Exception
            lbStatus.Text = "GridRollUse_RowCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub GridRollUse_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridRollUse.RowDeleting
        Dim txtID As Label
        Dim SqlString As String
        Dim Dt As DataTable
        Try
            'If CheckMenuLevel("Delete") = False Then
            '    Exit Sub
            'End If
            SqlString = " EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue)
            Dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count <> 0 Then
                lbStatus.Text = "Cannot Delete Data Output already exist "
                Exit Sub
            End If

            txtID = GridRollUse.Rows(e.RowIndex).FindControl("RollNo")

            SQLExecuteNonQuery("DELETE FROM PROResultRollUse WHERE TransNmbr  = " + QuotedStr(tbLotLHPNo.Text) + _
                               " AND WONo = " + QuotedStr(lbLotWONo.Text) + " AND ItemNo = " + QuotedStr(lbLotItemNo.Text) + _
                               " AND RollNo = " + QuotedStr(txtID.Text), ViewState("DBConnection"))
            bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)

        Catch ex As Exception
            lbStatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridRollUse_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridRollUse.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox

        Try
            'If CheckMenuLevel("Edit") = False Then
            '    Exit Sub
            'End If
            GridRollUse.EditIndex = e.NewEditIndex
            GridRollUse.ShowFooter = False
            bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            obj = GridRollUse.Rows(e.NewEditIndex)
            txt = obj.FindControl("RollNoEdit")
            ViewState("RollNoEdit") = txt.Text
            txt.Focus()
            
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridRollUse_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridRollUse.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox

        Try
            dbName = GridRollUse.Rows(e.RowIndex).FindControl("RollNoEdit")

            If dbName.Text.Trim.Length = 0 Then
                lbStatus.Text = "Roll No must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE PROResultRollUse SET RollNo = " + QuotedStr(dbName.Text.Replace("'", "''")) + _
            " WHERE TransNmbr  = " + QuotedStr(tbLotLHPNo.Text) + " AND WONo = " + QuotedStr(lbLotWONo.Text) + _
            " AND ItemNo = " + QuotedStr(lbLotItemNo.Text) + " AND RollNo = " + QuotedStr(ViewState("RollNoEdit").ToString)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridRollUse.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridRollUse.EditIndex = -1
            bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)

        Catch ex As Exception
            lbStatus.Text = "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridRollUse_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridRollUse.Sorting
        Try
            If ViewState("SortOrderUse") = Nothing Or ViewState("SortOrderUse") = "DESC" Then
                ViewState("SortOrderUse") = "ASC"
            Else
                ViewState("SortOrderUse") = "DESC"
            End If
            ViewState("SortExpressionUse") = e.SortExpression + " " + ViewState("SortOrderUse")
            bindDataGridLot(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
        Catch ex As Exception
            lbStatus.Text = "GridRollUse_Sorting Eror =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView2.ActiveViewIndex = index
            If MultiView2.ActiveViewIndex = 1 Then
                FillCombo(ddlUseRollNo, "SELECT RollNo FROM V_PROResultRollUse WHERE TransNmbr = " + QuotedStr(tbLotLHPNo.Text) + " AND WOItemNo = " + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
            End If
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If (tbGenerateNoStart.Text.Trim = "") Or (tbGenerateNoEnd.Text.Trim = "") Or (tbGenerateSize.Text.Trim = "") Or (tbGenerateLength.Text.Trim = "") Or (tbGenerateWeight.Text.Trim = "") Or (tbGenerateGSM.Text.Trim = "") Or (tbGeneratePrefix.Text.Trim = "") Or (tbGeneretDigit.Text.Trim = "") Then
            lbStatus.Text = "Set Lot No must be complete"
            tbGenerateNoStart.Focus()
            Exit Sub
        End If
        If (CInt(tbGenerateNoStart.Text) = 0) Or (CInt(tbGenerateNoEnd.Text) = 0) Or (CFloat(tbGenerateSize.Text) = 0) Or (CFloat(tbGenerateLength.Text) = 0) Or (CFloat(tbGenerateWeight.Text) = 0) Or (CFloat(tbGenerateGSM.Text) = 0) Or (tbGeneratePrefix.Text.Trim = "") Or (CInt(tbGeneretDigit.Text) = 0) Then
            lbStatus.Text = "Set Lot No must be complete"
            tbGenerateNoStart.Focus()
            Exit Sub
        End If
        If CInt(tbGeneratePrefix.Text.Length) >= CInt(tbGeneretDigit.Text) Then
            lbStatus.Text = "Prefix can not greater than Digit"
            tbGeneratePrefix.Focus()
            Exit Sub
        End If
        'Dim SQLString As String
        'SQLString = "EXEC S_PROResultGetNoLot " + QuotedStr(tbGeneratePrefix.Text) + ", " + tbGenerateNoStart.Text + ", " + tbGenerateNoEnd.Text + ", " + tbGeneretDigit.Text + ", " + QuotedStr(tbGenerateSufix.Text)

        'lbStatus.Text = SQLString
        'Exit Sub
        'If ddlUseRollNo.SelectedValue = "" Then
        '    lbStatus.Text = "Use roll No must have value"
        '    ddlUseRollNo.Focus()
        '    Exit Sub
        'End If
        BindDataSetNoLot(ddlUseRollNo.SelectedValue, ddlStatus.SelectedValue)
    End Sub
    Private Sub BindDataSetNoLot(ByVal UseRollNo As String, ByVal Status As String)
        Dim SQLString As String
        Dim Dt As DataTable
        Dim DrResult As DataRow
        Dim dbCode As DropDownList
        Dim NoUrut As Integer
        Try
            SQLString = "EXEC S_PROResultGetNoLot " + QuotedStr(tbGeneratePrefix.Text) + ", " + tbGenerateNoStart.Text + ", " + tbGenerateNoEnd.Text + ", " + tbGeneretDigit.Text + ", " + QuotedStr(tbGenerateSufix.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            NoUrut = CInt(SQLExecuteScalar("Select CONVERT(VARCHAR(5),MAX(COALESCE(NoUrut,0))) FROM V_PROResultRollOutput WHERE TransNmbr = " + QuotedStr(tbLotLHPNo.Text) + " AND WOItemNo = " + QuotedStr(ddlLotWOItemNo.SelectedValue), ViewState("DBConnection").ToString))
            If NoUrut = 0 Then
                NoUrut = 1
            Else : NoUrut = NoUrut + 1
            End If

            If Dt.Rows.Count > 0 Then
                For Each DrResult In Dt.Rows
                    'Dim ExistRow As DataRow()
                    'ExistRow = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Location = " + QuotedStr(ddlLocation.SelectedValue) + " AND LotNo = " + QuotedStr(DrResult("Hasil")))
                    'If (ExistRow.Count) = 0 Then
                    SQLString = "INSERT INTO PROResultRollOutput (TransNmbr, WONo, ItemNo, NoUrut, RollNo, Length, Width, GSM, QtyRoll, WeightRoll, UseRollNo, Status) " + _
                    "SELECT " + QuotedStr(tbLotLHPNo.Text) + ", " + QuotedStr(lbLotWONo.Text) + ", " + QuotedStr(lbLotItemNo.Text) + ", " + _
                    NoUrut.ToString + "," + QuotedStr(DrResult("Hasil")) + ", " + tbGenerateLength.Text.Replace(",", "") + _
                    ", " + tbGenerateSize.Text.Replace(",", "") + ", " + tbGenerateGSM.Text.Replace(",", "") + ",1," + tbGenerateWeight.Text.Replace(",", "") + _
                    ", " + QuotedStr(UseRollNo) + ", " + QuotedStr(Status)
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    'End If

                    NoUrut = NoUrut + 1
                Next

            End If
            lbStatus.Text = "Set Success for No Lot"
            ClearLotNo()
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
            FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("bindDataSetNoLot Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridRollOutput_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridRollOutput.PageIndexChanging
        Try
            Dim dbCode As DropDownList
            GridRollOutput.PageIndex = e.NewPageIndex
            If GridRollOutput.EditIndex <> -1 Then
                GridRollOutput_RowCancelingEdit(Nothing, Nothing)
            End If
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
            FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("GridRollOutput_PageIndexChanging Error : " + ex.ToString)
        End Try       
    End Sub

    Protected Sub GridRollOutput_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridRollOutput.RowCancelingEdit
        Try
            Dim dbCode As DropDownList
            GridRollOutput.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridRollOutput.EditIndex = -1
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
            FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
            GridRollOutput.Columns(0).Visible = True
        Catch ex As Exception
            lbStatus.Text = "GridRollOutput_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridRollOutput_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridRollOutput.RowCommand
        'Dim SQLString As String
        Dim dbCode, dbStatus As DropDownList
        Try
            If e.CommandName = "Insert" Then
                dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
                dbStatus = GridRollOutput.FooterRow.FindControl("StatusAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lbStatus.Text = "Use Roll No must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If

                If (CInt(tbGenerateNoStart.Text) = 0) Or (CInt(tbGenerateNoEnd.Text) = 0) Or (CFloat(tbGenerateSize.Text) = 0) Or (CFloat(tbGenerateLength.Text) = 0) Or (CFloat(tbGenerateWeight.Text) = 0) Or (CFloat(tbGenerateGSM.Text) = 0) Or (tbGeneratePrefix.Text.Trim = "") Or (CInt(tbGeneretDigit.Text) = 0) Then
                    lbStatus.Text = "Set Lot No must be complete"
                    tbGenerateNoStart.Focus()
                    Exit Sub
                End If
                If CInt(tbGeneratePrefix.Text.Length) >= CInt(tbGeneretDigit.Text) Then
                    lbStatus.Text = "Prefix can not greater than Digit"
                    tbGeneratePrefix.Focus()
                    Exit Sub
                End If

                BindDataSetNoLot(dbCode.SelectedValue, dbStatus.SelectedValue)
            End If
        Catch ex As Exception
            lbStatus.Text = "GridRollUse_RowCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub GridRollOutput_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridRollOutput.RowDeleting
        Dim txtID As Label
        Dim dbCode As DropDownList
        Try
            'If CheckMenuLevel("Delete") = False Then
            '    Exit Sub
            'End If
            txtID = GridRollOutput.Rows(e.RowIndex).FindControl("RollNo")

            SQLExecuteNonQuery("DELETE FROM PROResultRollOutput WHERE TransNmbr  = " + QuotedStr(tbLotLHPNo.Text) + _
                               " AND WONo = " + QuotedStr(lbLotWONo.Text) + " AND ItemNo = " + QuotedStr(lbLotItemNo.Text) + _
                               " AND RollNo = " + QuotedStr(txtID.Text), ViewState("DBConnection"))
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
            FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridRollOutput_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridRollOutput.RowEditing
        'Dim obj As GridViewRow
        'Dim txt As DropDownList
        'Dim lblUseRoll As TextBox
        'Try
        '    'If CheckMenuLevel("Edit") = False Then
        '    '    Exit Sub
        '    'End If
        '    ' lbStatus.Text = tbLotLHPNo.Text + " " + ddlLotWOItemNo.SelectedValue
        '    ' Exit Sub
        '    GridRollOutput.Columns(0).Visible = False

        '    GridRollOutput.EditIndex = e.NewEditIndex
        '    GridRollOutput.ShowFooter = False
        '    bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)

        '    obj = GridRollOutput.Rows(e.NewEditIndex)
        '    txt = obj.FindControl("UseRollNoEdit")
        '    'UseRollNoEdit
        '    lblUseRoll = obj.FindControl("RollNoEdit")

        '    ViewState("RollNo") = lblUseRoll.Text

        '    ViewState("UseRollNo") = SQLExecuteScalar("SELECT UseRollNo FROM V_PROResultRollOutput WHERE TransNmbr = " + QuotedStr(tbLotLHPNo.Text) + " AND WOItemNo = " + QuotedStr(ddlLotWOItemNo.SelectedValue) + " AND RollNo = " + QuotedStr(lblUseRoll.Text), ViewState("DBConnection").ToString)

        '    FillCombo(txt, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
        '    txt.SelectedValue = ViewState("UseRollNo")
        '    txt.Focus()
        'Catch ex As Exception
        '    lbStatus.Text = "GridRollOutput_RowEditing exception : " + ex.ToString
        'End Try
    End Sub

    Protected Sub btnBackLot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackLot.Click
        Try
            MovePanel(pnlLotNo, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btnBackLot_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridRollOutput_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridRollOutput.RowUpdating
        Dim SQLString As String
        Dim dbName, dbStatus As DropDownList
        Dim lbRollNo As TextBox
        Dim dbCode As DropDownList
        Dim tblength, tbwidth, tbgsm, tbweight As TextBox
        Try
            dbName = GridRollOutput.Rows(e.RowIndex).FindControl("UseRollNoEdit")
            dbStatus = GridRollOutput.Rows(e.RowIndex).FindControl("StatusEdit")
            lbRollNo = GridRollOutput.Rows(e.RowIndex).FindControl("RollNoEdit")
            tblength = GridRollOutput.Rows(e.RowIndex).FindControl("LengthEdit")
            tbwidth = GridRollOutput.Rows(e.RowIndex).FindControl("WidthEdit")
            tbgsm = GridRollOutput.Rows(e.RowIndex).FindControl("GSMEdit")
            tbweight = GridRollOutput.Rows(e.RowIndex).FindControl("WeightRollEdit")

            'If dbName.Text.Trim.Length = 0 Then
            '    lbStatus.Text = "Use Roll No must be filled."
            '    dbName.Focus()
            '    Exit Sub
            'End If

            SQLString = "UPDATE PROResultRollOutput SET RollNo = " + QuotedStr(lbRollNo.Text) + _
            ", UseRollNo = " + QuotedStr(dbName.SelectedValue) + _
            ", Status = " + QuotedStr(dbStatus.SelectedValue) + _
            ", Length = " + QuotedStr(tblength.Text) + ", Width = " + QuotedStr(tbwidth.Text) + _
            ", GSM = " + QuotedStr(tbgsm.Text) + ", WeightRoll = " + QuotedStr(tbweight.Text) + _
            " WHERE TransNmbr  = " + QuotedStr(tbLotLHPNo.Text) + " AND WONo = " + QuotedStr(lbLotWONo.Text) + _
            " AND ItemNo = " + QuotedStr(lbLotItemNo.Text) + " AND RollNo = " + QuotedStr(ViewState("RollNo").ToString)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridRollOutput.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridRollOutput.EditIndex = -1
            lbStatus.Text = MessageDlg("Data Roll " + lbRollNo.Text + " Save Success")
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
            dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
            FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
        Catch ex As Exception
            lbStatus.Text = "GridRollOutput_RowUpdating Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Dim SQLString As String
        Dim GVR As GridViewRow
        Dim useRollNo, Status As DropDownList
        Dim Weight As TextBox
        Dim lblWeight, lbluseRollNo, lblStatus As Label

        Dim dbName, dbStatus As DropDownList
        Dim lbRollNo, tblength, tbwidth, tbgsm As Label
        Dim dbCode As DropDownList
        Dim tbweight As TextBox

        Try
            For Each GVR In GridRollOutput.Rows
                useRollNo = GVR.FindControl("UseRollNoEdit")
                lbluseRollNo = GVR.FindControl("UseRollNo")

                Status = GVR.FindControl("StatusEdit")
                lblStatus = GVR.FindControl("Status")

                Weight = GVR.FindControl("WeightRollEdit")
                lblWeight = GVR.FindControl("WeightRoll")

                dbName = GVR.FindControl("UseRollNoEdit")
                dbStatus = GVR.FindControl("StatusEdit")
                lbRollNo = GVR.FindControl("RollNo")
                tblength = GVR.FindControl("Length")
                tbwidth = GVR.FindControl("Width")
                tbgsm = GVR.FindControl("GSM")
                tbweight = GVR.FindControl("WeightRollEdit")

                If (useRollNo.SelectedItem.Text <> lbluseRollNo.Text) Or (Status.SelectedItem.Text <> lblStatus.Text) Or (Weight.Text <> lblStatus.Text) Then
                    SQLString = "UPDATE PROResultRollOutput SET RollNo = " + QuotedStr(lbRollNo.Text) + _
                                ", UseRollNo = " + QuotedStr(dbName.SelectedValue) + _
                                ", Status = " + QuotedStr(dbStatus.SelectedValue) + _
                                ", Length = " + QuotedStr(tblength.Text) + ", Width = " + QuotedStr(tbwidth.Text) + _
                                ", GSM = " + QuotedStr(tbgsm.Text) + ", WeightRoll = " + QuotedStr(tbweight.Text) + _
                                " WHERE TransNmbr  = " + QuotedStr(tbLotLHPNo.Text) + " AND WONo = " + QuotedStr(lbLotWONo.Text) + _
                                " AND ItemNo = " + QuotedStr(lbLotItemNo.Text) + " AND RollNo = " + QuotedStr(lbRollNo.Text)

                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                    GridRollOutput.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                    GridRollOutput.EditIndex = -1
                    lbStatus.Text = MessageDlg("Data Roll " + lbRollNo.Text + " Save Success")
                    bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
                    dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
                    FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
                End If
            Next
        Catch ex As Exception
            lbStatus.Text = "btnApply_Click Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridRollOutput_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridRollOutput.Sorting
        Try
            If ViewState("SortOrderOut") = Nothing Or ViewState("SortOrderOut") = "DESC" Then
                ViewState("SortOrderOut") = "ASC"
            Else
                ViewState("SortOrderOut") = "DESC"
            End If
            ViewState("SortExpressionOut") = e.SortExpression + " " + ViewState("SortOrderOut")
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
        Catch ex As Exception
            lbStatus.Text = "GridRollOutput_Sorting Eror =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt5.Click, btnAddDt5ke2.Click
        Try
            ClearDt5()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt5") = "Insert"
            MovePanel(pnlDt5, pnlEditDt5)
            EnableHd(False)
            StatusButtonSave(False)
            tbEmp.Focus()
        Catch ex As Exception
            lbStatus.Text = "btnAddDt5_Click error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Operator, Operator_Name, JobTitle, JobTtlName FROM V_PROResultGetEmp"
            ResultField = "Operator, Operator_Name, JobTitle, JobTtlName "
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmp_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt5.Click
        Try
            If CekDt5() = False Then
                btnSaveDt5.Focus()
                Exit Sub
            End If

            If ViewState("StateDt5") = "Edit" Then
                Dim Row As DataRow
                'lbStatus.Text = "edit"
                'Exit Sub
                Row = ViewState("Dt5").Select("EmpNumb = " + QuotedStr(tbEmp.Text))(0)
                Row.BeginEdit()
                Row("EmpNumb") = tbEmp.Text
                Row("Emp_Name") = tbEmpName.Text
                Row("JobTitle") = tbJobTitle.Text
                Row("JobTtlName") = tbJobTitleName.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                'lbStatus.Text = "insert"
                'Exit Sub
                dr = ViewState("Dt5").NewRow
                dr("EmpNumb") = tbEmp.Text
                dr("Emp_Name") = tbEmpName.Text
                dr("JobTitle") = tbJobTitle.Text
                dr("JobTtlName") = tbJobTitleName.Text
                ViewState("Dt5").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt5, pnlDt5)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            BindGridDt(ViewState("Dt5"), GridDt5)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt5 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub GridDt5_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt5.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt5.Rows(e.RowIndex)
            dr = ViewState("Dt5").Select("EmpNumb = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt5"), GridDt5)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 5 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt5_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt5.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt5.Rows(e.NewEditIndex)
            FillTextBoxDt5(GVR.Cells(1).Text)
            MovePanel(pnlDt5, pnlEditDt5)
            EnableHd(False)
            ViewState("StateDt5") = "Edit"
            StatusButtonSave(False)
            EnableDt5(True)
            tbEmp.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt5 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt5.Click
        Try
            MovePanel(pnlEditDt5, pnlDt5)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt5")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt5 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text, GVR.Cells(3).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData()
    End Sub

    Protected Sub ddlShowRecord2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord2.SelectedIndexChanged
        GridRollOutput.PageIndex = 0
        GridRollOutput.EditIndex = -1
        GridRollOutput.PageSize = ddlShowRecord2.SelectedValue
        bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    btnProcessDel.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridRollOutput, sender)
    End Sub
    Protected Sub btnProcessDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProcessDel.Click
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lb As Label
            Dim RollNo As String
            For Each GVR In GridRollOutput.Rows
                CB = GVR.FindControl("cbSelect")
                lb = GVR.FindControl("RollNo")
                
                If CB.Checked Then
                    
                    RollNo = lb.Text 'GVR.Cells(2).Text + "|" +
                    'Dim txtID As Label
                    Dim dbCode As DropDownList
                    Try
                        'If CheckMenuLevel("Delete") = False Then
                        '    Exit Sub
                        'End If
                        RollNo = lb.Text

                        SQLExecuteNonQuery("DELETE FROM PROResultRollOutput WHERE TransNmbr  = " + QuotedStr(tbLotLHPNo.Text) + _
                                           " AND WONo = " + QuotedStr(lbLotWONo.Text) + " AND ItemNo = " + QuotedStr(lbLotItemNo.Text) + _
                                           " AND RollNo = " + QuotedStr(lb.Text), ViewState("DBConnection"))
                        bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
                        dbCode = GridRollOutput.FooterRow.FindControl("UseRollNoAdd")
                        FillCombo(dbCode, "EXEC S_PROResultGetUseRollNo " + QuotedStr(tbLotLHPNo.Text) + "," + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
                    Catch ex As Exception
                        lbStatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try

                End If
            Next
            lbStatus.Text = "Delete Selected Data Success"
            'bindDataDeleteAll()
            bindDataGridOutput(tbLotLHPNo.Text, ddlLotWOItemNo.SelectedValue)
        Catch ex As Exception
            Throw New Exception("btnProcesDel_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetOperator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetOperator.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Operator, Operator_Name, JobTitle, JobTtlName FROM V_PROResultGetEmp"
            ResultField = "Operator, Operator_Name, JobTitle, JobTtlName"
            ViewState("Sender") = "btnGetOperator"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetOperator Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetWaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetWaste.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsWaste "
            ResultField = "Waste_Code, Waste_Name"
            ViewState("Sender") = "btnGetWaste"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetWaste Error : " + ex.ToString
        End Try
    End Sub
End Class


