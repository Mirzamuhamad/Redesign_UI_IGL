Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrDO_TrDO
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_MKDOHd"


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
                lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT SO_No) from V_MKDOGetSO", ViewState("DBConnection").ToString)
            End If
            GridDt.Columns(10).Visible = False
            GridDt.Columns(11).Visible = False
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
                If ViewState("Sender") = "btnDelivery" Then
                    tbDeliveryCode.Text = Session("Result")(1).ToString
                    tbDeliveryName.Text = Session("Result")(2).ToString
                    tbDeliveryAddress.Text = Session("Result")(3).ToString
                    If Session("Result")(5).ToString <> "" Then
                        tbDeliveryAddress.Text = tbDeliveryAddress.Text + " Phone : " + Session("Result")(5).ToString
                    End If
                    If Session("Result")(6).ToString <> "" Then
                        tbDeliveryAddress.Text = tbDeliveryAddress.Text + " Fax : " + Session("Result")(6).ToString
                    End If

                    ddlDeliveryCity.SelectedValue = Session("Result")(4).ToString
                End If
                If ViewState("Sender") = "btnProduct" Then
                    BindToText(tbProduct, Session("Result")(0).ToString)
                    BindToText(tbProductName, Session("Result")(1).ToString)
                    BindToText(tbSpec, Session("Result")(2).ToString)
                    BindToText(tbQtyOrder, Session("Result")(3).ToString)
                    ddlUnitOrder.SelectedValue = Session("Result")(4).ToString
                    BindToText(tbQtyWrhs, Session("Result")(5).ToString)
                    ddlUnitWrhs.SelectedValue = Session("Result")(6).ToString
                    BindToText(tbQtyM2, Session("Result")(7).ToString)
                    BindToText(tbQtyRoll, Session("Result")(8).ToString)
                    BindToText(tbRemarkDt, Session("Result")(9).ToString)
                End If
                If ViewState("Sender") = "btnSONo" Then
                    If tbCustCode.Text.Trim = "" Then
                        BindToText(tbCustCode, Session("Result")(8).ToString)
                        BindToText(tbCustName, Session("Result")(9).ToString)
                        BindToText(tbCustName, Session("Result")(9).ToString)
                    Else
                        If tbCustCode.Text <> Session("Result")(8).ToString Then
                            Exit Sub
                        End If
                    End If
                    BindToText(tbSONo, Session("Result")(0).ToString)
                    BindToText(tbCustPONo, Session("Result")(1).ToString)
                    ddlNeedDelivery.SelectedValue = Session("Result")(2).ToString
                    BindToText(tbDeliveryCode, Session("Result")(3).ToString)
                    BindToText(tbDeliveryName, Session("Result")(4).ToString)
                    BindToText(tbDeliveryAddress, Session("Result")(5).ToString)
                    ddlDeliveryCity.SelectedValue = Session("Result")(6).ToString
                    BindToText(tbRemarkDt, Session("Result")(7).ToString)
                    BindToText(tbSONoRev, Session("Result")(10).ToString)
                    BindToText(tbNoKontrakSO, Session("Result")(11).ToString)
                    tbDeliveryCode.Enabled = ddlNeedDelivery.SelectedValue = "Y"
                    tbDeliveryAddress.Enabled = ddlNeedDelivery.SelectedValue = "Y"
                    ddlDeliveryCity.Enabled = ddlNeedDelivery.SelectedValue = "Y"
                    btnDelivery.Visible = ddlNeedDelivery.SelectedValue = "Y"
                    FillCombo(ddlDeliveryDate, "EXEC S_MKDOGetDeliveryDate " + QuotedStr(tbSONo.Text) + ", " + QuotedStr(tbCode.Text) + ", " + lbRevisi.Text, True, "Delivery", "DeliveryName", ViewState("DBConnection"))
                End If
                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = TrimStr(drResult("ProductName").ToString)
                            dr("Specification") = TrimStr(drResult("Specification").ToString)
                            dr("Remark") = TrimStr(drResult("Remark").ToString)
                            dr("QtyOrder") = drResult("QtyOrder")
                            dr("UnitOrder") = drResult("UnitOrder")
                            dr("QtyWrhs") = drResult("QtyWrhs")
                            dr("UnitWrhs") = drResult("UnitWrhs")
                            dr("QtyM2") = drResult("QtyM2")
                            dr("QtyRoll") = drResult("QtyRoll")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(ViewState("Dt").Rows.count = 0)
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing
                End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    Dim Qty As Double
                    For Each drResult In Session("Result").Rows

                        'insert
                        If FirstTime Then
                            BindToText(tbCustCode, drResult("Customer"))
                            BindToText(tbCustName, drResult("Customer_Name"))
                            BindToText(tbSONo, drResult("SO_No"))
                            FillCombo(ddlDeliveryDate, "EXEC S_MKDOGetDeliveryDate " + QuotedStr(tbSONo.Text) + ", " + QuotedStr(tbCode.Text) + ", " + lbRevisi.Text, False, "Delivery", "DeliveryName", ViewState("DBConnection"))
                            BindToDropList(ddlDeliveryDate, drResult("SO_Date"))
                            BindToText(tbCustPONo, drResult("CustPONo"))
                            BindToDropList(ddlNeedDelivery, drResult("FgNeedDelivery"))
                            BindToText(tbDeliveryCode, drResult("DeliveryCode"))
                            BindToText(tbDeliveryName, drResult("DeliveryName"))
                            BindToText(tbDeliveryAddress, drResult("DeliveryAddr"))
                            BindToDropList(ddlDeliveryCity, drResult("DeliveryCity"))
                            BindToText(tbSONoRev, drResult("Revisi"))
                            BindToText(tbNoKontrakSO, drResult("So_SPK"))
                            BindToText(tbRemark, drResult("Remark"))
                        End If
                        Qty = 0
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("ProductName")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("QtyOrder") = drResult("QtyOrder")
                            dr("UnitOrder") = drResult("UnitOrder")
                            dr("QtyWrhs") = drResult("Qty")
                            dr("UnitWrhs") = drResult("Unit")
                            dr("QtyM2") = drResult("QtyM2")
                            dr("QtyRoll") = drResult("QtyRoll")
                            dr("PriceForex") = drResult("PriceForex")
                            dr("Remark") = drResult("RemarkDt")
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
            If Not ViewState("ProductClose") Is Nothing Then
                If HiddenRemarkClose.Value <> "False Value" Then                    
                    Dim sqlstring, result As String
                    sqlstring = "Declare @A VarChar(255) EXEC S_MKDOClosing " + QuotedStr(tbCode.Text) + "," + lbRevisi.Text + "," + QuotedStr(ViewState("ProductClose").ToString) + "," + QuotedStr(HiddenRemarkClose.Value) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                    If result.Length > 2 Then
                        lbStatus.Text = MessageDlg(result)
                    Else
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    End If
                End If
                ViewState("ProductClose") = Nothing
                HiddenRemarkClose.Value = ""
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
            'FillCombo(ddlDeliveryDate, "EXEC S_MKDOGetDeliveryDate " + QuotedStr(tbSONo.Text), True, "Delivery", "Delivery", ViewState("DBConnection"))
            FillCombo(ddlUnitWrhs, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitOrder, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlDeliveryCity, "EXEC S_GetCity", True, "City_Code", "City_Name", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            tbQtyM2.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyRoll.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyWrhs.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyOrder.Attributes.Add("OnBlur", "setformatdt();")
            tbPrice.Attributes.Add("OnBlur", "setformatdt();")

            tbQtyOrder.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyWrhs.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")

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

    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MKDODt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbCustCode.Enabled = State
            btnCust.Visible = State
            btnSONo.Visible = State
            tbDeliveryCode.Enabled = State
            btnDelivery.Visible = State
            tbDeliveryAddress.Enabled = State
            ddlDeliveryCity.Enabled = State
            ddlDeliveryDate.Enabled = State
            'ddlDeliveryDate.Enabled = State
            'tbDeliveryHour.Enabled = State
            btnGetData.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
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
                tbCode.Text = GetAutoNmbr("DO", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                CekTrans = SQLExecuteScalar("SELECT COUNT(TransNmbr) FROM MKTDOHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " AND Revisi = " + lbRevisi.Text, ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Delivery Order No. " + tbCode.Text + " and Revisi " + lbRevisi.Text + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MKTDOHd (TransNmbr, Status, TransDate, Revisi, " + _
                "Customer, SONo, SORev, CustPONo, FgNeedDelivery, " + _
                "DeliveryCode, DeliveryAddr, DeliveryCity, " + _
                "DeliveryHour, DeliveryDate,DeliveryDate2, SODelivery, " + _
                "Remark,ContractNo, UserPrep, DatePrep, FgActive) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', 0, " + _
                QuotedStr(tbCustCode.Text) + "," + QuotedStr(tbSONo.Text) + "," + tbSONoRev.Text + "," + QuotedStr(tbCustPONo.Text) + "," + _
                QuotedStr(ddlNeedDelivery.SelectedValue) + "," + _
                QuotedStr(tbDeliveryCode.Text) + "," + QuotedStr(tbDeliveryAddress.Text) + "," + _
                QuotedStr(ddlDeliveryCity.SelectedValue) + "," + _
                QuotedStr(tbDeliveryHour.Text) + "," + _
                QuotedStr(Format(tbDeliveryDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(ddlDeliveryDate.SelectedValue) + "," + _
                QuotedStr(Format(tbDeliveryDate2.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(tbNoKontrak.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), 'Y'"
                ViewState("TransNmbr") = tbCode.Text
                ViewState("Revisi") = "0"
            Else
                SQLString = "UPDATE MKTDOHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Customer = " + QuotedStr(tbCustCode.Text) + _
                ", SONo = " + QuotedStr(tbSONo.Text) + ", SORev = " + QuotedStr(tbSONoRev.Text) + _
                ", CustPONo = " + QuotedStr(tbCustPONo.Text) + _
                ", FgNeedDelivery = " + QuotedStr(ddlNeedDelivery.SelectedValue) + _
                ", DeliveryCode = " + QuotedStr(tbDeliveryCode.Text) + _
                ", DeliveryAddr = " + QuotedStr(tbDeliveryAddress.Text) + _
                ", DeliveryCity = " + QuotedStr(ddlDeliveryCity.SelectedValue) + _
                ", DeliveryHour = " + QuotedStr(tbDeliveryHour.Text) + _
                ", ContractNo = " + QuotedStr(tbNoKontrak.Text) + _
                ", SODelivery = " + QuotedStr(ddlDeliveryDate.SelectedValue) + _
                ", DeliveryDate = " + QuotedStr(Format(tbDeliveryDate.SelectedValue, "yyyy-MM-dd")) + _
                ", DeliveryDate2 = " + QuotedStr(Format(tbDeliveryDate2.SelectedValue, "yyyy-MM-dd")) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I)("Revisi") = lbRevisi.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Revisi, Product, Specification, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll,PriceForex, Remark " + _
                                         " FROM MKTDODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE MKTDODt SET Product = @Product, Specification = @Specification, " + _
            "QtyOrder = @QtyOrder, UnitOrder = @UnitOrder, QtyWrhs = @QtyWrhs, " + _
            "UnitWrhs = @UnitWrhs, QtyM2 = @QtyM2, QtyRoll = @QtyRoll,PriceForex = @PriceForex, Remark = @Remark " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = @OldRevisi AND Product = @OldProduct", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Specification", SqlDbType.VarChar, 255, "Specification")
            Update_Command.Parameters.Add("@QtyOrder", SqlDbType.Float, 18, "QtyOrder")
            Update_Command.Parameters.Add("@UnitOrder", SqlDbType.VarChar, 5, "UnitOrder")
            Update_Command.Parameters.Add("@QtyWrhs", SqlDbType.Float, 18, "QtyWrhs")
            Update_Command.Parameters.Add("@UnitWrhs", SqlDbType.VarChar, 5, "UnitWrhs")
            Update_Command.Parameters.Add("@QtyM2", SqlDbType.Float, 18, "QtyM2")
            Update_Command.Parameters.Add("@QtyRoll", SqlDbType.Float, 18, "QtyRoll")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 18, "PriceForex")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            
            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldRevisi", SqlDbType.Int, 4, "Revisi")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM MKTDODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = @Revisi AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Revisi", SqlDbType.Int, 4, "Revisi")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("MKTDODt")

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
            ViewState("TransNmbr") = Now.Year.ToString
            ViewState("Revisi") = "0"
            ClearHd()

            Cleardt()

            pnlDt.Visible = True
            BindDataDt("", "0")

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate")
            lbRevisi.Text = "0"
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbNoKontrak.Text = ""
            tbSONo.Text = ""
            tbSONoRev.Text = ""
            tbCustPONo.Text = ""
            ddlNeedDelivery.SelectedValue = "N"
            tbDeliveryCode.Text = ""
            tbDeliveryName.Text = ""
            tbDeliveryAddress.Text = ""
            ddlDeliveryCity.SelectedValue = ""
            tbDeliveryHour.Text = "00:00"
            FillCombo(ddlDeliveryDate, "EXEC S_MKDOGetDeliveryDate '', '', 0 ", True, "Delivery", "DeliveryName", ViewState("DBConnection"))
            ddlDeliveryDate.SelectedValue = ""
            tbDeliveryDate.SelectedDate = ViewState("ServerDate")
            tbDeliveryDate2.SelectedDate = ViewState("ServerDate")
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
            ddlUnitWrhs.SelectedValue = ""
            ddlUnitOrder.SelectedValue = ""
            tbPrice.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyOrder.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyM2.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyRoll.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyWrhs.Text = FormatFloat("0", ViewState("DigitQty"))
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
            If tbSONo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("SO No must have value. ")
                btnSONo.Focus()
                Return False
            End If
            If ddlNeedDelivery.SelectedValue = "Y" And tbDeliveryCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer Delivery must have value. ")
                tbDeliveryCode.Focus()
                Return False
            End If
            If ddlDeliveryDate.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("SO Delivery Date must have value. ")
                ddlDeliveryDate.Focus()
                Return False
            End If

            If tbNoKontrak.Text = "" Then
                lbStatus.Text = MessageDlg("No Contract must have value. ")
                ddlDeliveryDate.Focus()
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
                If Dr("PriceForex") = 0 Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If

            Else

                If tbPrice.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If

                If tbProduct.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProduct.Focus()
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
                    ViewState("Revisi") = GVR.Cells(4).Text
                    ViewState("Status") = GVR.Cells(3).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    btnGetData.Visible = False
                    
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        pnlDt.Visible = True
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(4).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        tbDeliveryCode.Enabled = ddlNeedDelivery.SelectedValue = "Y"
                        tbDeliveryAddress.Enabled = ddlNeedDelivery.SelectedValue = "Y"
                        ddlDeliveryCity.Enabled = ddlNeedDelivery.SelectedValue = "Y"
                        btnDelivery.Visible = ddlNeedDelivery.SelectedValue = "Y"
                        'FillCombo(ddlDeliveryDate, "EXEC S_MKDOGetDeliveryDate " + QuotedStr(tbSONo.Text), True, "Delivery", "DeliveryName", ViewState("DBConnection"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        btnGetData.Visible = True
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Revisi" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    If Not GVR.Cells(3).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must be Posted Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_MKDOCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + GVR.Cells(4).Text + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = GVR.Cells(2).Text
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    If Not GVR.Cells(3).Text = "H" Then
                        lbStatus.Text = MessageDlg("Data Must be Hold Before Deleted")
                        Exit Sub
                    End If

                    Dim SqlString As String

                    SqlString = "Declare @A VarChar(255) EXEC S_MKDODelete " + QuotedStr(GVR.Cells(2).Text) + ", " + GVR.Cells(4).Text + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "
                    
                    SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                    BindData(Session("AdvanceFilter"))
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_MKFormDO ''" + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(4).Text) + "'', " + QuotedStr(ViewState("UserId").ToString)
                        'Session("SelectCommand") = "EXEC S_MKFormDO " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(4).Text
                        Session("ReportFile") = ".../../../Rpt/FormDO.frx"
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
            If e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status DO is not Post, cannot close product")
                    Exit Sub
                End If
                If GVR.Cells(13).Text = "Y" Then
                    lbStatus.Text = MessageDlg("Product Closed Already")
                    Exit Sub
                End If
                
                ViewState("ProductClose") = GVR.Cells(2).Text
                AttachScript("closing();", Page, Me.GetType)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("StateDt") = "Edit"
            ViewState("Product") = GVR.Cells(2).Text
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            tbProduct.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Taon As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Taon) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            tbCode.Text = Taon
            lbRevisi.Text = Revisi
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbDeliveryDate, Dt.Rows(0)("DeliveryDate").ToString)
            BindToDate(tbDeliveryDate2, Dt.Rows(0)("DeliveryDate2").ToString)
            BindToText(tbCode, Dt.Rows(0)("TransNmbr").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbSONo, Dt.Rows(0)("SONo").ToString)
            BindToText(tbSONoRev, Dt.Rows(0)("SORev").ToString)
            BindToText(tbCustPONo, Dt.Rows(0)("CustPONo").ToString)
            BindToText(tbNoKontrakSO, Dt.Rows(0)("ContractSO").ToString)
            BindToText(tbDeliveryCode, Dt.Rows(0)("DeliveryCode").ToString)
            BindToText(tbDeliveryName, Dt.Rows(0)("DeliveryName").ToString)
            BindToText(tbDeliveryAddress, Dt.Rows(0)("DeliveryAddr").ToString)
            BindToText(tbDeliveryHour, Dt.Rows(0)("DeliveryHour").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbNoKontrak, Dt.Rows(0)("ContractNo").ToString)
            BindToDropList(ddlDeliveryCity, Dt.Rows(0)("DeliveryCity").ToString)
            FillCombo(ddlDeliveryDate, "EXEC S_MKDOGetDeliveryDate " + QuotedStr(tbSONo.Text) + ", " + QuotedStr(tbCode.Text) + ", " + lbRevisi.Text, True, "Delivery", "DeliveryName", ViewState("DBConnection"))
            BindToDropList(ddlDeliveryDate, Dt.Rows(0)("SODelivery").ToString)
            BindToDropList(ddlNeedDelivery, Dt.Rows(0)("FgNeedDelivery").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(Product))

            If Dr.Length > 0 Then
                BindToText(tbProduct, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToText(tbSpec, Dr(0)("Specification").ToString)
                BindToText(tbQtyOrder, Dr(0)("QtyOrder").ToString)
                BindToText(tbQtyM2, Dr(0)("QtyM2").ToString)
                BindToText(tbQtyRoll, Dr(0)("QtyRoll").ToString)
                BindToText(tbPrice, Dr(0)("PriceForex").ToString)
                BindToText(tbQtyWrhs, Dr(0)("QtyWrhs").ToString)
                BindToDropList(ddlUnitWrhs, Dr(0)("UnitWrhs").ToString)
                BindToDropList(ddlUnitOrder, Dr(0)("UnitOrder").ToString)
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
            SQL = "EXEC S_MKDOGetSODt " + QuotedStr(tbSONo.Text) + ", '" + ddlDeliveryDate.SelectedValue + "', " + QuotedStr(tbProduct.Text.Trim)

            Dt = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProduct.Text = Dr("Product")
                tbProductName.Text = TrimStr(Dr("ProductName").ToString)
                tbSpec.Text = TrimStr(Dr("Specification").ToString)
                ddlUnitWrhs.SelectedValue = Dr("UnitWrhs")
                ddlUnitOrder.SelectedValue = Dr("UnitOrder")
                tbQtyOrder.Text = Dr("QtyOrder")
                tbQtyWrhs.Text = Dr("QtyWrhs")
                tbQtyM2.Text = Dr("QtyM2")
                tbQtyRoll.Text = Dr("QtyRoll")
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbSpec.Text = ""
                ddlUnitWrhs.SelectedValue = ""
                ddlUnitOrder.SelectedValue = ""
                tbQtyOrder.Text = FormatNumber("0", ViewState("DigitQty"))
                tbQtyWrhs.Text = FormatNumber("0", ViewState("DigitQty"))
                tbQtyM2.Text = FormatNumber("0", ViewState("DigitQty"))
                tbQtyRoll.Text = FormatNumber("0", ViewState("DigitQty"))
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
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            pnlDt.Visible = True
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            btnGetData.Visible = True
            tbCode.Focus()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status, msg As String
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
                        'If GVR.Cells(4).Text = "P" Then
                        ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(4).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                        'End If
                    End If
                Next
                Result = Result + "',''"
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_MKFormDO " + Result
                'Session("SelectCommand") = "EXEC S_MKFormDO " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(4).Text
                Session("ReportFile") = ".../../../Rpt/FormDO.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                msg = ""
                '3 = status, 2 & 3 = key, 
                GetListCommand(Status, GridView1, "3,2,4", ListSelectNmbr, Nmbr, msg)

                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else

                        Result = ExecSPCommandGo(ActionValue, "S_MKDO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result '+ " <br/>"
                            Exit Sub
                        End If
                    End If
                Next
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")

                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                '3 = status, 2 & 3 = key, 
                GetListCommand(Status, GridView1, "3,2,4", ListSelectNmbr, Nmbr, lbStatus.Text)

                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else

                        Result = ExecSPCommandGo(ActionValue, "S_MKDO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
                'If msg.Trim <> "" Then
                '    lbStatus.Text = MessageDlg(msg)
                'End If
            End If
        Catch ex As Exception
            lbStatus.Text = "BtnGo_Click Error : " + ex.ToString
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
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

            Session("Filter") = "EXEC S_MKDOGetSODt " + QuotedStr(tbSONo.Text) + ", '" + ddlDeliveryDate.SelectedValue + "', '' "
            ResultField = "Product, ProductName, Specification, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, Remark "
            CriteriaField = "Product, ProductName, Specification, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, Remark "
            'Session("ClickSame") = ""
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            'ResultSame = ""
            'Session("ResultSame") = ResultSame.Split(",")
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

            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("Product = " + QuotedStr(tbProduct.Text))

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                'If ExistRow.Count > AllowedRecord() Then
                '    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                '    Exit Sub
                'End If

                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("Product")))(0)

                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("Product") = tbProduct.Text
                Row("ProductName") = tbProductName.Text
                Row("UnitWrhs") = ddlUnitWrhs.SelectedValue
                Row("UnitOrder") = ddlUnitOrder.SelectedValue
                Row("Specification") = tbSpec.Text
                Row("QtyOrder") = tbQtyOrder.Text
                Row("QtyWrhs") = tbQtyWrhs.Text
                Row("PriceForex") = tbPrice.Text
                Row("QtyM2") = tbQtyM2.Text
                Row("QtyRoll") = tbQtyRoll.Text
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
                dr("ProductName") = tbProductName.Text
                dr("UnitWrhs") = ddlUnitWrhs.SelectedValue
                dr("UnitOrder") = ddlUnitOrder.SelectedValue
                dr("Specification") = tbSpec.Text
                dr("QtyOrder") = tbQtyOrder.Text
                dr("QtyWrhs") = tbQtyWrhs.Text
                dr("PriceForex") = tbPrice.Text
                dr("QtyM2") = tbQtyM2.Text
                dr("QtyRoll") = tbQtyRoll.Text
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
            lbStatus.Text = "btn back Error : " + ex.ToString
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
            Session("Filter") = "EXEC S_MKDOGetSODt " + QuotedStr(tbSONo.Text) + ", '" + ddlDeliveryDate.SelectedValue + "', '' "
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Product, ProductName, Specification, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, Remark "
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
            Session("filter") = "Select Customer_Code, Customer_Name from VMsCustomer WHERE FgActive = 'Y'"
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Customer_Code, Customer_Name"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDelivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelivery.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select * from VMsCustAddress WHERE Cust_Code =" + QuotedStr(tbCustCode.Text)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Cust_Code, Delivery_Code, Delivery_Name, Delivery_Addr1, City, Phone_No, Fax"
            ViewState("Sender") = "btnDelivery"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Delivery Click Error : " + ex.ToString
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
            tbSONo.Text = ""
            tbDeliveryCode.Text = ""
            tbDeliveryName.Text = ""
            tbDeliveryAddress.Text = ""
            ddlDeliveryCity.SelectedValue = ""
            ddlNeedDelivery.SelectedValue = "N"
            tbDeliveryCode.Enabled = ddlNeedDelivery.SelectedValue = "Y"
            tbDeliveryAddress.Enabled = ddlNeedDelivery.SelectedValue = "Y"
            ddlDeliveryCity.Enabled = ddlNeedDelivery.SelectedValue = "Y"
            btnDelivery.Visible = ddlNeedDelivery.SelectedValue = "Y"
            tbCustPONo.Text = ""
            tbCustCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDeliveryCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDeliveryCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("CustAddress", tbCustCode.Text + "|" + tbDeliveryCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbDeliveryCode.Text = Dr("Delivery_Code")
                tbDeliveryName.Text = Dr("Delivery_Name")
                tbDeliveryAddress.Text = Dr("Delivery_Addr1")
                ddlDeliveryCity.SelectedValue = Dr("City")
            Else
                tbDeliveryCode.Text = ""
                tbDeliveryName.Text = ""
                tbDeliveryAddress.Text = ""
                ddlDeliveryCity.SelectedValue = ""
            End If
            tbDeliveryCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb DeliveryCode change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSONo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSONo.Click
        Dim ResultField As String
        Try
            If tbCustCode.Text.Trim = "" Then
                Session("filter") = "Select * from V_MKDOGetSO"
            Else
                Session("filter") = "Select * from V_MKDOGetSO WHERE Customer_Code = " + QuotedStr(tbCustCode.Text)
            End If
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "SO_No, CustPONo, FgNeedDelivery, DeliveryCode, DeliveryName, DeliveryAddr, DeliveryCity, Remark, Customer_Code, Customer_Name, Revisi,ContractNo"
            ViewState("Sender") = "btnSONo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn SONo Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlNeedDelivery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNeedDelivery.TextChanged
        If ddlNeedDelivery.SelectedValue = "N" Then
            tbDeliveryCode.Text = ""
            tbDeliveryName.Text = ""
            tbDeliveryAddress.Text = ""
            ddlDeliveryCity.SelectedIndex = 0
        End If
        tbDeliveryCode.Enabled = ddlNeedDelivery.SelectedValue = "Y"
        tbDeliveryAddress.Enabled = ddlNeedDelivery.SelectedValue = "Y"
        ddlDeliveryCity.Enabled = ddlNeedDelivery.SelectedValue = "Y"
        btnDelivery.Visible = ddlNeedDelivery.SelectedValue = "Y"
    End Sub

    Protected Sub tbQtyOrder_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOrder.TextChanged
        Try
            Dim dt As New DataTable
            dt = SQLExecuteQuery("EXEC S_MKDOGetQtySO " + QuotedStr(tbSONo.Text) + "," + QuotedStr(tbProduct.Text) + "," + (CFloat(tbQtyOrder.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
            BindToText(tbQtyWrhs, dt.Rows(0)("QtyWrhsDO").ToString, ViewState("DigitQty"))
            BindToText(tbQtyM2, dt.Rows(0)("QtyM2DO").ToString, ViewState("DigitQty"))
            BindToText(tbQtyRoll, dt.Rows(0)("QtyRollDO").ToString, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("tbQtyOrder_TextChanged Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Delivery Date, SO Delivery"
            FDateValue = "TransDate, DeliveryDate, SODelivery"
            'FDateName = "Date"
            'FDateValue = "TransDate"
            FilterName = "DO No, Customer, Customer Name, SO No, PO Cust No, Need Delivery, Delivery, Delivery Name, Delivery Addr, Delivery City, Delivery Hour, Remark"
            FilterValue = "TransNmbr, Customer, Customer_Name, SONo, CustPONo, FgNeedDelivery, DeliveryCode, DeliveryName, DeliveryAddr, DeliveryCity, DeliveryHour, Remark"
            'FilterName = "DO No"
            'FilterValue = "TransNmbr"
            ViewState("DateFieldName") = FDateName.Split(",")
            ViewState("DateFieldValue") = FDateValue.Split(",")
            ViewState("FieldName") = FilterName.Split(",")
            ViewState("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, ResultSame As String
        Try
            Session("filter") = "EXEC S_MKDOGetSOOut"
            ResultField = "SO_No, Revisi, Status, Customer, Customer_Name, CustPONo, DeliveryCode,so_SPK, DeliveryName, DeliveryAddr, DeliveryCity, FgNeedDelivery, Remark, Product, ProductName, Specification, UnitOrder, QtyOrder, Unit, Qty, QtyM2, QtyRoll,PriceForex,SO_Date, RemarkDt"
            CriteriaField = "SO_No, Revisi, Status, Customer, Customer_Name, CustPONo, DeliveryCode,so_SPK, DeliveryName, DeliveryAddr, DeliveryCity, FgNeedDelivery, Remark, Product, ProductName, Specification,PriceForex, Unit, SO_Date"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ResultSame = "SO_No"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbCount_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlDeliveryDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeliveryDate.SelectedIndexChanged
        Try

            If ddlDeliveryDate.SelectedValue = "" Then
                tbDeliveryDate.SelectedDate = ViewState("ServerDate")
            Else
                tbDeliveryDate.SelectedDate = ddlDeliveryDate.SelectedValue
            End If


        Catch ex As Exception
            lbStatus.Text = "ddlDeliveryDate_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub
End Class