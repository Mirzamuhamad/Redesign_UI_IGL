Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Data.Odbcf
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrRecaivingPland
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringPoHd As String = "SELECT DISTINCT TransNmbr, Nmbr, Revisi, TransDate, Status, FgReport, ShipmentType, SuppContractNo, CustContractNo, Supplier, Attn, Term, TermPayment, FgAddCost, AddCostRemark, Delivery, DeliveryAddr, DeliveryCity, Currency, ForexRate, BaseForex, Disc, DiscForex, DP, DPForex, PPn, PPnForex, PPhForex, OtherForex, TotalForex, Remark, UserPrep, DatePrep, UserAppr, DateAppr, FgActive, POType, SupplierName, FgPriceIncludeTax, FactorRate, A.Department, A.DepartmentName, DecPlacePrice, DecPlaceBaseForex From V_PRPOHd A Where POType <> 'Service'  "
    Protected GetStringDeliveryHd As String = "Select * From V_PRPODeliveryHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                FillCombo(ddlWarehouse, "Select Wrhs_Code, Wrhs_Name from VMsWarehouse WHERE Wrhs_Type IN ('Owner', 'Deposit Out') ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                'FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                Session("AdvanceFilter") = ""
                BtnAdd.Visible = False
                btnAdd2.Visible = BtnAdd.Visible
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
                    tbAddress1.Text = Session("Result")(2).ToString
                    tbAddress2.Text = Session("Result")(3).ToString
                    BindToText(tbAttn, Session("Result")(4).ToString)
                    BindToDropList(ddlWarehouse, Session("Result")(5).ToString)
                    EnableDelivery(True)
                End If

                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    ' tbFgHome.Text = ddlHome.SelectedValue
                End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("ProductCode = " + QuotedStr(drResult("Product")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("ProductCode") = drResult("Product")
                            dr("Product_Name") = drResult("ProductName")
                            dr("Unit") = TrimStr(drResult("UnitWrhs"))
                            dr("Revisi") = tbrev.Text
                            dr("ItemNo") = drResult("ItemNo")
                            dr("Qty") = drResult("Qty")
                            dr("ShipTo") = tbSuppCode.Text
                            dr("JmlPacking") = drResult("JmlPacking")
                            dr("QtyPacking") = drResult("QtyPacking")
                            dr("UnitPacking") = drResult("UnitPacking")
                            dr("StartDelivery") = drResult("DStart")
                            dr("EndDelivery") = drResult("DEnd")
                            dr("FgHome") = ddlHome.SelectedValue
                            ViewState("Dt").Rows.Add(dr)
                        End If
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
        'ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitQty") = 2
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'Me.tbQtyWrhs.Attributes.Add("ReadOnly", "True")

        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnBlur", "QtyxPrice(" + Me.tbQty.ClientID + "); setformatdt();")
        ViewState("DeleteHome") = ""
        ViewState("Deleteshipto") = ""
        ViewState("HdValue") = ""
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
            DT = BindDataTransaction(GetStringPoHd, StrFilter, ViewState("DBConnection").ToString)
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
            'btnAdd2.Visible = BtnGo.Visible

            ddlCommand.Visible = False
            btnGo2.Visible = False
            'btnAdd2.Visible = False
            ddlCommand2.Visible = False
            BtnGo.Visible = False
            BtnAdd.Visible = False

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
        Return "SELECT * From V_PRPODeliveryDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Private Function GetStringhd(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_PRPODeliveryHd WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
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
                Session("SelectCommand") = "EXEC S_PRFormReqRetur " + Result
                Session("ReportFile") = ".../../../Rpt/FormPRRetur.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRPO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlHome.Enabled = State
            tbRef.Enabled = State
            'btnReffNo.Visible = State
            tbEndDate.Enabled = State And ViewState("StateHd") = "Insert"
            tbStartDate.Enabled = State
            'tbSuppCode.Enabled = State
            'btnSupp.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableDelivery(ByVal State As Boolean)
        Try
            btnGetDt.Enabled = State
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
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataHd(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Hd") = Nothing
            dt = SQLExecuteQuery(GetStringhd(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Hd") = dt
            BindGridDt(dt, GridView2)
        Catch ex As Exception
            Throw New Exception("Bind Data hd Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
            pnlDelivery.Visible = False
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
                    If CekExistData(ViewState("Dt"), "ProductCode", tbProductCode.Text) Then
                        lbStatus.Text = "ProductCode " + tbProductName.Text + " has already been exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ProductCode = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ProductCode") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("FgHome") = ddlHome.SelectedValue
                Row("ShipTo") = tbSuppCode.Text
                Row("ItemNo") = "1"
                Row("Qty") = tbQty.Text
                Row("Unit") = tbUnit.Text
                Row("Revisi") = tbrev.Text
                Row("StartDelivery") = tbStartDel.Text
                Row("EndDelivery") = tbEndDel.Text
                'Row("Revisi") = tbRevisi.Text
                Row("QtyPacking") = tbQty.Text
                Row("JmlPacking") = tbjmlpack.Text
                Row("UnitPacking") = tbUnit.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ProductCode", tbProductCode.Text) = True Then
                    lbStatus.Text = "ProductCode " + tbProductName.Text + " has already been exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ProductCode") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("FgHome") = ddlHome.SelectedValue
                dr("ShipTo") = tbSuppCode.Text
                dr("ItemNo") = "1"
                dr("Qty") = tbQty.Text
                dr("Unit") = tbUnit.Text
                dr("Revisi") = tbrev.Text
                dr("StartDelivery") = tbStartDel.Text
                dr("EndDelivery") = tbEndDel.Text
                dr("QtyPacking") = tbQty.Text
                dr("UnitPacking") = tbUnit.Text
                dr("JmlPacking") = tbjmlpack.Text
                dr("Remark") = tbRemarkDt.Text
                dr.EndEdit()
                ViewState("Dt").Rows.Add(dr)

            End If
            'MovePanel(pnlEditDt, pnlDt)
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
            ' If ViewState("HdValue") = "" Then
            'lbStatus.Text = "Detail Data must be saved first"
            ' ViewState("HdValue") = ""
            'EnableDelivery(False)
            ' Exit Sub
            ' End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            ElseIf ViewState("StateSave") = "Delete" Then
                ViewState("StateSave") = "Delete"
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"

            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                If SQLExecuteScalar("SELECT TransNmbr From PRCPODeliveryHd WHERE TransNmbr = " + QuotedStr(tbRef.Text) + "And Revisi = " + QuotedStr(tbrev.Text) + " AND FgHome = " + QuotedStr(ddlHome.SelectedValue) + " AND ShipTo = " + QuotedStr(tbSuppCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "TransNmbr " + QuotedStr(tbRef.Text) + " has already been exist"
                    Exit Sub
                End If
                'tbRef.Text = GetAutoNmbr("PRR", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PRCPODeliveryHd (TransNmbr,Revisi, FgHome, ShipTo, Attn, Address1, Address2, StartRR, EndRR, WrhsCode, Remark) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", " + tbrev.Text + " , " + QuotedStr(ddlHome.SelectedValue) + ", '" + Format(tbSuppCode.Text) + "', " + _
                QuotedStr(tbAttn.Text) + ", " + QuotedStr(tbAddress1.Text) + ", " + QuotedStr(tbAddress2.Text) + ", '" + _
                Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "' , '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlWarehouse.SelectedValue) + ", " + _
                QuotedStr(tbRemark.Text)

            ElseIf ViewState("StateSave") = "Delete" Then
                'If SQLExecuteScalar("SELECT TransNmbr From PRCPODeliveryHd WHERE TransNmbr = " + QuotedStr(tbRef.Text) + "And Revisi = " + QuotedStr(tbrev.Text) + " AND FgHome = " + QuotedStr(ddlHome.SelectedValue) + " AND ShipTo = " + QuotedStr(tbSuppCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                '    lbStatus.Text = "TransNmbr " + QuotedStr(tbRef.Text) + " has already been exist"
                '    Exit Sub
                'End If
                'tbRef.Text = GetAutoNmbr("PRR", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "Delete PRCPODeliveryHd Where TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " + tbrev.Text + " AND FgHome = " + QuotedStr(ViewState("DeleteHome")) + " AND ShipTo = " + QuotedStr(ViewState("Deleteshipto"))

            Else
                'Dim cekStatus As String
                'cekStatus = SQLExecuteScalar("Select FgHome FROM PRCPODeliveryHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                'If cekStatus = "Y" Then
                '    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                '    Exit Sub
                'End If
                SQLString = "UPDATE PRCPODeliveryHd SET ShipTo = " + QuotedStr(tbSuppCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + ", WrhsCode = " + QuotedStr(ddlWarehouse.SelectedValue) + _
                ", Address1 = " + QuotedStr(tbAddress1.Text) + ", Address2 = " + QuotedStr(tbAddress2.Text) + ", startRR = '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', EndRR = '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', Remark = " + QuotedStr(tbRemark.Text) + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text) + "AND Revisi= " + QuotedStr(tbrev.Text) + " AND FgHome+'|'+ShipTo = " + QuotedStr(ViewState("HdValue"))
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I)("Revisi") = ViewState("Revisi")
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Revisi, FgHome, ShipTo, ItemNo, ProductCode, Qty, Unit, StartDelivery, EndDelivery, QtyRR, QtyClose, QtyBL, FgPackages, QtyPacking, UnitPacking, JmlPacking, Remark  FROM PRCPODeliveryDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'" + " AND Revisi = " + tbrev.Text, con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PRCPODeliveryDt SET Revisi = @Revisi, FgHome = @FgHome, ShipTo = @ShipTo, ItemNo = @ItemNo,  Remark = @Remark, " + _
                     " ProductCode = @ProductCode,Qty = @Qty, Unit = @Unit, StartDelivery = @StartDelivery, EndDelivery = @EndDelivery, QtyRR = @QtyRR, " + _
                     " FgPackages = @FgPackages, QtyPacking = @QtyPacking, UnitPacking = @UnitPacking,  " + _
                     " JmlPacking = @JmlPacking " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " + tbrev.Text + " AND ShipTo = @OldShipTo AND ProductCode = @OldProductCode AND FgHome = @OldFgHome", con)

            'Define output parameters.
            Update_Command.Parameters.Add("@Revisi", SqlDbType.Int, 4, "Revisi")
            Update_Command.Parameters.Add("@FgHome", SqlDbType.VarChar, 1, "FgHome")
            Update_Command.Parameters.Add("@ShipTo", SqlDbType.VarChar, 12, "ShipTo")
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 20, "ItemNo")
            Update_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductCode")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@StartDelivery", SqlDbType.DateTime, 8, "StartDelivery")
            Update_Command.Parameters.Add("@EndDelivery", SqlDbType.DateTime, 8, "EndDelivery")
            Update_Command.Parameters.Add("@QtyRR", SqlDbType.Float, 9, "QtyRR")
            Update_Command.Parameters.Add("@FgPackages", SqlDbType.VarChar, 1, "FgPackages")
            Update_Command.Parameters.Add("@QtyPacking", SqlDbType.Float, 9, "QtyPacking")
            Update_Command.Parameters.Add("@UnitPacking", SqlDbType.VarChar, 5, "UnitPacking")
            Update_Command.Parameters.Add("@JmlPacking", SqlDbType.Float, 9, "JmlPacking")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")

            'Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProductCode", SqlDbType.VarChar, 20, "ProductCode")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldShipTo", SqlDbType.VarChar, 12, "ShipTo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldFgHome", SqlDbType.VarChar, 1, "FgHome")
            param.SourceVersion = DataRowVersion.Original
            'Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
            "DELETE FROM PRCPODeliveryDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ShipTo = @ShipTo AND ProductCode = @ProductCode AND FgHome =@FgHome", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductCode")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@ShipTo", SqlDbType.VarChar, 12, "ShipTo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@FgHome", SqlDbType.VarChar, 1, "FgHome")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCPODeliveryDt")

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
            'If CekHd() = False Then
            '    Exit Sub
            'End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                EnableDelivery(True)
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
            pnlDelivery.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(pnlDelivery, pnlinputdel)

            ' ModifyInput2(True, pnlDelivery, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            EnableDelivery(False)
            ViewState("StateHd") = "Insert"

            'tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbStartDel.SelectedDate = ViewState("ServerDate") 'Today
            ClearHd()
            Cleardt()
            pnlDt.Visible = False
            BindDataDt("", 0)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            'tbRef.Text = ViewState("Transnmbr")
            'tbRevisi.Text = ViewState("Revisi")
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbAttn.Text = ""
            tbStartDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEndDate.SelectedDate = ViewState("ServerDate") 'Today
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            ddlWarehouse.SelectedValue = ""
            tbRemark.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbjmlpack.Text = ""
            tbRemarkDt.Text = ""
            tbQty.Text = ""
            tbUnit.Text = ""
            tbQty.Text = ""
            tbUnit.Text = ""
            tbStartDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEndDate.SelectedDate = ViewState("ServerDate") 'Today

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
            pnlDelivery.Visible = False

        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "select * from V_RRPOShipTo WHERE FgHome = " + QuotedStr(ddlHome.SelectedValue)
            ResultField = "Ship_To, Ship_To_Name, Address1, Address2, Attn, WareHouse "
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "select * from VMsProduct "
            ResultField = "Product_Code, Product_Name, On_Hand, Unit "
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try

    End Sub


    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select Ship_To, Ship_To_Name, Address1, Address2, Attn,Warehouse FROM V_RRPOShipTo WHERE FgHome = " + QuotedStr(ddlHome.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbSuppCode.Text = Dr("Ship_To").ToString
                tbSuppName.Text = Dr("Ship_To_Name").ToString
                tbAttn.Text = Dr("Attn").ToString
                tbAddress1.Text = Dr("Address1").ToString
                tbAddress2.Text = Dr("Address2").ToString
                ddlWarehouse.SelectedValue = Dr("Warehouse").ToString
                EnableDelivery(True)
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbAddress1.Text = ""
                tbAddress2.Text = ""
                ddlWarehouse.SelectedValue = 0
                EnableDelivery(False)
            End If
            'AttachScript("setformatdt();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("Subcode Error : " + ex.ToString)
        End Try
    End Sub


    'Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbSuppCode.TextChanged
    '    Dim Dt As DataTable
    '    Dim Dr As DataRow
    '    Try
    '        Dt = SQLExecuteQuery("Select Ship_To, Ship_To_Name, Address1, Address2, Attn,Warehouse FROM V_RRPOShipTo WHERE FgHome = " + QuotedStr(ddlHome.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
    '        If Dt.Rows.Count > 0 Then
    '            Dr = Dt.Rows(0)
    '            tbSuppCode.Text = Dr("Ship_To").ToString
    '            tbSuppName.Text = Dr("Ship_To_Name").ToString
    '            tbAttn.Text = Dr("Attn").ToString
    '            tbAddress1.Text = Dr("Address1").ToString
    '            tbAddress2.Text = Dr("Address2").ToString
    '            tbSuppName.Text = Dr("Attn").ToString
    '            ddlWarehouse.SelectedValue = Dr("Warehouse").ToString
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "tbBatch_SelectedIndexChanged Error : " + ex.ToString
    '    End Try
    'End Sub


    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select * FROM VMsProduct WHERE Product_Code = '" + tbProductCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code").ToString
                tbProductName.Text = Dr("Product_Name").ToString

            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb TK PanenCode Error : " + ex.ToString)
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

    Function CekHd(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Shipto").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Ship to Must Have Value")
                    Return False
                End If

                If tbSuppCode.Text = "" Then
                    lbStatus.Text = MessageDlg("Ship To must have value")
                    btnSupp.Focus()
                    Return False
                End If

            Else
                ''If tbStartDate.IsNull Then
                ''    lbStatus.Text = MessageDlg("RR Date must have value")
                ''    tbStartDate.Focus()
                ''    Return False
                ''End If
                If tbSuppCode.Text = "" Then
                    lbStatus.Text = MessageDlg("Ship To must have value")
                    btnSupp.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("ProductCode").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If

                If Dr("Qty").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyPacking").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Packing Must Have Value")
                    Return False
                End If
            Else
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If tbUnit.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    tbUnit.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                'If CFloat(tbQtyWrhs.Text) <= 0 Then
                '    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                '    tbQtyWrhs.Focus()
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
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In GridView1.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, FgHome, ShipTo, Attn, Remark"
            FilterValue = "TransNmbr,FgHome, ShipTo, Attn, Remark"
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
            If e.CommandName = "Go" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlDelivery)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(3).Text

                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))

                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(pnlInput, pnlDelivery)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        ViewState("DigitCurr") = GetCurrDigit(ViewState("Currency"), ViewState("DBConnection").ToString)
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Receving Plan" Then
                    'BtnAdd_Click(Nothing, Nothing)
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(3).Text

                    tbRef.Text = GVR.Cells(2).Text
                    tbrev.Text = GVR.Cells(3).Text
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(3).Text
                    pnlDelivery.Visible = True
                    pnlinputdel.Visible = False
                    pnlDt.Visible = False
                    ViewState("StateHd") = "Edit"
                    ModifyInput2(True, pnlInput, pnlDt, GridDt)
                    BindDataHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    BtnAdd.Visible = True
                    btnAdd2.Visible = BtnAdd.Visible
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        If LCase(ViewState("UserId")) <> "admin" Then
                            CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                            If CekMenu <> "" Then
                                lbStatus.Text = CekMenu
                                Exit Sub
                            End If
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_PRFormReqRetur ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormPRRetur.frx"
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
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ProductCode = " + QuotedStr(GVR.Cells(1).Text))
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



    'Protected Sub lbSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupp.Click
    '    Try
    '        'Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier');", Page, Me.GetType)
    '        'AttachScript("OpenMaster('MsSupplier')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Supplier Error : " + ex.ToString
    '    End Try
    'End Sub

    Dim BaseForex As Decimal = 0

    '' untuk tampilkan data total di grid
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                tbBaseForex.Text = CStr(BaseForex) ' FormatNumber(BaseForex, ViewState("DigitCurr"))
    '                AttachScript("BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click

        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            'If Not CekHd() Then
            '    Exit Sub
            'End If
            Session("Result") = Nothing
            If ddlHome.SelectedValue = "" Then
                Exit Sub
            End If
            'Session("filter") = " EXEC S_PRPODeliveryGetProduct '" + ddlHome.SelectedValue + "', '" + tbSuppCode.Text + "', '" + tbStartDate.Text + "', '" + tbEndDate.Text + "', '" + tbRevisi.Text + "', '" + tbProductCode.Text + "' "
            Session("filter") = " EXEC S_PRPODeliveryGetProduct " + QuotedStr(ddlHome.SelectedValue) + ", " + QuotedStr(tbSuppCode.Text) + ",  " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ",  " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", '', " + QuotedStr(tbRef.Text) + ", " + tbrev.Text

            ResultField = "ItemNo, Product, ProductName, Specification, UnitWrhs, Qty, JmlPacking, QtyPacking, UnitPacking, Dstart, DEnd"
            CriteriaField = "ItemNo, Product, ProductName, Specification, UnitWrhs,Qty, JmlPacking, QtyPacking, UnitPacking, Dstart, DEnd"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")

            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
        'Dim StrFilter, SqlString As String
        'Dim GVR As GridViewRow = Nothing
        'Dim Result As String

        'Try
        '    StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
        '    tbRevisi.Text = GVR.Cells(3).Text
        '    tbProductCode.Text = GVR.Cells(2).Text

        '    SqlString = "EXEC S_PRPODeliveryGetProduct '" + ddlHome.SelectedValue + "', '" + tbSuppCode.Text + "', '" + tbStartDate.Text + "', '" + tbEndDate.Text + "', '" + tbRevisi.Text + "', '" + tbProductCode.Text + "', " + StrFilter
        '    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
        '    If Result.Length > 0 Then
        '        'Result = Dt.Rows(0)
        '        tbSuppCode.Text = Result("Ship_To").ToString
        '        tbSuppName.Text = Result("Ship_To_Name").ToString
        '        tbAttn.Text = Result("Attn").ToString
        '        tbAddress1.Text = Result("Address1").ToString
        '        tbAddress2.Text = Result("Address2").ToString
        '        tbSuppName.Text = Result("Attn").ToString

        '    End If

        '    'If ViewState("SortExpression") = Nothing Then
        '    '    ViewState("SortExpression") = "ShipTo ASC"
        '    'End If

        '    BindDataMaster(SqlString, GridView1, ViewState("SortExpression"), ViewState("DBConnection"))
        '    MovePanel(pnlInput, PnlHd)

        'Catch ex As Exception
        '    lbStatus.Text = lbStatus.Text + "BindDataGrid Error: " & ex.ToString
        'Finally
        'End Try

        ''Dim ResultField As String 'ResultSame 
        ''Try
        ''    Session("Result") = Nothing
        ''    Session("Filter") = "select * from VMsAccount"
        ''    ResultField = "Account, Description, Currency, FgSubled"
        ''    Session("Column") = ResultField.Split(",")
        ''    'ResultSame = "Currency"
        ''    'Session("ResultSame") = ResultSame.Split(",")
        ''    ViewState("Sender") = "btnGetDt"
        ''    AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        ''    'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
        ''    '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenSearchMultiDlg();", True)
        ''    'End If
        ''Catch ex As Exception
        ''    lbStatus.Text = "btn get Dt Error : " + ex.ToString
        ''End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringDeliveryHd, "TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)

            tbRef.Text = ViewState("Transnmbr")
            tbrev.Text = ViewState("Revisi")
            BindToText(tbAddress1, Dt.Rows(0)("Address1").ToString)
            BindToDropList(ddlWarehouse, Dt.Rows(0)("wrhsCode").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("ShipTo").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("ShipToName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbAddress2, Dt.Rows(0)("Address2").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("StartRR").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndRR").ToString)
            ' BindToText(tbRef, Dt.Rows(0)("Transnmbr").ToString)
            ' BindToText(tbrev, Dt.Rows(0)("Revisi").ToString)
            ViewState("DigitCurr") = 0
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
                BindToDropList(ddlHome, Dr(0)("FgHome").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToDate(tbStartDel, Dr(0)("StartDelivery").ToString)
                BindToDate(tbEndDel, Dr(0)("EndDelivery").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbUnitPacking, Dr(0)("UnitPacking").ToString)
                BindToText(tbQtyPacking, Dr(0)("QtyPacking").ToString)
                BindToText(tbjmlpack, Dr(0)("JmlPacking").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDelivery(ByVal FgHome As String, ByVal Shipto As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Hd").select("FgHome+'|'+ShipTo = " + QuotedStr(FgHome + "|" + Shipto))
            If Dr.Length > 0 Then
                BindToText(tbAddress1, Dr(0)("Address1").ToString)
                BindToDropList(ddlHome, Dr(0)("FgHome").ToString)
                BindToDropList(ddlWarehouse, Dr(0)("wrhsCode").ToString)
                BindToText(tbSuppCode, Dr(0)("ShipTo").ToString)
                BindToText(tbSuppName, Dr(0)("ShipToName").ToString)
                BindToText(tbAttn, Dr(0)("Attn").ToString)
                BindToText(tbAddress2, Dr(0)("Address2").ToString)
                BindToDate(tbStartDate, Dr(0)("StartRR").ToString)
                BindToDate(tbEndDate, Dr(0)("EndRR").ToString)
                BindToText(tbRemark, Dr(0)("Remark").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
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
    'Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged, ddlUnit.TextChanged
    '    Try
    '        tbQtyWrhs.Text = FindConvertUnit(tbProductCode.Text, ddlUnit.SelectedValue, tbQty.Text, ViewState("DBConnection").ToString).ToString
    '        tbQtyWrhs.Text = FormatFloat(tbQtyWrhs.Text, ViewState("DigitQty"))
    '        tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
    '        tbQty.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
    '    End Try
    'End Sub


    'Protected Sub ddlReffType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReffType.SelectedIndexChanged
    '    tbReffNo.Text = ""
    '    tbPONo.Text = ""
    '    btnReffNo.Visible = Not ddlReffType.SelectedValue = "Others"
    '    tbReffNo.Enabled = Not btnReffNo.Visible
    'End Sub

    'Protected Sub btnReffNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReffNo.Click
    '    Dim ResultField As String 'ResultSame 
    '    Try
    '        If tbSuppCode.Text = "" Then
    '            If ViewState("StateHd") = "Insert" Then
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' "
    '            Else
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' and Report = " + QuotedStr(ddlReport.SelectedValue)
    '            End If
    '        Else
    '            If ViewState("StateHd") = "Insert" Then
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' AND Supplier = '" + tbSuppCode.Text + "' "
    '            Else
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' AND Supplier = '" + tbSuppCode.Text + "' and Report = " + QuotedStr(ddlReport.SelectedValue)
    '            End If
    '        End If
    '        ResultField = "Reference, PONo, Supplier, Supplier_Name, Attn, Report"
    '        ViewState("Sender") = "btnReffNo"
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn get Dt Error : " + ex.ToString
    '    End Try

    'End Sub



    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    Try
    '        tbReffNo.Text = ""
    '        tbPONo.Text = ""
    '    Catch ex As Exception
    '        lbStatus.Text = "ddlReport_SelectedIndexChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnCancelDelivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDelivery.Click
        Try
            pnlDelivery.Visible = True
            pnlinputdel.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub GridView2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        Try
            GridView2.PageIndex = e.NewPageIndex
            GridView2.DataSource = ViewState("Hd")
            GridView2.DataBind()
        Catch ex As Exception
            lbStatus.Text = "GridView2_PageIndexChanging Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        Dim GVR As GridViewRow = Nothing
        Try

            If e.CommandName = "Detail" Then
                GVR = GridView2.Rows(Convert.ToInt32(e.CommandArgument))
                lblShipto.Text = GVR.Cells(2).Text
                lblShiptoName.Text = GVR.Cells(3).Text
                tbFgHome.Text = GVR.Cells(1).Text
                'LblBatchNameMT.Text = GVR.Cells(4).Text
                Dim drow As DataRow()
                If ViewState("Dt") Is Nothing Then
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                Else
                    drow = ViewState("Dt").Select("FgHome+'|'+Shipto= " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt)
                        GridDt.Columns(0).Visible = Not ViewState("StateHd") = "View"
                        pnlDt.Visible = True
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt.DataSource = DtTemp
                        GridDt.DataBind()
                        GridDt.Columns(0).Visible = False
                    End If
                End If
                pnlDelivery.Visible = False
                pnlinputdel.Visible = False
                pnlDt.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "GridView2 Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridView2.Rows(e.NewEditIndex)
            FillTextBoxDelivery(GVR.Cells(1).Text, GVR.Cells(2).Text)
            ViewState("HdValue") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text
            pnlDelivery.Visible = False
            pnlinputdel.Visible = True
            'EnableHd(False)
            ViewState("StateHd") = "Edit"
            'StatusButtonSave(False)

        Catch ex As Exception
            lbStatus.Text = "Grid dt Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView2.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridView2.Rows(e.RowIndex)
        dr = ViewState("Hd").Select("FgHome = " + QuotedStr(GVR.Cells(1).Text) + " And Shipto = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()

        ViewState("DeleteHome") = GVR.Cells(1).Text
        ViewState("Deleteshipto") = GVR.Cells(2).Text
        ViewState("StateSave") = "Delete"
        BindData()
        BindGridDt(ViewState("Hd"), GridView2)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
            gv.Columns(0).Visible = Not IsEmpty
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSaveDelivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDelivery.Click
        Dim Row As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If ViewState("StateHd") = "Edit" Then
                If ViewState("HdValue") <> ddlHome.SelectedValue + "|" + tbSuppCode.Text Then
                    If CekExistData(ViewState("Hd"), "FgHome+'|'+ShipTo", ddlHome.SelectedValue + "|" + tbSuppCode.Text) Then
                        lbStatus.Text = "Ship To " + tbSuppName.Text + " has already been exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Hd").Select("FgHome+'|'+Shipto = " + QuotedStr(ViewState("HdValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row("FgHome") = ddlHome.SelectedValue
                Row("Shipto") = tbSuppCode.Text
                Row("ShipToName") = tbSuppName.Text
                Row("Attn") = tbAttn.Text
                Row("Address1") = tbAddress1.Text
                Row("Address2") = tbAddress2.Text
                Row("StartRR") = Format(tbStartDate.SelectedValue, "yyyy-MM-dd")
                Row("EndRR") = Format(tbEndDate.SelectedValue, "yyyy-MM-dd")
                Row("WrhsCode") = ddlWarehouse.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                ' ViewState("Dt").AcceptChanges()

            Else
                'Insert
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                If CekExistData(ViewState("Hd"), "FgHome,Shipto ", ddlHome.SelectedValue + "|" + tbSuppCode.Text) = True Then
                    lbStatus.Text = "Ship To " + tbSuppName.Text + " has already been exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Hd").NewRow
                dr("FgHome") = ddlHome.SelectedValue
                dr("Shipto") = tbSuppCode.Text
                dr("ShipToName") = tbSuppName.Text
                dr("Attn") = tbAttn.Text
                dr("Address1") = tbAddress1.Text
                dr("Address2") = tbAddress2.Text
                dr("StartRR") = Format(tbStartDate.SelectedValue, "yyyy-MM-dd")
                dr("EndRR") = Format(tbEndDate.SelectedValue, "yyyy-MM-dd")
                dr("WrhsCode") = ddlWarehouse.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                '  dr.EndEdit()
                ViewState("Hd").Rows.Add(dr)

            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Hd"), GridView2)
            StatusButtonSave(True)
            pnlDelivery.Visible = True
            pnlinputdel.Visible = False
            pnlDt.Visible = False

        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub


    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2.Click
        Try
            pnlDt.Visible = False
            pnlEditDt.Visible = False
            pnlDelivery.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btnBackDt2_Click : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged
        Try
            'lbStatus.Text = MessageDlg("tbQty_TextChanged")
            If CFloat(tbjmlpack.Text) = 0 Then
                tbjmlpack.Text = "0"
            Else
                'tbQtyPacking.Text = CFloat(tbQty.Text) / CFloat(tbjmlpack.Text)
                tbjmlpack.Text = CFloat(tbQty.Text) / CFloat(tbQtyPacking.Text)
                tbjmlpack.Text = FormatFloat(tbjmlpack.Text, 0)
                ' tbQty.Text = CInt(tbQty.Text)
            End If
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyPacking_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged, tbQtyPacking.TextChanged
        Dim Jml As Double
        Try
            'lbStatus.Text = MessageDlg("QtyPack")
            If CFloat(tbQty.Text) <> 0 Then
                tbjmlpack.Text = CFloat(tbQty.Text) / CFloat(tbQtyPacking.Text)
                tbjmlpack.Text = FormatNumber(tbjmlpack.Text, 0)
                ' Jml = tbjmlpack.Text * tbQty.Text
            End If
        Catch ex As Exception
            lbStatus.Text = "tbQtyPacking_TextChanged : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbjmlpack_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbjmlpack.TextChanged
        Dim Jml As Double
        Try
            If CFloat(tbjmlpack.Text) = 0 Then
                tbQty.Text = "0"
            Else
                tbQtyPacking.Text = CFloat(tbQty.Text) / CFloat(tbjmlpack.Text)
                tbQtyPacking.Text = FormatNumber(tbQtyPacking.Text, 0)
                ' Jml = tbjmlpack.Text * tbQty.Text
            End If

        Catch ex As Exception
            lbStatus.Text = "tbjmlpack_TextChanged : " + ex.ToString
        End Try
    End Sub

End Class
