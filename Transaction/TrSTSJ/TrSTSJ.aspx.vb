Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrSTSJ_TrSTSJ
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_STSJHd"


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLocation") = False
                SetInit()
                Session("AdvanceFilter") = ""
                lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT DO_No) from V_STSJGetDODt", ViewState("DBConnection").ToString)
                ddlRow.SelectedValue = "15"

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
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCustomer" Then
                    BindToText(tbCustCode, Session("Result")(0).ToString)
                    BindToText(tbCustName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnDONo" Then
                    If tbCustCode.Text.Trim = "" Then
                        BindToText(tbCustCode, Session("Result")(2).ToString)
                        BindToText(tbCustName, Session("Result")(3).ToString)
                    Else
                        If tbCustCode.Text <> Session("Result")(2).ToString Then
                            Exit Sub
                        End If
                    End If
                    BindToText(tbDONo, Session("Result")(0).ToString)
                    BindToText(tbSONo, Session("Result")(4).ToString)
                    BindToText(tbCustPONo, Session("Result")(5).ToString)
                    BindToText(tbDeliveryTime, Session("Result")(6).ToString)
                End If
                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        '"Product, ProductName, Specification, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, NetWeight, GrossWeight, Remark "
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("Product_Name") = TrimStr(drResult("ProductName").ToString)
                            dr("Specification") = TrimStr(drResult("Specification").ToString)
                            dr("ProductGroup_Code") = drResult("ProductGroup_Code")
                            dr("Location") = ""
                            dr("QtyOrder") = drResult("QtyOrder")
                            dr("UnitOrder") = drResult("UnitOrder")
                            dr("Qty") = drResult("QtyWrhs")
                            dr("Unit") = drResult("UnitWrhs")
                            dr("QtyM2") = drResult("QtyM2")
                            dr("QtyRoll") = drResult("QtyRoll")
                            dr("UnitPacking") = drResult("UnitPack")
                            Dim dt As New DataTable
                            dt = SQLExecuteQuery("EXEC S_STSJGetQtyPack " + QuotedStr(drResult("Product")) + "," + QuotedStr(drResult("UnitWrhs")) + "," + QuotedStr(dr("UnitPacking")) + "," + (0).ToString + "," + (CFloat(drResult("QtyWrhs"))).ToString, ViewState("DBConnection").ToString).Tables(0)
                            dr("QtyPerPack") = FormatNumber(dt.Rows(0)("QtyPerPack").ToString, ViewState("DigitQty"))
                            dr("QtyPacking") = FormatNumber(dt.Rows(0)("QtyPack").ToString, ViewState("DigitQty"))
                            dr("NetWeight") = drResult("NetWeight")
                            dr("GrossWeight") = drResult("GrossWeight")
                            dr("Remark") = TrimStr(drResult("Remark").ToString)
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(ViewState("Dt").Rows.count = 0)
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing
                End If
                If ViewState("Sender") = "btnSubLed" Then
                    tbSubledCode.Text = Session("Result")(0).ToString
                    tbSubledName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnVehicle" Then
                    tbVehicleCode.Text = Session("Result")(0).ToString
                    tbVehicleName.Text = Session("Result")(1).ToString
                    If Session("Result")(2).ToString = "Supplier" Then
                        ddlDeliveryBy.SelectedValue = "Expedition"
                    Else
                        ddlDeliveryBy.SelectedValue = "Company"
                    End If
                    If ddlDeliveryBy.SelectedItem.Text = "Expedition" Then
                        ddlDeliveryCostBy.Enabled = True
                    Else
                        ddlDeliveryCostBy.Enabled = False
                        ddlDeliveryCostBy.SelectedValue = ""
                    End If
                End If
                If ViewState("Sender") = "btnProduct" Then
                    'ResultField = "Product, ProductName, Specification, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, NetWeight, GrossWeight, 
                    'Remark, UnitPack, ProductGroup_Code "
                    BindToText(tbProduct, Session("Result")(0).ToString)
                    BindToText(tbProductName, Session("Result")(1).ToString)
                    BindToText(tbSpec, Session("Result")(2).ToString)
                    BindToText(tbQtyOrder, Session("Result")(3).ToString)
                    ddlUnitOrder.SelectedValue = Session("Result")(4).ToString
                    BindToText(tbQtyWrhs, Session("Result")(5).ToString)
                    ddlUnitWrhs.SelectedValue = Session("Result")(6).ToString
                    BindToText(tbQtyM2, Session("Result")(7).ToString)
                    BindToText(tbQtyRoll, Session("Result")(8).ToString)
                    BindToText(tbNetWeight, Session("Result")(9).ToString)
                    BindToText(tbGrossWeight, Session("Result")(10).ToString)
                    BindToText(tbRemarkDt, Session("Result")(11).ToString)
                    BindToDropList(ddlUnitPacking, Session("Result")(12).ToString)
                    'ddlUnitPacking.SelectedValue = SQLExecuteScalar("EXEC S_STSJFindUnitPacking " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue), ViewState("DBConnection").ToString)
                    Dim dt As New DataTable
                    dt = SQLExecuteQuery("EXEC S_STSJGetQtyPack " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + QuotedStr(ddlUnitPacking.SelectedValue) + "," + (CFloat(tbQtyPerPack.Text)).ToString + "," + (CFloat(tbQtyWrhs.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
                    If CFloat(tbQtyPerPack.Text) = 0 Then
                        tbQtyPerPack.Text = FormatNumber(dt.Rows(0)("QtyPerPack").ToString, ViewState("DigitQty"))
                    End If
                    tbQtyPacking.Text = FormatNumber(dt.Rows(0)("QtyPack").ToString, ViewState("DigitQty"))
                    GetInfo(tbProduct.Text)
                    lbProdGrp.Text = Session("Result")(13).ToString
                End If
                If ViewState("Sender") = "btnRRVehicle" Then
                    tbRRVehicleCode.Text = Session("Result")(0).ToString
                    tbRRVehicleName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    Dim Qty As Double
                    For Each drResult In Session("Result").Rows
                        'insert
                        ' "DO_No, DO_Date, Revisi, Status, FgActive, DeliveryDate, Customer_Code, Customer_Name, SO_No, Cust_PO_No, Delivery_Code, Delivery_Name, Delivery_Addr, Delivery_City, Delivery_Date, Delivery_Hour, Remark_Hd, SO_Delivery, Remark"
                        If FirstTime Then
                            BindToText(tbDONo, drResult("DO_No"))
                            ddlReport.SelectedValue = "Y"
                            BindToText(tbCustCode, drResult("Customer_Code"))
                            BindToText(tbCustName, drResult("Customer_Name"))
                            BindToText(tbSONo, drResult("SO_No"))
                            BindToText(tbCustPONo, drResult("Cust_PO_No"))
                            BindToText(tbRemark, drResult("Remark_Hd"))
                        End If
                        Qty = 0
                        ' "Product, ProductName, Specification, NetWeight, GrossWeight, UnitPack, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll"
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("Product_Name") = drResult("ProductName")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Location") = ""
                            dr("QtyOrder") = drResult("QtyOrder")
                            dr("UnitOrder") = drResult("UnitOrder")
                            dr("Qty") = drResult("QtyWrhs")
                            dr("Unit") = drResult("UnitWrhs")
                            dr("QtyM2") = drResult("QtyM2")
                            dr("QtyRoll") = FormatNumber(drResult("QtyRoll").ToString, ViewState("DigitQty"))
                            dr("UnitPacking") = drResult("UnitPack")
                            Dim dt As New DataTable
                            dt = SQLExecuteQuery("EXEC S_STSJGetQtyPack " + QuotedStr(drResult("Product")) + "," + QuotedStr(drResult("UnitWrhs")) + "," + QuotedStr(dr("UnitPacking")) + "," + (0).ToString + "," + (CFloat(drResult("QtyWrhs"))).ToString, ViewState("DBConnection").ToString).Tables(0)
                            dr("QtyPerPack") = FormatNumber(dt.Rows(0)("QtyPerPack").ToString, ViewState("DigitQty"))
                            dr("QtyPacking") = FormatNumber(dt.Rows(0)("QtyPack").ToString, ViewState("DigitQty"))
                            dr("NetWeight") = drResult("NetWeight")
                            dr("GrossWeight") = drResult("GrossWeight")
                            dr("Remark") = drResult("Remark")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next

                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error : " + ex.ToString
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
            FillRange(ddlRange)
            FillCombo(ddlwrhs, "EXEC S_GetWrhsUserRR " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitWrhs, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitOrder, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitPacking, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("i") = 0
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))


            tbQtyM2.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyRoll.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyWrhs.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyOrder.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyPacking.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyPerPack.Attributes.Add("OnBlur", "setformatdt();")
            tbNetWeight.Attributes.Add("OnBlur", "setformatdt();")
            tbGrossWeight.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyM2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyRoll.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyWrhs.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyOrder.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyPerPack.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbNetWeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGrossWeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
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
                ViewState("SortExpression") = "TransNmbr DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_STSJDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlReport.Enabled = State
            tbCustCode.Enabled = State
            btnCust.Visible = State
            btnDONo.Visible = State
            ddlwrhs.Enabled = State
            tbSubledCode.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            btnSubled.Visible = tbSubledCode.Enabled
            'btnGetData.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDtExtended()
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
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

    Private Function AllowedRecord() As Integer
        Try
            If ViewState("Product") = tbProduct.Text Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function


    Private Sub SaveAll()
        Dim SQLString, CekTrans As String
        Dim I As Integer
        Try
            If pnlEditDt.Visible = True Then
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
                tbCode.Text = GetAutoNmbr("SJ", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                CekTrans = SQLExecuteScalar("SELECT COUNT(TransNmbr) FROM STCSJHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Delivery Note No. " + tbCode.Text + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO STCSJHd (TransNmbr, Status, TransDate, FgReport," + _
                "DONo, Customer, SONo, CustPONo, " + _
                "Warehouse, FgSubLed, SubLed, " + _
                "IssuedBy, CarNo, Driver, " + _
                "DeliveryTime, DeliveryBy, DeliveryCostBy, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'," + QuotedStr(ddlReport.SelectedValue) + "," + _
                QuotedStr(tbDONo.Text) + "," + QuotedStr(tbCustCode.Text) + "," + QuotedStr(tbSONo.Text) + "," + QuotedStr(tbCustPONo.Text) + "," + _
                QuotedStr(ddlwrhs.SelectedValue) + "," + QuotedStr(tbFgSubLed.Text) + "," + QuotedStr(tbSubledCode.Text) + "," + _
                QuotedStr(tbIssuedBy.Text) + "," + QuotedStr(tbVehicleCode.Text) + "," + QuotedStr(tbDriver.Text) + "," + _
                QuotedStr(tbDeliveryTime.Text) + "," + QuotedStr(ddlDeliveryBy.SelectedValue) + "," + QuotedStr(ddlDeliveryCostBy.SelectedValue) + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                ViewState("TransNmbr") = tbCode.Text
            Else
                SQLString = "UPDATE STCSJHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Customer = " + QuotedStr(tbCustCode.Text) + _
                ", DONo = " + QuotedStr(tbDONo.Text) + ", SONo = " + QuotedStr(tbSONo.Text) + _
                ", CustPONo = " + QuotedStr(tbCustPONo.Text) + _
                ", Warehouse = " + QuotedStr(ddlwrhs.SelectedValue) + _
                ", FgSubLed = " + QuotedStr(tbFgSubLed.Text) + ", SubLed = " + QuotedStr(tbSubledCode.Text) + _
                ", IssuedBy = " + QuotedStr(tbIssuedBy.Text) + ", CarNo = " + QuotedStr(tbVehicleCode.Text) + _
                ", Driver = " + QuotedStr(tbDriver.Text) + ", DeliveryTime = " + QuotedStr(tbDeliveryTime.Text) + _
                ", DeliveryBy = " + QuotedStr(ddlDeliveryBy.SelectedValue) + _
                ", DeliveryCostBy = " + QuotedStr(ddlDeliveryCostBy.SelectedValue) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Location, QtyOrder, UnitOrder, Qty, Unit, " + _
                                         "QtyM2, QtyRoll, QtyPacking, UnitPacking, QtyPerPack, NetWeight, GrossWeight, Remark " + _
                                         " FROM STCSJDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' ", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE STCSJDt SET Product = @Product, Location = @Location, " + _
            "QtyOrder = @QtyOrder, UnitOrder = @UnitOrder, Qty = @Qty, Unit = @Unit, " + _
            "QtyM2 = @QtyM2, QtyRoll = @QtyRoll, QtyPacking = @QtyPacking, UnitPacking = @UnitPacking, " + _
            "QtyPerPack = @QtyPerPack, NetWeight = @NetWeight, GrossWeight = @GrossWeight, Remark = @Remark " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @OldProduct AND Location = @OldLocation ", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Location", SqlDbType.VarChar, 20, "Location")
            Update_Command.Parameters.Add("@QtyOrder", SqlDbType.Float, 18, "QtyOrder")
            Update_Command.Parameters.Add("@UnitOrder", SqlDbType.VarChar, 5, "UnitOrder")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 22, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@QtyM2", SqlDbType.Float, 18, "QtyM2")
            Update_Command.Parameters.Add("@QtyRoll", SqlDbType.Float, 18, "QtyRoll")
            Update_Command.Parameters.Add("@QtyPacking", SqlDbType.Float, 18, "QtyPacking")
            Update_Command.Parameters.Add("@UnitPacking", SqlDbType.VarChar, 5, "UnitPacking")
            Update_Command.Parameters.Add("@QtyPerPack", SqlDbType.Float, 18, "QtyPerPack")
            Update_Command.Parameters.Add("@NetWeight", SqlDbType.Float, 22, "NetWeight")
            Update_Command.Parameters.Add("@GrossWeight", SqlDbType.Float, 22, "GrossWeight")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldLocation", SqlDbType.VarChar, 20, "Location")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM STCSJDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @Product AND Location = @Location ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Location", SqlDbType.VarChar, 20, "Location")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("STCSJDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ClearHd()
            Cleardt()

            pnlDt.Visible = True
            BindDataDt("")

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = Now.Date
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbDONo.Text = ""
            tbSONo.Text = ""
            tbCustPONo.Text = ""
            ddlwrhs.SelectedIndex = 0
            ddlwrhs.Enabled = True
            tbFgSubLed.Text = "N"
            tbSubledCode.Text = ""
            tbSubledCode.Enabled = tbFgSubLed.Text <> "N"
            btnSubled.Visible = tbSubledCode.Enabled
            tbSubledName.Text = ""
            tbVehicleCode.Text = ""
            tbVehicleName.Text = ""
            tbIssuedBy.Text = ViewState("UserId")
            tbDriver.Text = ""
            ddlDeliveryBy.SelectedItem.Text = "Company"
            ddlDeliveryCostBy.Enabled = ddlDeliveryBy.SelectedItem.Text = "Expedition"
            tbDeliveryTime.Text = "00:00"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProduct.Text = ""
            tbProductName.Text = ""
            tbSpec.Text = ""
            ddlLocation.SelectedValue = ""
            ddlUnitWrhs.SelectedValue = ""
            ddlUnitOrder.SelectedValue = ""
            ddlUnitPacking.SelectedValue = ""
            tbQtyOrder.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyM2.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyRoll.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyWrhs.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyPacking.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyPerPack.Text = FormatFloat("0", ViewState("DigitQty"))
            tbNetWeight.Text = "0"
            tbGrossWeight.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub


    Function CekHd() As Boolean
        Try
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If tbCustName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value. ")
                tbCustCode.Focus()
                Return False
            End If
            If tbDONo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("DO No must have value. ")
                btnDONo.Focus()
                Return False
            End If
            If ddlwrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlwrhs.Focus()
                Return False
            End If
            If tbVehicleCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Vehicle must have value. ")
                tbVehicleCode.Focus()
                Return False
            End If
            If (tbFgSubLed.Text.Trim <> "N") And (tbSubledName.Text.Trim = "") Then
                lbStatus.Text = MessageDlg("Subled must have value. ")
                tbSubledCode.Focus()
                Return False
            End If
            If (ddlDeliveryBy.SelectedItem.Text.Trim = "Expedition") And (ddlDeliveryCostBy.SelectedValue.Trim = "") Then
                lbStatus.Text = MessageDlg("Delivery Cost By must have value. ")
                ddlDeliveryCostBy.Focus()
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
                If Dr("Location").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
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
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Warehouse Must Have Value")
                    Return False
                End If
                'If CFloat(Dr("QtyM2").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Qty M2 Must Have Value")
                '    Return False
                'End If
                'If CFloat(Dr("QtyRoll").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Qty Roll Must Have Value")
                '    Return False
                'End If
                'If CFloat(Dr("QtyPerPack").ToString) < 0 Then '05052014-302
                If CFloat(Dr("QtyPerPack").ToString) <= 0 Then '05052014-302
                    lbStatus.Text = MessageDlg("Qty Per Pack Must Have Value")
                    Return False
                End If
                If CFloat(Dr("NetWeight").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Net Weight Must Have Value")
                    Return False
                End If
                If CFloat(Dr("GrossWeight").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Gross Weight Must Have Value")
                    Return False
                End If
            Else
                If tbProduct.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If
                If ddlLocation.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    ddlLocation.Focus()
                    Return False
                End If
                If CFloat(tbQtyOrder.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    tbQtyOrder.Focus()
                    Return False
                End If
                If ddlUnitOrder.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    ddlUnitOrder.Focus()
                    Return False
                End If
                If CFloat(tbQtyWrhs.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    tbQtyWrhs.Focus()
                    Return False
                End If
                If ddlUnitWrhs.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Warehouse Must Have Value")
                    ddlUnitWrhs.Focus()
                    Return False
                End If
                'If CFloat(tbQtyM2.Text) <= 0 Then
                '    lbStatus.Text = MessageDlg("Qty M2 Must Have Value")
                '    tbQtyM2.Focus()
                '    Return False
                'End If
                'If CFloat(tbQtyRoll.Text) <= 0 Then
                '    lbStatus.Text = MessageDlg("Qty Roll Must Have Value")
                '    tbQtyRoll.Focus()
                '    Return False
                'End If
                'If CFloat(tbQtyPerPack.Text) < 0 Then '05052014-302
                If CFloat(tbQtyPerPack.Text) <= 0 Then '05052014-302
                    lbStatus.Text = MessageDlg("Qty Per Pack Must Have Value")
                    tbQtyPerPack.Focus()
                    Return False
                End If
                If CFloat(tbNetWeight.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Net Weight Must Have Value")
                    tbNetWeight.Focus()
                    Return False
                End If
                If CFloat(tbGrossWeight.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Gross Weight Must Have Value")
                    tbGrossWeight.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
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
            If e.CommandName = "Go" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    pnlDt.Visible = True
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    'btnGetData.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        pnlDt.Visible = True
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'btnGetData.Visible = True
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Delete" Then
                    Dim SqlString, Result As String

                    If Not GVR.Cells(3).Text = "H" Then
                        lbStatus.Text = MessageDlg("Data Must be Hold Before Deleted")
                        Exit Sub
                    End If

                    'lbStatus.Text = ConfirmDlg("Sure to delete this data?")    

                    SqlString = "Declare @A VarChar(255) EXEC S_STSJDelete " + QuotedStr(GVR.Cells(2).Text) + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                    'SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                    BindData(Session("AdvanceFilter"))
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "H") Or (GVR.Cells(3).Text = "G") Then
                            Session("DBConnection") = ViewState("DBConnection")
                            Session("SelectCommand") = "EXEC S_STFormSJ ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(ViewState("UserId"))
                            Session("ReportFile") = ".../../../Rpt/FormDN.frx"
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            lbStatus.Text = MessageDlg("Data must be Hold or Get Approval or Posted to Print")
                            Exit Sub
                        End If

                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Complete" Then
                    Try
                        If GVR.Cells(3).Text = "P" Then
                            ViewState("TransNmbr") = GVR.Cells(2).Text
                            MovePanel(PnlHd, pnlComplete)
                            If GVR.Cells(18).Text = "Expedition" Then 'DeliveryBy
                                tbRRExpeditionName.Enabled = True
                                tbRRExpeditionNo.Enabled = True
                            Else
                                tbRRExpeditionName.Enabled = False
                                tbRRExpeditionNo.Enabled = False
                            End If
                            FillTextBoxCompleteHd(ViewState("TransNmbr"))
                            BindDataComplete(ViewState("TransNmbr"))
                        Else
                            lbStatus.Text = MessageDlg("Data must be Post to complete")
                            Exit Sub
                        End If
                    Catch ex As Exception
                        lbStatus.Text = "btn Complete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Un-Complete" Then
                    Try
                        If GVR.Cells(3).Text = "F" Then
                            Dim SqlString, Result As String

                            SqlString = "Declare @A VarChar(255) EXEC S_STSJUnComplete " + QuotedStr(GVR.Cells(2).Text) + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
                            Result = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + " <br/>"
                            End If
                            'SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                            BindData(Session("AdvanceFilter"))
                        Else
                            lbStatus.Text = MessageDlg("Data must be Complete to Un-complete")
                            Exit Sub
                        End If
                    Catch ex As Exception
                        lbStatus.Text = "btn Complete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Cancel" Then
                    Try
                        If GVR.Cells(3).Text = "P" Then
                            ViewState("TransNmbr") = GVR.Cells(2).Text
                            BindToDate(tbCancelDate, Now.Date)
                            tbReasonCancel.Text = ""
                            MovePanel(PnlHd, pnlCancel)
                        Else
                            lbStatus.Text = MessageDlg("Data must be Post to Cancel")
                            Exit Sub
                        End If
                    Catch ex As Exception
                        lbStatus.Text = "btn Cancel Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String

                    Try
                        paramgo = "SJ|" + GVR.Cells(2).Text
                        'lbStatus.Text = paramgo
                        'Exit Sub
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'SJ' "
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

    Protected Sub OnConfirm(ByVal sender As Object, ByVal e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
            execBtnGo(BtnGo)
        ElseIf confirmValue = "No" Then
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked NO!')", True)
            execBtnGo(BtnGo)
        End If
    End Sub

    Protected Sub OnConfirm2(ByVal sender As Object, ByVal e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
            execBtnGo(btnGo2)
        ElseIf confirmValue = "No" Then
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked NO!')", True)
            execBtnGo(btnGo2)
        End If
    End Sub
  
    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim lb As Label
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            lb = GVR.FindControl("lbLocation")
            dr = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(GVR.Cells(1).Text + "|" + TrimStr(lb.Text)))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            lb = GVR.FindControl("lbLocation")
            'ViewState("Product") = GVR.Cells(1).Text + "|" + GVR.Cells(4).Text 'Product Code | Location Code
            ViewState("Product") = GVR.Cells(1).Text + "|" + TrimStr(lb.Text) 'Product Code | Location Code
            If ViewState("SetLocation") Then
                FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                ViewState("SetLocation") = False
            End If
            ViewState("StateDt") = "Edit"
            FillTextBoxDt(ViewState("Product"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            tbProduct.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Taon As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Taon), ViewState("DBConnection").ToString)
            tbCode.Text = Taon
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbCode, Dt.Rows(0)("TransNmbr").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbDONo, Dt.Rows(0)("DONo").ToString)
            BindToText(tbSONo, Dt.Rows(0)("SONo").ToString)
            BindToText(tbCustPONo, Dt.Rows(0)("CustPONo").ToString)
            BindToDropList(ddlwrhs, Dt.Rows(0)("Warehouse_Code").ToString)
            BindToText(tbFgSubLed, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbSubledCode, Dt.Rows(0)("SubLed_Code").ToString)
            BindToText(tbSubledName, Dt.Rows(0)("SubLed_Name").ToString)
            BindToText(tbIssuedBy, Dt.Rows(0)("Issued_By").ToString)
            BindToText(tbVehicleCode, Dt.Rows(0)("CarNo").ToString)
            BindToText(tbVehicleName, Dt.Rows(0)("VehicleName").ToString)
            BindToText(tbDriver, Dt.Rows(0)("Driver").ToString)
            If Trim(Dt.Rows(0)("DeliveryTime").ToString) = "" Then
                tbDeliveryTime.Text = "00:00"
            Else
                BindToText(tbDeliveryTime, Dt.Rows(0)("DeliveryTime").ToString)
            End If
            BindToDropList(ddlDeliveryBy, Dt.Rows(0)("DeliveryBy").ToString)
            BindToDropList(ddlDeliveryCostBy, Dt.Rows(0)("DeliveryCostBy").ToString)
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
                BindToText(tbProduct, Dr(0)("Product").ToString)
                GetInfo(tbProduct.Text)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbSpec, Dr(0)("Specification").ToString)
                lbProdGrp.Text = Dr(0)("ProductGroup_Code").ToString
                BindToDropList(ddlLocation, Dr(0)("Location").ToString)
                BindToText(tbQtyOrder, Dr(0)("QtyOrder").ToString)
                BindToDropList(ddlUnitOrder, Dr(0)("UnitOrder").ToString)
                BindToText(tbQtyWrhs, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitWrhs, Dr(0)("Unit").ToString)
                BindToText(tbQtyM2, Dr(0)("QtyM2").ToString)
                BindToText(tbQtyRoll, Dr(0)("QtyRoll").ToString)
                BindToText(tbQtyPacking, Dr(0)("QtyPacking").ToString)
                BindToDropList(ddlUnitPacking, Dr(0)("UnitPacking").ToString)
                BindToText(tbQtyPerPack, Dr(0)("QtyPerPack").ToString)
                BindToText(tbNetWeight, Dr(0)("NetWeight").ToString)
                BindToText(tbGrossWeight, Dr(0)("GrossWeight").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProduct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProduct.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQL As String
        Try
            SQL = "EXEC S_STSJGetDODt " + QuotedStr(tbDONo.Text) + ", " + QuotedStr(tbProduct.Text.Trim)

            Dt = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProduct.Text = Dr("Product").ToString
                tbProductName.Text = TrimStr(Dr("ProductName").ToString)
                tbSpec.Text = TrimStr(Dr("Specification").ToString)
                ddlUnitWrhs.SelectedValue = Dr("UnitWrhs")
                ddlUnitOrder.SelectedValue = Dr("UnitOrder")
                tbQtyOrder.Text = FormatFloat(Dr("QtyOrder"), ViewState("DigitQty"))
                tbQtyWrhs.Text = FormatFloat(Dr("QtyWrhs"), ViewState("DigitQty"))
                tbQtyM2.Text = FormatFloat(Dr("QtyM2"), ViewState("DigitQty"))
                tbQtyRoll.Text = FormatFloat(Dr("QtyRoll"), ViewState("DigitQty"))
                ddlUnitPacking.SelectedValue = SQLExecuteScalar("EXEC S_STSJFindUnitPacking " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue), ViewState("DBConnection").ToString)
                Dim dt2 As New DataTable
                dt2 = SQLExecuteQuery("EXEC S_STSJGetQtyPack " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + QuotedStr(ddlUnitPacking.SelectedValue) + "," + (CFloat(tbQtyPerPack.Text)).ToString + "," + (CFloat(tbQtyWrhs.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
                If CFloat(tbQtyPerPack.Text) = 0 Then
                    tbQtyPerPack.Text = FormatNumber(dt2.Rows(0)("QtyPerPack").ToString, ViewState("DigitQty"))
                End If
                tbQtyPacking.Text = FormatNumber(dt2.Rows(0)("QtyPack").ToString, ViewState("DigitQty"))
                GetInfo(tbProduct.Text)
                lbProdGrp.Text = Dr("ProductGroup_Code").ToString
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbSpec.Text = ""
                ddlUnitWrhs.SelectedValue = ""
                ddlUnitOrder.SelectedValue = ""
                tbQtyWrhs.Text = FormatNumber("0", ViewState("DigitQty"))
                tbQtyOrder.Text = FormatNumber("0", ViewState("DigitQty"))
                tbQtyM2.Text = FormatNumber("0", ViewState("DigitQty"))
                tbQtyRoll.Text = FormatNumber("0", ViewState("DigitQty"))
                ddlUnitPacking.SelectedValue = ""
                tbQtyPerPack.Text = FormatNumber("0", ViewState("DigitQty"))
                tbQtyPacking.Text = FormatNumber("0", ViewState("DigitQty"))
                PnlInfo.Visible = False
                lbProdGrp.Text = ""
            End If            
            tbProduct.Focus()
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub BindGridDtExtended()
        Try
            BindGridDt(ViewState("Dt"), GridDt)
            'If GetCountRecord(ViewState("Dt")) > 0 Then
            '    GridDt.Columns(1).Visible = True
            'Else
            '    GridDt.Columns(1).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("BindGridDtExtended Error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            
            MovePanel(PnlHd, pnlInput)
            pnlDt.Visible = True
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            ViewState("TransNmbr") = Now.Year.ToString
            newTrans()
            btnHome.Visible = False
            'btnGetData.Visible = True
            tbCode.Focus()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
        'Dim Status As String
        'Dim Result, ListSelectNmbr, ActionValue As String
        'Dim Nmbr(100) As String
        'Dim j As Integer
        'Try
        '    If sender.ID.ToString = "BtnGo" Then
        '        ActionValue = ddlCommand.SelectedValue
        '    Else
        '        ActionValue = ddlCommand2.SelectedValue
        '    End If
        '    Status = CekStatus(ActionValue)

        '    ListSelectNmbr = ""
        '    '3 = status, 2 & 3 = key, 
        '    GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)

        '    If ListSelectNmbr = "" Then Exit Sub
        '    For j = 0 To (Nmbr.Length - 1)
        '        If Nmbr(j) = "" Then
        '            Exit For
        '        Else

        '            Result = ExecSPCommandGo(ActionValue, "S_STSJ", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

        '            If Trim(Result) <> "" Then
        '                lbStatus.Text = lbStatus.Text + Result + " <br/>"
        '            End If
        '        End If
        '    Next
        '    BindData("TransNmbr in (" + ListSelectNmbr + ")")
        'Catch ex As Exception
        '    lbStatus.Text = "BtnGo_Click Error : " + ex.ToString
        'End Try
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            GridView1.PageSize = ddlRow.SelectedValue
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True

        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField As String
        Dim CriteriaField As String
        Try
            If Not CekHd() Then
                Exit Sub
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("Result") = Nothing

            Session("Filter") = "EXEC S_STSJGetDODt " + QuotedStr(tbDONo.Text) + ", '' "
            ResultField = "Product, ProductName, Specification, ProductGroup_Code, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, NetWeight, GrossWeight, Remark, UnitPack "
            CriteriaField = "Product, ProductName, Specification, ProductGroup_Code, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, NetWeight, GrossWeight, Remark, UnitPack "
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnGetData"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdddt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Try
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
            tbProduct.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
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
            If CekDt() = False Then
                Exit Sub
            End If

            SaveAll()
            newTrans()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            Dim ExistRow As DataRow()
            Dim SqlString, Result As String

            ExistRow = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(tbProduct.Text + "|" + ddlLocation.SelectedValue))

            'SqlString = "SELECT dbo.CekStockMinus ('N', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbProduct.Text) + ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubledCode.Text) + ", " + QuotedStr(ddlLocation.SelectedValue) + ", " + tbQtyWrhs.Text.Replace(",", "") + ") "
            'Result = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
            'If Trim(Result) <> "" Then
            '    lbStatus.Text = MessageDlg("Save failed " + Result)
            '    Exit Sub
            'End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                'If ExistRow.Count > AllowedRecord() Then
                '    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                '    Exit Sub
                'End If

                Row = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(ViewState("Product")))(0)

                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("Product") = tbProduct.Text
                Row("Product_Name") = tbProductName.Text
                Row("Specification") = tbSpec.Text
                Row("Location") = ddlLocation.SelectedValue
                Row("Location_Name") = ddlLocation.SelectedItem.Text
                Row("QtyOrder") = tbQtyOrder.Text
                Row("UnitOrder") = ddlUnitOrder.SelectedValue
                Row("Qty") = tbQtyWrhs.Text
                Row("Unit") = ddlUnitWrhs.SelectedValue
                Row("QtyM2") = tbQtyM2.Text
                Row("QtyRoll") = tbQtyRoll.Text
                Row("QtyPacking") = tbQtyPacking.Text
                Row("UnitPacking") = ddlUnitPacking.SelectedValue
                Row("QtyPerPack") = tbQtyPerPack.Text
                Row("NetWeight") = tbNetWeight.Text
                Row("GrossWeight") = tbGrossWeight.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                ViewState("Product") = Nothing
            Else
                'Insert
                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("Dt").NewRow
                dr("Product") = tbProduct.Text
                dr("Product_Name") = tbProductName.Text
                dr("Specification") = tbSpec.Text
                dr("Location") = ddlLocation.SelectedValue
                dr("Location_Name") = ddlLocation.SelectedItem.Text
                dr("QtyOrder") = CFloat(tbQtyOrder.Text)
                dr("UnitOrder") = ddlUnitOrder.SelectedValue
                dr("Qty") = CFloat(tbQtyWrhs.Text)
                dr("Unit") = ddlUnitWrhs.SelectedValue
                dr("QtyM2") = CFloat(tbQtyM2.Text)
                dr("QtyRoll") = CFloat(tbQtyRoll.Text)
                dr("QtyPacking") = CFloat(tbQtyPacking.Text)
                dr("UnitPacking") = ddlUnitPacking.SelectedValue
                dr("QtyPerPack") = CFloat(tbQtyPerPack.Text)
                dr("NetWeight") = CFloat(tbNetWeight.Text)
                dr("GrossWeight") = CFloat(tbGrossWeight.Text)
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)

            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDtExtended()
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
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
            If lbStatus.Text.Length > 0 Then Exit Sub
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn Home Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("Filter") = "EXEC S_STSJGetDODt " + QuotedStr(tbDONo.Text) + ", '' "
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Product, ProductName, Specification, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, NetWeight, GrossWeight, Remark, UnitPack, ProductGroup_Code "
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select Customer_Code, Customer_Name from VMsCustomer "
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Customer_Code, Customer_Name"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code")
                tbCustName.Text = Dr("Customer_Name")
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
            End If
            tbDONo.Text = ""
            tbSONo.Text = ""
            tbCustPONo.Text = ""
            tbDeliveryTime.Text = "00:00"
            tbCustCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try
    End Sub


    'Protected Sub tbQtyOrder_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOrder.TextChanged, btnQtyOrder.Click
    '    Try
    '        Dim dt As New DataTable
    '        dt = SQLExecuteQuery("EXEC S_STSJGetQtyDO " + QuotedStr(tbDONo.Text) + "," + QuotedStr(tbProduct.Text) + "," + (CFloat(tbQtyOrder.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)

    '        tbQtyWrhs.Text = FormatNumber(dt.Rows(0)("QtyWrhsSJ").ToString, ViewState("DigitQty"))
    '        tbQtyM2.Text = FormatNumber(dt.Rows(0)("QtyM2SJ").ToString, ViewState("DigitQty"))
    '        tbQtyRoll.Text = FormatNumber(dt.Rows(0)("QtyRollSJ").ToString, ViewState("DigitQty"))

    '        Dim dt2 As New DataTable
    '        dt2 = SQLExecuteQuery("EXEC S_STSJGetQtyPack " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + QuotedStr(ddlUnitPacking.SelectedValue) + "," + (CFloat(tbQtyPerPack.Text)).ToString + "," + (CFloat(tbQtyWrhs.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
    '        If CFloat(tbQtyPerPack.Text) = 0 Then
    '            tbQtyPerPack.Text = FormatNumber(dt2.Rows(0)("QtyPerPack").ToString, ViewState("DigitQty"))
    '        End If
    '        tbQtyPacking.Text = FormatNumber(dt2.Rows(0)("QtyPack").ToString, ViewState("DigitQty"))
    '    Catch ex As Exception
    '        Throw New Exception("tbQtyOrder_TextChanged Error : " + ex.ToString)
    '    End Try

    'End Sub

    Protected Sub btnDONo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDONo.Click
        Dim ResultField As String
        Try
            If tbCustCode.Text.Trim = "" Then
                Session("filter") = "Select DISTINCT* from V_STSJGetDOHd"
            Else
                Session("filter") = "Select DISTINCT* from V_STSJGetDOHd WHERE Customer_Code = " + QuotedStr(tbCustCode.Text)
            End If
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "DO_No, DO_Date, Customer_Code, Customer_Name, SONo, CustPONo, DeliveryHour"
            ViewState("Sender") = "btnDONo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn DONo Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlwrhs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlwrhs.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("WrhsUser", ddlwrhs.SelectedValue + "|" + ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubLed.Text = Dr("FgSubLed")
                tbSubledCode.Text = ""
                tbSubledName.Text = ""
            Else
                tbFgSubLed.Text = "N"
                tbSubledCode.Text = ""
                tbSubledName.Text = ""
            End If
            ViewState("SetLocation") = True
            tbSubledCode.Enabled = tbFgSubLed.Text <> "N"
            btnSubled.Visible = tbSubledCode.Enabled
            ddlwrhs.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    
    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubLed.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLed"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSubled Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSubledCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubledCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("SubLed", tbFgSubLed.Text + "|" + tbSubledCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSubledCode.Text = Dr("SubLed_No")
                tbSubledName.Text = Dr("SubLed_Name")
            Else
                tbSubledCode.Text = ""
                tbSubledName.Text = ""
            End If
            'AttachScript("setformat();", Page, Me.GetType())
            tbSubledCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbSubledCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnVehicle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVehicle.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsVehicle"
            ResultField = "Vehicle_Code, Vehicle_Name, Belongs_To "
            ViewState("Sender") = "btnVehicle"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnVehicle Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbVehicleCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbVehicleCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Vehicle", tbVehicleCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbVehicleCode.Text = Dr("Vehicle_Code")
                tbVehicleName.Text = Dr("Vehicle_Name")
                If Dr("Belongs_To") = "Supplier" Then
                    ddlDeliveryBy.SelectedValue = "Expedition"
                Else
                    ddlDeliveryBy.SelectedValue = "Company"
                End If
                If ddlDeliveryBy.SelectedItem.Text = "Expedition" Then
                    ddlDeliveryCostBy.Enabled = True
                Else
                    ddlDeliveryCostBy.Enabled = False
                    ddlDeliveryCostBy.SelectedValue = ""
                End If
            Else
                tbVehicleCode.Text = ""
                tbVehicleName.Text = ""
            End If
            tbVehicleCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbVehicleCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlDeliveryBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeliveryBy.TextChanged
        Try
            If ddlDeliveryBy.SelectedItem.Text = "Expedition" Then
                ddlDeliveryCostBy.Enabled = True
            Else
                ddlDeliveryCostBy.Enabled = False
                ddlDeliveryCostBy.SelectedValue = ""
            End If

        Catch ex As Exception
            Throw New Exception("ddlDeliveryBy Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbQtyPerPack_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyPerPack.TextChanged, tbQtyWrhs.TextChanged
        Dim dt As New DataTable
        dt = SQLExecuteQuery("EXEC S_STSJGetQtyPack " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + QuotedStr(ddlUnitPacking.SelectedValue) + "," + (CFloat(tbQtyPerPack.Text)).ToString + "," + (CFloat(tbQtyWrhs.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
        tbQtyPacking.Text = FormatNumber(dt.Rows(0)("QtyPack").ToString, ViewState("DigitQty"))
    End Sub

    Protected Sub ddlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocation.SelectedIndexChanged
        If ViewState("InputLocation") = "Y" Then
            RefreshMaster("S_GetWrhsLocation", "Location_Code", "Location_Name", ddlLocation, ViewState("DBConnection"))
            ViewState("InputLocation") = Nothing
        End If
    End Sub

    Protected Sub btnCancelComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelComplete.Click
        Try
            MovePanel(pnlComplete, PnlHd)
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "btn CancelComplete Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRRVehicle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRRVehicle.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsVehicle"
            ResultField = "Vehicle_Code, Vehicle_Name, Belongs_To"
            ViewState("Sender") = "btnRRVehicle"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnRRVehicle Error : " + ex.ToString
        End Try
    End Sub
    Private Sub BindDataComplete(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("CompleteDt") = Nothing
            dt = SQLExecuteQuery("SELECT * FROM V_STSJCompleteDt WHERE TransNmbr= " + QuotedStr(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("CompleteDt") = dt
            BindGridComplete(ViewState("CompleteDt"), GridCompleteDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindGridComplete(ByVal source As DataTable, ByVal gv As GridView)
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
            gv.Columns(0).Visible = Not IsEmpty
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridCompleteDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridCompleteDt.RowCancelingEdit
        Try
            'GridCompleteDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridCompleteDt.EditIndex = -1
            BindDataComplete(ViewState("TransNmbr"))
        Catch ex As Exception
            Throw New Exception("GridCompleteDt_RowCancelingEdit Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub GridCompleteDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridCompleteDt.RowDeleting
        Dim tbQtyRR, tbQtyLoss As TextBox
        Dim lbProduct, lbProductName, lbLocation, lbLocationName, lbQtyWrhs, lbUnitWrhs As Label
        Dim GVR As GridViewRow
        Try
            GVR = GridCompleteDt.Rows(e.RowIndex)

            lbProduct = GVR.FindControl("ProductCodeEdit")
            lbProductName = GVR.FindControl("ProductNameEdit")
            lbLocation = GVR.FindControl("LocationCodeEdit")
            lbLocationName = GVR.FindControl("LocationNameEdit")
            lbQtyWrhs = GVR.FindControl("QtyOrderEdit")
            lbUnitWrhs = GVR.FindControl("UnitOrderEdit")
            tbQtyRR = GVR.FindControl("QtyRREdit")
            tbQtyLoss = GVR.FindControl("QtyLossEdit")

            'If CheckMenuLevel("Delete") = False Then
            '    Exit Sub
            'End If

            SQLExecuteNonQuery("Delete from STCSJDt SET QtyRR =" + (CFloat(tbQtyRR.Text)).ToString + ", QtyLoss =" + (CFloat(tbQtyLoss.Text)).ToString + _
            " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")) + " AND Product = " + QuotedStr(lbProduct.Text) + " AND Location = " + QuotedStr(lbLocation.Text), ViewState("DBConnection").ToString)
            BindDataComplete(ViewState("TransNmbr"))

        Catch ex As Exception
            Throw New Exception("GridCompleteDt_RowDeleting Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridCompleteDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridCompleteDt.RowEditing
        Try
            'If CheckMenuLevel("Edit") = False Then
            '    Exit Sub
            'End If
            GridCompleteDt.EditIndex = e.NewEditIndex
            GridCompleteDt.ShowFooter = False
            BindDataComplete(ViewState("TransNmbr"))
        Catch ex As Exception
            Throw New Exception("GridCompleteDt_RowEditing Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridCompleteDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridCompleteDt.RowUpdating
        'Dim Row As DataRow
        'Try
        '    Row = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(ViewState("Product")))(0)

        '    If CekDt() = False Then
        '        btnSaveDt.Focus()
        '        Exit Sub
        '    End If

        '    Row.BeginEdit()
        '    Row("Product") = tbProduct.Text
        '    Row("Product_Name") = tbProductName.Text
        '    Row("Specification") = tbSpec.Text
        '    Row("Location") = ddlLocation.SelectedValue
        '    Row("Location_Name") = ddlLocation.SelectedItem.Text
        '    Row("Qty") = tbQtyWrhs.Text
        '    Row("Unit") = ddlUnitWrhs.SelectedValue
        '    Row("QtyM2") = tbQtyM2.Text
        '    Row("QtyRoll") = tbQtyRoll.Text
        '    Row("QtyPacking") = tbQtyPacking.Text
        '    Row("UnitPacking") = ddlUnitPacking.SelectedValue
        '    Row("QtyPerPack") = tbQtyPerPack.Text
        '    Row("NetWeight") = tbNetWeight.Text
        '    Row("GrossWeight") = tbGrossWeight.Text
        '    Row("Remark") = tbRemarkDt.Text
        '    Row.EndEdit()
        '    ViewState("Product") = Nothing
        'Catch ex As Exception

        'End Try

        Dim tbQtyRR, tbQtyLoss As TextBox
        Dim lbProduct, lbProductName, lbLocation, lbLocationName, lbQtyWrhs, lbUnitWrhs As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = GridCompleteDt.Rows(e.RowIndex)

            lbProduct = GVR.FindControl("ProductCodeEdit")
            lbProductName = GVR.FindControl("ProductNameEdit")
            lbLocation = GVR.FindControl("LocationCodeEdit")
            lbLocationName = GVR.FindControl("LocationNameEdit")
            lbQtyWrhs = GVR.FindControl("QtyOrderEdit")
            lbUnitWrhs = GVR.FindControl("UnitOrdersEdit")
            tbQtyRR = GVR.FindControl("QtyRREdit")
            tbQtyLoss = GVR.FindControl("QtyLossEdit")

            'If (CFloat(tbQtyRR.Text) + CFloat(tbQtyLoss.Text)) > (CFloat(tbQtyWrhs.Text)) Then
            '    lbStatus.Text = MessageDlg("Qty RR plus Qty Loss cannot greater than Qty Wrhs. ")
            '    tbQtyRR.Focus()
            'End If

            SQLString = "UPDATE STCSJDt SET QtyRR =" + (CFloat(tbQtyRR.Text)).ToString + ", QtyLoss =" + (CFloat(tbQtyLoss.Text)).ToString + _
            " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")) + " AND Product = " + QuotedStr(lbProduct.Text) + " AND Location = " + QuotedStr(lbLocation.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'GridCompleteDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridCompleteDt.EditIndex = -1
            BindDataComplete(ViewState("TransNmbr"))
        Catch ex As Exception
            Throw New Exception("GridCompleteDt_RowUpdating Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub QtyRREdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim tbQtyRR, tbQtyLoss As TextBox
        Dim lbQtyWrhs As Label
        Dim GVR As GridViewRow
        Try

            GVR = GridCompleteDt.Rows(GridCompleteDt.EditIndex)
            tbQtyRR = GVR.FindControl("QtyRREdit")
            tbQtyLoss = GVR.FindControl("QtyLossEdit")
            lbQtyWrhs = GVR.FindControl("QtyOrderEdit")

            tbQtyLoss.Text = (CFloat(lbQtyWrhs.Text) - CFloat(tbQtyRR.Text)).ToString

        Catch ex As Exception
            Throw New Exception("QtyRREdit_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub QtyLossEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim tbQtyRR, tbQtyLoss As TextBox
        Dim lbQtyWrhs As Label
        Dim GVR As GridViewRow
        Try

            GVR = GridCompleteDt.Rows(GridCompleteDt.EditIndex)
            tbQtyRR = GVR.FindControl("QtyRREdit")
            tbQtyLoss = GVR.FindControl("QtyLossEdit")
            lbQtyWrhs = GVR.FindControl("QtyOrderEdit")

            tbQtyRR.Text = (CFloat(lbQtyWrhs.Text) - CFloat(tbQtyLoss.Text)).ToString

        Catch ex As Exception
            Throw New Exception("QtyLossEdit_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxCompleteHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction("SELECT * FROM V_STSJCompleteHd ", "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            BindToDate(tbRRDate, Dt.Rows(0)("RRDate").ToString)
            BindToText(tbRRVehicleCode, Dt.Rows(0)("Vehicle_Code").ToString)
            BindToText(tbRRVehicleName, Dt.Rows(0)("Vehicle_Name").ToString)
            BindToText(tbRRDriver, Dt.Rows(0)("RRDriver").ToString)
            BindToText(tbRRExpeditionName, Dt.Rows(0)("RRExpeditionName").ToString)
            BindToText(tbRRExpeditionNo, Dt.Rows(0)("RRExpeditionNo").ToString)
            BindToText(tbArrivalTime, Dt.Rows(0)("RRTimeArrival").ToString)
            BindToText(tbUnloadingTime, Dt.Rows(0)("RRTimeUnLoading").ToString)
            BindToText(tbFinishTime, Dt.Rows(0)("RRTimeFinish").ToString)
            BindToText(tbRRRemark, Dt.Rows(0)("RRRemark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box complete header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
        Try
            Dim SQLString, Result As String

            If tbRRVehicleName.Text.Trim = "" Then
                lbStatus.Text = "Vehicle No must have value."
                tbRRVehicleCode.Focus()
                Exit Sub
            End If

            SQLString = "Declare @A VarChar(255) EXEC S_STSJComplete " + QuotedStr(ViewState("TransNmbr")) + "," + _
                QuotedStr(tbRRDate.SelectedDate) + "," + _
                QuotedStr(tbRRVehicleCode.Text) + "," + _
                QuotedStr(tbRRDriver.Text) + "," + _
                QuotedStr(tbRRExpeditionName.Text) + "," + _
                QuotedStr(tbRRExpeditionNo.Text) + "," + _
                QuotedStr(tbArrivalTime.Text) + "," + _
                QuotedStr(tbUnloadingTime.Text) + "," + _
                QuotedStr(tbFinishTime.Text) + "," + _
                QuotedStr(tbRRRemark.Text) + "," + _
                (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
            SQLString = Replace(SQLString, "''", "NULL")
            Result = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            'SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            If (Trim(Result) <> "") And (Trim(Result) <> "0") Then
                lbStatus.Text = lbStatus.Text + Result + " <br/>"
            End If
            MovePanel(pnlComplete, PnlHd)
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            Throw New Exception("btnComplete_Click error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelCancel.Click
        Try
            MovePanel(pnlCancel, PnlHd)
            BindData("")
        Catch ex As Exception
            lbStatus.Text = "btn CancelCancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelTrans.Click
        Try
            Dim SQLString, Result As String

            SQLString = "Declare @A VarChar(255) EXEC S_STSJCancel " + QuotedStr(ViewState("TransNmbr")) + "," + _
                QuotedStr(tbCancelDate.SelectedDate) + "," + QuotedStr(tbReasonCancel.Text) + "," + _
                (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
            SQLString = Replace(SQLString, "''", "NULL")
            Result = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            If Trim(Result) <> "" Then
                lbStatus.Text = lbStatus.Text + Result + " <br/>"
            End If
            MovePanel(pnlCancel, PnlHd)
            BindData("")
        Catch ex As Exception
            Throw New Exception("btnCancelTrans_Click error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub lbWarehouse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWarehouse.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsWarehouse')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Warehouse Error : " + ex.ToString
        End Try
    End Sub
    Private Sub GetInfo(ByVal Product As String)
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_GetInfoStock " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubledCode.Text) + ", " + QuotedStr(Product)

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PnlInfo.Visible = True
            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, ResultSame As String
        Try
            Session("filter") = "EXEC S_STSJGetDOOut"
            ResultField = "DO_No, DO_Date, Revisi, Status, FgActive, DeliveryDate, Customer_Code, Customer_Name, SO_No, Cust_PO_No, Delivery_Code, Delivery_Name, Delivery_Addr, Delivery_City, Delivery_Date, Delivery_Hour, Remark_Hd, SO_Delivery, Product, ProductName, Specification, NetWeight, GrossWeight, UnitPack, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, Remark"
            CriteriaField = "DO_No, DO_Date, Revisi, Status, FgActive, DeliveryDate, Customer_Code, Customer_Name, SO_No, Cust_PO_No, Delivery_Code, Delivery_Name, Delivery_Addr, Delivery_City, Delivery_Date, Delivery_Hour, Remark_Hd, SO_Delivery, Product, ProductName, Specification, UnitPack, UnitOrder, Remark"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ResultSame = "DO_No"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbCount_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub tbQtyRoll_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyRoll.TextChanged, btnQtyRoll.Click
    '    Try
    '        If lbProdGrp.Text = "HY" Then
    '            Dim dt As New DataTable
    '            dt = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + "," + QuotedStr(ddlUnitOrder.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
    '            tbQtyOrder.Text = FormatNumber(dt.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))

    '            Dim dt2 As New DataTable
    '            dt2 = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + "," + QuotedStr(ddlUnitWrhs.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
    '            tbQtyWrhs.Text = FormatNumber(dt2.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))

    '            Dim dt3 As New DataTable
    '            dt3 = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + ",'M2'", ViewState("DBConnection").ToString).Tables(0)
    '            tbQtyM2.Text = FormatNumber(dt3.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception("tbQtyRoll_TextChanged Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridView1.PageSize = ddlRow.SelectedValue
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "ddlRow_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Sub execBtnGo(ByVal sender As Object)
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
            '3 = status, 2 & 3 = key, 
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)

            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else

                    Result = ExecSPCommandGo(ActionValue, "S_STSJ", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "BtnGo_Click Error : " + ex.ToString
        End Try
    End Sub
   
    Protected Sub btnQtyWrhs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQtyWrhs.Click
        Try

            Dim dt As New DataTable
            dt = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + (CFloat(tbQtyWrhs.Text)).ToString + "," + QuotedStr(ddlUnitOrder.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            tbQtyOrder.Text = FormatNumber(dt.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))

            Dim dt2 As New DataTable
            dt2 = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + (CFloat(tbQtyWrhs.Text)).ToString + ",'ROLL'", ViewState("DBConnection").ToString).Tables(0)
            tbQtyRoll.Text = FormatNumber(dt2.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))

            Dim dt3 As New DataTable
            dt3 = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + (CFloat(tbQtyWrhs.Text)).ToString + ",'M2'", ViewState("DBConnection").ToString).Tables(0)
            tbQtyM2.Text = FormatNumber(dt3.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))

        Catch ex As Exception
            Throw New Exception("tbQtyRoll_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnQtyRoll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQtyRoll.Click
        Try
            
            If lbProdGrp.Text = "HY" Then
                Dim dt As New DataTable
                dt = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + "," + QuotedStr(ddlUnitOrder.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
                tbQtyOrder.Text = FormatNumber(dt.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
                'ViewState("i") = ViewState("i") + 1
                Dim dt2 As New DataTable
                dt2 = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + "," + QuotedStr(ddlUnitWrhs.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
                tbQtyWrhs.Text = FormatNumber(dt2.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))

                Dim dt3 As New DataTable
                dt3 = SQLExecuteQuery("EXEC S_STSJGetQtyConvert " + QuotedStr(tbProduct.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + ",'M2'", ViewState("DBConnection").ToString).Tables(0)
                tbQtyM2.Text = FormatNumber(dt3.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
            End If
            'lbStatus.Text = ViewState("i").ToString

        Catch ex As Exception
            Throw New Exception("tbQtyRoll_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnQtyOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQtyOrder.Click
        Try
            Dim dt As New DataTable
            dt = SQLExecuteQuery("EXEC S_STSJGetQtyDO " + QuotedStr(tbDONo.Text) + "," + QuotedStr(tbProduct.Text) + "," + (CFloat(tbQtyOrder.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)

            tbQtyWrhs.Text = FormatNumber(dt.Rows(0)("QtyWrhsSJ").ToString, ViewState("DigitQty"))
            tbQtyM2.Text = FormatNumber(dt.Rows(0)("QtyM2SJ").ToString, ViewState("DigitQty"))
            tbQtyRoll.Text = FormatNumber(dt.Rows(0)("QtyRollSJ").ToString, ViewState("DigitQty"))

            Dim dt2 As New DataTable
            dt2 = SQLExecuteQuery("EXEC S_STSJGetQtyPack " + QuotedStr(tbProduct.Text) + "," + QuotedStr(ddlUnitWrhs.SelectedValue) + "," + QuotedStr(ddlUnitPacking.SelectedValue) + "," + (CFloat(tbQtyPerPack.Text)).ToString + "," + (CFloat(tbQtyWrhs.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
            If CFloat(tbQtyPerPack.Text) = 0 Then
                tbQtyPerPack.Text = FormatNumber(dt2.Rows(0)("QtyPerPack").ToString, ViewState("DigitQty"))
            End If
            tbQtyPacking.Text = FormatNumber(dt2.Rows(0)("QtyPack").ToString, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("tbQtyOrder_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class