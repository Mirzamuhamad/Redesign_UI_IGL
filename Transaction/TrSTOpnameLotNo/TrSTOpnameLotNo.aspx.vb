Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrSTOpnameLotNo_TrSTOpnameLotNo
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_STCOpnameLotNoHd "

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_STCOpnameLotNoDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT *, dbo.FormatFloat(Qty,2) as QtyKey FROM V_STCOpnameLotNoDt2 WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetGrade") = False
                FillCombo(ddlWrhs, "select Wrhs_Code, Wrhs_Name from VMsWarehouse", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            dsWarehouse.ConnectionString = ViewState("DBConnection")
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnproduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    TbProductName.Text = Session("Result")(1).ToString
                    lblUnit.Text = Session("Result")(3).ToString
                    BindToDropList(ddlWrhs, Session("Result")(4).ToString)
                    tbQtyOH.Text = FormatNumber(Session("Result")(5).ToString, Viewstate("DigitQty"))
                    tbQtySystemOH.Text = FormatNumber(Session("Result")(6).ToString, Viewstate("DigitQty"))
                    tbQtyActualOH.Text = FormatNumber(Session("Result")(6).ToString, Viewstate("DigitQty"))
                    tbQtySystemPackages.Text = FormatNumber(Session("Result")(7).ToString, Viewstate("DigitQty"))
                    tbQtyActualPackages.Text = FormatNumber(Session("Result")(7).ToString, Viewstate("DigitQty"))
                    tbQtyOH_TextChanged(Nothing, Nothing)
                    tbQtyActualPackages.Text = tbQtySystemPackages.Text
                End If

                If ViewState("Sender") = "SearchAdd" Or ViewState("Sender") = "SearchEdit" Then
                    Dim Product As TextBox
                    Dim ProductName As Label
                    If ViewState("Sender") = "SearchAdd" Then
                        Product = GridDt2.FooterRow.FindControl("ProductAdd")
                        ProductName = GridDt2.FooterRow.FindControl("ProductNameAdd")
                    Else
                        Product = GridDt2.Rows(GridDt2.EditIndex).FindControl("ProductEdit")
                        ProductName = GridDt2.Rows(GridDt2.EditIndex).FindControl("ProductNameEdit")
                    End If
                    Product.Text = Session("Result")(0).ToString
                    ProductName.Text = Session("Result")(1).ToString
                    Product.Focus()
                End If


                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        'insert
                        Row = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product_Code")) + " AND Warehouse = " + QuotedStr(drResult("Warehouse")))

                        If Row.Count = 0 Then
                            'Dim dre As SqlDataReader
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Warehouse") = drResult("Warehouse")
                            dr("Warehouse_Name") = drResult("Warehouse_Name")
                            dr("QtyOnHand") = FormatNumber(drResult("On_Hand"), Viewstate("DigitQty"))
                            dr("QtySystemOH") = FormatNumber(drResult("OH_System"), Viewstate("DigitQty"))
                            dr("QtySystemPackage") = FormatNumber(drResult("OH_Packages"), Viewstate("DigitQty"))
                            dr("QtyActualOH") = FormatNumber(drResult("OH_System"), Viewstate("DigitQty"))
                            dr("QtyActualPackage") = FormatNumber(drResult("OH_Packages"), Viewstate("DigitQty"))
                            dr("Unit") = drResult("Unit")
                            dr("Remark") = ""
                            InputDetail2(drResult("Product_Code"), drResult("Warehouse"))
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = True
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
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        'tbQtySystem.Attributes.Add("ReadOnly", "True")
        'tbQtyAdjust.Attributes.Add("ReadOnly", "True")
        'Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'tbQtyActual.Attributes.Add("OnBlur", "hitung('');")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetStringHd1 As String
        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            Else
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            End If
            GetStringHd1 = "Select * From V_STCOpnameLotNoHd "

            DT = BindDataTransaction(GetStringHd1, StrFilter, ViewState("DBConnection").ToString)
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
                    Result = ExecSPCommandGo(ActionValue, "S_STOpnameLotNo", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbDate.Enabled = State
            tbRemark.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt2(ByVal State As Boolean)
        Try
            tbLotNo.Enabled = State
            tbLotQty.Enabled = State
            tbPalletNo.Enabled = State
            
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

    Private Sub BindDataDt2(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
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

    Private Sub SaveAll()
        Dim SQLString As String
        Try
            'savedt2()
            'If pnlDt.Visible = False Then
            '    lbStatus.Text = "Detail Data must be saved first"
            '    Exit Sub
            'End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then

                tbRef.Text = GetAutoNmbr("SLN", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO STCOpnameLotNoHd (TransNmbr, TransDate, Status, OpnameDate, Remark, UserPrep, DatePrep) " + _
                            "SELECT '" + tbRef.Text + "', '" + _
                            Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', 'H', " + _
                            QuotedStr(Format(tbOpnameDate.SelectedValue, "yyyy-MM-dd")) + ", '" + _
                            tbRemark.Text + "', " + _
                            QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM STCOpnameLotNoHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE STCOpnameLotNoHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                            " OpnameDate = " + QuotedStr(Format(tbOpnameDate.SelectedValue, "yyyy-MM-dd")) + _
                            ", Remark = " + QuotedStr(tbRemark.Text) + _
                            ", DateAppr = getDate() WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Warehouse, QtyOnHand, QtySystemOH, QtySystemPackage, QtyActualOH, QtyActualPackage, Remark FROM STCOpnameLotNoDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE STCOpnameLotNoDt SET Product = @Product, Warehouse = @Warehouse, QtyOnHand = @QtyOnHand, QtySystemOH = @QtySystemOH, QtySystemPackage = @QtySystemPackage, QtyActualOH = @QtyActualOH, QtyActualPackage = @QtyActualPackage, Remark = @Remark WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @Produk AND Warehouse = @OldWarehouse", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Warehouse", SqlDbType.VarChar, 20, "Warehouse")
            Update_Command.Parameters.Add("@QtyOnHand", SqlDbType.Float, 18, "QtyOnHand")
            Update_Command.Parameters.Add("@QtySystemOH", SqlDbType.Float, 18, "QtySystemOH")
            Update_Command.Parameters.Add("@QtySystemPackage", SqlDbType.Float, 18, "QtySystemPackage")
            Update_Command.Parameters.Add("@QtyActualOH", SqlDbType.Float, 18, "QtyActualOH")
            Update_Command.Parameters.Add("@QtyActualPackage", SqlDbType.Float, 18, "QtyActualPackage")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@Produk", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldWarehouse", SqlDbType.VarChar, 20, "Warehouse")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM STCOpnameLotNoDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @Produk AND Warehouse = @OldWarehouse", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Produk", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@OldWarehouse", SqlDbType.VarChar, 20, "Warehouse")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("STCOpnameLotNoDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, Product, Warehouse, LotNo, Qty, QtyPackageSystem, QtyPackageActual, ExpireDate, PalletNo, Remark, Status FROM STCOpnameLotNoDt2 WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)

            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim param1 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command1 = New SqlCommand( _
                    "UPDATE STCOpnameLotNoDt2 SET Product = @Product, Warehouse = @Warehouse, LotNo = @LotNo, Qty = @Qty, QtyPackageSystem = @QtyPackageSystem, QtyPackageActual = @QtyPackageActual, ExpireDate = @ExpireDate, PalletNo = @PalletNo, Remark = @Remark, Status = @Status WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @OldProduct AND Warehouse = @OldWarehouse AND LotNo = @OldLotNo AND Qty = @OldQty", con)

            Update_Command1.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command1.Parameters.Add("@Warehouse", SqlDbType.VarChar, 20, "Warehouse")
            Update_Command1.Parameters.Add("@LotNo", SqlDbType.VarChar, 20, "LotNo")
            Update_Command1.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command1.Parameters.Add("@QtyPackageSystem", SqlDbType.Float, 18, "QtyPackageSystem")
            Update_Command1.Parameters.Add("@QtyPackageActual", SqlDbType.Float, 18, "QtyPackageActual")
            Update_Command1.Parameters.Add("@ExpireDate", SqlDbType.DateTime, 8, "ExpireDate")
            Update_Command1.Parameters.Add("@PalletNo", SqlDbType.VarChar, 20, "PalletNo")
            Update_Command1.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")
            Update_Command1.Parameters.Add("@Status", SqlDbType.VarChar, 255, "Status")

            ' Define intput (WHERE) parameters.
            param1 = Update_Command1.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Update_Command1.Parameters.Add("@OldWarehouse", SqlDbType.VarChar, 20, "Warehouse")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Update_Command1.Parameters.Add("@OldLotNo", SqlDbType.VarChar, 20, "LotNo")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Update_Command1.Parameters.Add("@OldQty", SqlDbType.Float, 20, "Qty")
            param1.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command1

            ' Create the DeleteCommand.
            Dim Delete_Command1 = New SqlCommand( _
                "DELETE FROM STCOpnameLotNoDt2 WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @Product AND Warehouse = @Warehouse And LotNo = @LotNo And Qty = @Qty ", con)
            ' Add the parameters for the DeleteCommand.
            param1 = Delete_Command1.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Delete_Command1.Parameters.Add("@Warehouse", SqlDbType.VarChar, 20, "Warehouse")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Delete_Command1.Parameters.Add("@LotNo", SqlDbType.VarChar, 20, "LotNo")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Delete_Command1.Parameters.Add("@Qty", SqlDbType.Float, 20, "Qty")
            param1.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command1

            Dim Dt2 As New DataTable("STCOpnameLotNoDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
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

            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
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
            ModifyInput2(True, pnlInput, pnlDt, GridDt2)
            newTrans()
            EnableHd(True)
            btnHome.Visible = False
            'Panel1.Visible = True
            tbDate.Focus()
            btnGetDt.Visible = True
            btnAddDt3.Visible = True
            btnAddDt4.Visible = True
      	        MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("Add") = "Clear"
            ViewState("DigitCurr") = 0
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            GridDt.Columns(1).Visible = False
            ViewState("PrevQuotation") = ""
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbOpnameDate.SelectedDate = ViewState("ServerDate")
            tbDate.SelectedDate = ViewState("ServerDate")
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            TbProductName.Text = ""
            tbQtyOH.Text = "0"
            tbQtySystemOH.Text = "0"
            tbQtyActualOH.Text = "0"
            tbQtyActualPackages.Text = "0"
            tbQtySystemPackages.Text = "0"
            tbRemarkDt.Text = ""
            ddlWrhs.SelectedValue = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbLotNo.Text = ""
            tbLotQty.Text = "0"
            tbPalletNo.Text = "0"
            tbExpireDate.SelectedDate = ViewState("ServerDate")
            tbQtyActual.Text = "0"
            tbStatus.Text = ""
            tbRemarkDt2.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt2 Error " + ex.ToString)
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
        Dim DtCek, DtCek2 As DataTable
        Dim SQLCek, SQLCek2 As String
        Dim DrCek2 As DataRow
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
            EnableHd(True)
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "btnAddDt_Click error : " + ex.ToString
        End Try
        
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
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
                
                If Dr("QtyOnHand").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty OH System Must Have Value")
                    Return False
                End If
            Else
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
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
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                'If Dr("QtyActualPackage").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Qty Actual Packages Must Have Value")
                '    Return False
                'End If
            Else
		If tbLotNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Lot No Must Have Value")
                    tbLotNo.Focus()
                    Return False
                End If
                'If tbQtyActualPackages.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Qty Actual Packages  Must Have Value")
                '    tbProductCode.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt 2Error : " + ex.ToString)
        End Try
    End Function
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
        
    End Sub

    Protected Sub cbSelectDt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Quotation No, Start Effective Date, Customer Code, Customer Name, Currency, Price Include Tax, Remark"
            FilterValue = "TransNmbr, QuotationNo, dbo.FormatDate(StartEffective), Customer, Customer_Name, Currency, PriceIncludeTax, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            Session("DateTime") = ViewState("ServerDate")
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt2.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    BindDataDt2(ViewState("Reference"))
                    GridDt2.Columns(0).Visible = False
                    GridDt.Columns(0).Visible = False
                    ViewState("StateHd") = "View"
                    MultiView1.ActiveViewIndex = 0
                    ModifyInput2(False, pnlInput, pnlDt, GridDt2)
                    btnHome.Visible = True
                    btnGetDt.Visible = False
                    btnAddDt3.Visible = False
                    btnAddDt4.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt2.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        BindDataDt2(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        GridDt2.Columns(0).Visible = True
                        GridDt2.Columns(1).Visible = True
                        GridDt.Columns(1).Visible = True
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt2)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        btnGetDt.Visible = True
                        btnAddDt3.Visible = True
                        btnAddDt4.Visible = True
        	        MultiView1.ActiveViewIndex = 0
	
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Session("DBCOnnection") = ViewState("DBConnection")
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FormOpnameLotNoPrint " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/RptMKTCustComplain.frx"
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

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt2.PageIndexChanging
        Try
            GridDt2.PageIndex = e.NewPageIndex
            GridDt2.DataSource = ViewState("Dt2")
            GridDt2.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridDt2.RowCancelingEdit
        Try
            GridDt2.ShowFooter = False
            GridDt2.EditIndex = -1
            BindDataDt2(ViewState("Dt2"))
            GridDt2.Columns(0).Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GridDt2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt2.RowCommand
        
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim lbProduct, lbwrhs, lbLotNo As Label
            Dim tbQty, tbQtyPackageActual, tbQtyPackagesystem As Label
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            lbProduct = GVR.FindControl("Product")
            lbwrhs = GVR.FindControl("Warehouse")
            lbLotNo = GVR.FindControl("LotNo")
            tbQty = GVR.FindControl("QtyKey")
            tbQtyPackageActual = GVR.FindControl("QtyPackageActual")
            tbQtyPackagesystem = GVR.FindControl("QtyPackageSystem")
            'tbQtyActual = GVR.FindControl("QtyPackages")
            If CFloat(tbQtyPackagesystem.Text) > 0 Then
                lbStatus.Text = "Cannot delete, Lot no have qty packages in system"
                Exit Sub
            End If
            ViewState("Dt2Value") = LabelProduct.Text + "|" + LabelWrhs.Text + "|" + lbLotNo.Text + "|" + Trim(tbQty.Text)

            dr = ViewState("Dt2").Select("Product+'|'+Warehouse+'|'+LotNo+'|'+QtyKey  = " + QuotedStr(ViewState("Dt2Value")))
            If dr.Count <> 0 Then
                dr(0).Delete()
            End If


            dr = ViewState("Dt2").Select("Product+'|'+Warehouse = " + QuotedStr(LabelProduct.Text + "|" + LabelWrhs.Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDt2)
                ' GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
		
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            GridDt2.Columns(0).Visible = True
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim tbLotNo, tbLotQty As Label
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            tbLotNo = GVR.FindControl("LotNo")
            tbLotQty = GVR.FindControl("QtyKey")

           'ViewState("Dt2Value") = LabelProduct.Text + "|" + LabelWrhs.Text + "|" + tbLotNo.Text + "|" + FormatNumber(tbLotQty.text, Viewstate("DigitQty"))
            ViewState("Dt2Value") = TrimStr(LabelProduct.Text.Trim + "|" + LabelWrhs.Text.Trim + "|" + tbLotNo.Text.Trim + "|" + Trim(tbLotQty.Text))
            
            FillTextBoxDt2(ViewState("Dt2Value"))

            'BindDataDt2(ViewState("Dt2Value"))
            MovePanel(pnlDt2, PnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            EnableDt2(False)
            tbQtyActual.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbOpnameDate, Dt.Rows(0)("OpnameDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product+'|'+Warehouse = " + QuotedStr(Product))
            
            If Dr.Length > 0 Then
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(TbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbQtyOH, Dr(0)("QtyOnHand").ToString)
                BindToText(tbQtySystemOH, Dr(0)("QtySystemOH").ToString)
                BindToText(tbQtySystemPackages, Dr(0)("QtySystemPackage").ToString)
                BindToText(tbQtyActualOH, Dr(0)("QtyActualOH").ToString)
                BindToText(tbQtyActualPackages, Dr(0)("QtyActualPackage").ToString)
                BindToDropList(ddlWrhs, Dr(0)("Warehouse").ToString)
                lblUnit.Text = Dr(0)("Unit").ToString
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Product+'|'+Warehouse+'|'+LotNo+'|'+QtyKey = " + QuotedStr(Product)) '+'|'+Qty 

            If Dr.Length > 0 Then
                BindToText(tbLotQty, Dr(0)("Qty").ToString)
                BindToText(tbLotQtyKey, Dr(0)("QtyKey").ToString)
                BindToText(tbQtyActual, Dr(0)("QtyPackageActual").ToString)
                BindToText(tbLotNo, Dr(0)("LotNo").ToString)
                BindToDate(tbExpireDate, Dr(0)("ExpireDate").ToString)
                BindToText(tbPalletNo, Dr(0)("PalletNo").ToString)
                BindToText(tbStatus, Dr(0)("Status").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)

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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                'If ViewState("DtValue") <> TrimStr(tbProductCode.Text) + "|" + TrimStr(tbProductCodeFG.Text) + "|" + TrimStr(tbWONo.Text) Then
                '    If CekExistData(ViewState("Dt"), "Product,ProductFG,WONo", TrimStr(tbProductCode.Text) + "|" + TrimStr(tbProductCodeFG.Text) + "|" + TrimStr(tbWONo.Text)) Then
                '        lbStatus.Text = "Data has already exists"
                '        Exit Sub
                '    End If
                'End If

                'Row = ViewState("Dt").Select("Product+'|'+ProductFG+'|'+WONo = " + QuotedStr(ViewState("DtValue")))(0)
                Row = ViewState("Dt").Select("Product+'|'+Warehouse = " + QuotedStr(ViewState("DtValue")))(0)

                If CekDt() = False Then
                    Exit Sub
                End If

                Row.BeginEdit()

                Row("Product") = tbProductCode.Text
                Row("Product_Name") = TbProductName.Text
                Row("Warehouse") = ddlWrhs.SelectedValue
                Row("Warehouse_Name") = ddlWrhs.SelectedItem.Text
                Row("QtyOnHand") = FormatNumber(tbQtyOH.Text, Viewstate("DigitQty"))
                Row("QtySystemOH") = FormatNumber(tbQtySystemOH.Text, Viewstate("DigitQty"))
                Row("QtyActualOH") = FormatNumber(tbQtyActualOH.Text, Viewstate("DigitQty"))
                Row("QtySystemPackage") = FormatNumber(tbQtySystemPackages.Text, Viewstate("DigitQty"))
                Row("QtyActualPackage") = FormatNumber(tbQtyActualPackages.Text, Viewstate("DigitQty"))
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "Product", TrimStr(tbProductCode.Text)) Then
                    lbStatus.Text = MessageDlg("Data has already exists")
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = TbProductName.Text
                dr("Warehouse") = ddlWrhs.SelectedValue
                dr("Warehouse_Name") = ddlWrhs.SelectedItem.Text
                dr("QtyOnHand") = FormatNumber(tbQtyOH.Text, Viewstate("DigitQty"))
                dr("QtySystemOH") = FormatNumber(tbQtySystemOH.Text, Viewstate("DigitQty"))
                dr("QtyActualOH") = FormatNumber(tbQtyActualOH.Text, Viewstate("DigitQty"))
                dr("QtySystemPackage") = FormatNumber(tbQtySystemPackages.Text, Viewstate("DigitQty"))
                dr("QtyActualPackage") = FormatNumber(tbQtyActualPackages.Text, Viewstate("DigitQty"))
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If

            InputDetail2(tbProductCode.Text, ddlWrhs.SelectedValue)
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            GridDt.Columns(1).Visible = True
            'ViewState("Add") = "NotClear"
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
            btnGetDt.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Dim SqlString As String
        Dim LbProduct, LbWrhs As Label
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect1")
                LbProduct = GRW.FindControl("Product")
                LbWrhs = GRW.FindControl("Warehouse")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    
    
    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbProduct_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Function GetQtySystem(ByVal tgl As DateTime, ByVal product As String) As Double
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_STOpnameLotNoGetQtySystem " + QuotedStr(product) + ", '" + Format(tgl, "yyyyMMdd") + "'", ViewState("DBConnection").ToString)
        dr.Read()
        Return dr("QtySystem")
    End Function

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim QSystem As Double
        Try
            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Unit FROM VMsProduct WHERE Product_Code = '" + tbProductCode.Text + "' ", ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                TbProductName.Text = Dr("Product_Name")
                QSystem = GetQtySystem(Format(tbOpnameDate.SelectedValue, "yyyy-MM-dd"), tbProductCode.Text)
                tbQtySystemOH.Text = FormatNumber(QSystem, ViewState("DigitQty"))
                tbQtyOH.Text = FormatNumber(QSystem, ViewState("DigitQty"))
                tbQtyOH_TextChanged(Nothing, Nothing)
                'tbQtyActual_TextChanged(Nothing, Nothing)
            Else
                tbProductCode.Text = ""
                TbProductName.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbQtyActualOH.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField, CriteriaField, DefaultField As String
        Try
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, Warehouse, On_Hand, OH_System, OH_Packages"
            'End If
            Session("Filter") = "EXEC S_STOpnameLotNoReff '" + Format(tbOpnameDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString) + ""
            ResultField = "Product_Code, Product_Name, Specification, Unit, Warehouse, On_Hand, OH_System, OH_Packages"
            Session("CriteriaField") = CriteriaField.Split(",")
            DefaultField = "Product_Name"
            Session("ColumnDefault") = DefaultField.Split(",")
            ViewState("Sender") = "btnproduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProduct_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If

            Session("Result") = Nothing
            CriteriaField = "Product_Code, Product_Name, Specification, Warehouse, Warehouse_Name, On_Hand, Unit, OH_System, OH_Packages"
            Session("Filter") = "EXEC S_STOpnameLotNoReff '" + Format(tbOpnameDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ViewState("UserId").ToString)
            ResultField = "Product_Code, Product_Name, Specification, Warehouse, Warehouse_Name, On_Hand, Unit, OH_System, OH_Packages"
            Session("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
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

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2.Click
        Try
	    if viewstate("StateHd") <> "View" then 	
                MultiView1.ActiveViewIndex = 0
              Menu1.Items.Item(0).Selected = True
              PnlWOgetdata.Visible = True
              StatusButtonSave(Not ViewState("StateHd") = "View")
              btnGetDt.Visible = True And (Not ViewState("StateHd") = "View")
                ' btnHome.Visible = True And ViewState("StateHd") = "View"
           
            Else
                MultiView1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True
                PnlWOgetdata.Visible = True
                StatusButtonSave(Not ViewState("StateHd") = "View")
 	    end if	

        Catch ex As Exception
            lbStatus.Text = "btnBackDt2_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub saveDt2()
        Dim Row As DataRow
        Dim Dr As DataRow
        Dim tbLotNo, lbQty, tbPalletNo As Label
        'Dim tbExpireDate As BasicFrame.WebControls.BasicDatePicker
        Dim tbExpireDate, tbQtyPackageActual, tbRemark, tbstatus, tbQtyPackagesystem As Label
        Dim QtyFG As Double

        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True
            QtyFG = 0
            If exe Then
                ' simpan ke GridDT

                For i = 0 To GridDt2.Rows.Count - 1
                    GVR = GridDt2.Rows(i)
                    tbQtyPackageActual = GVR.FindControl("QtyPackageActual")
                    tbQtyPackagesystem = GVR.FindControl("QtyPackagesystem")
                    tbPalletNo = GVR.FindControl("PalletNo")
                    tbRemark = GVR.FindControl("Remark")
                    If Len(GVR.FindControl("ExpireDate")) <> 0 Then
                        tbExpireDate = GVR.FindControl("ExpireDate")
                    End If


                    tbLotNo = GVR.FindControl("LotNo")
                    lbQty = GVR.FindControl("Qty")

                    ' tbQtyPackageActual.Text = tbQtyPackageActual.Text.Replace(",", "")
                    If tbQtyPackageActual.Text.Trim = "" Then
                        tbQtyPackageActual.Text = "0"
                    End If

                    If Not IsNumeric(tbQtyPackageActual.Text) Then
                        lbStatus.Text = "Actual " + tbQtyPackageActual.Text + " must in numeric format"
                        exe = False
                        tbQtyPackageActual.Focus()
                        Exit For
                    End If

                Next
                If exe Then
                    ' simpan ke GridDT2
                    For i = 0 To GridDt2.Rows.Count - 1
                        GVR = GridDt2.Rows(i)

                        lbProduct = GVR.FindControl("Product")
                        tbQtyPackageActual = GVR.FindControl("QtyPackageActual")
                        tbQtyPackagesystem = GVR.FindControl("QtyPackageSystem")
                        tbPalletNo = GVR.FindControl("PalletNo")
                        tbRemark = GVR.FindControl("Remark")

                        tbExpireDate = GVR.FindControl("ExpireDate")
                        tbLotNo = GVR.FindControl("LotNo")
                        lbQty = GVR.FindControl("Qty")
                        tbstatus = GVR.FindControl("Status")

                        Row = ViewState("Dt2").Select("Product = " + QuotedStr(LabelProduct.Text) + " And Warehouse = " + QuotedStr(LabelWrhs.Text) + " And LotNo = " + QuotedStr(tbLotNo.Text) + " And Qty = " + FormatNumber(lbQty.Text, ViewState("DigitQty")))(0)
                        If Len(tbQtyPackageActual.Text) <> 0 Then
                            Row.BeginEdit()
                            Row("QtyPackageActual") = FormatNumber(tbQtyPackageActual.Text, ViewState("DigitQty"))
                            Row("QtyPackageSystem") = FormatNumber((tbQtyPackageActual.Text * lbQty.Text), ViewState("DigitQty"))
                            Row("Remark") = tbRemark.Text
                            Row("Status") = tbstatus.Text
                            'Format(tbExpireDate.Text, "dd MMMM yyyy")
                            If Len(tbExpireDate.Text) <> 0 Then
                                Row("ExpireDate") = tbExpireDate.Text
                            End If

                            Row.EndEdit()
                            UpdateDt(LabelProduct.Text, LabelWrhs.Text, Row("QtyPackageSystem"), Row("QtyPackageActual"))
                        End If
                        'End If
                    Next

                End If
                ViewState("StateDt2") = "Cancel"
                Dim drow As DataRow()
                drow = ViewState("Dt2").Select("Product+'|'+Warehouse = " + QuotedStr(LabelProduct.Text + "|" + LabelWrhs.Text))

                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    'GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2.DataSource = DtTemp
                    GridDt2.DataBind()
                    GridDt2.Columns(0).Visible = True
                End If
                'update dt
                'BindGridDt(ViewState("Dt"), GridDt)
                'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                'GridDt2.Columns(1).Visible = True
            End If

        Catch ex As Exception
            lbStatus.Text = "Save Dt2 Error : " + ex.ToString
        End Try
    End Sub

   ' Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
   '     Try
    '        saveDt2()
            'ModifyDt()
     '       MultiView1.ActiveViewIndex = 0
      '      Menu1.Items.Item(0).Selected = True
       '     PnlWOgetdata.Visible = True
        '    StatusButtonSave(True)
       ' Catch ex As Exception
        '    lbStatus.Text = "btnOK_Click Error : " + ex.ToString
       ' End Try
   ' End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Protected Sub Product_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim tbCode, tb As TextBox
        Dim tbName As Label

        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim ddlCurr As DropDownList = New DropDownList
        Try
            tb = sender
            If tb.ID = "ProductAdd" Then
                ' masalah di sini
                Count = GridDt2.Controls(0).Controls.Count
                dgi = GridDt2.Controls(0).Controls(Count - 2) '-1 for allowpaging = False   - 2 allowpaging = True
                tbCode = dgi.FindControl("ProductAdd")
                tbName = dgi.FindControl("ProductNameAdd")
            Else
                Count = GridDt2.EditIndex
                dgi = GridDt2.Rows(Count)
                tbCode = dgi.FindControl("ProductEdit")
                tbName = dgi.FindControl("ProductNameEdit")
            End If
            ds = SQLExecuteQuery("Select Product_Code, Product_Name FROM VMsProduct Where Product_Code = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr("Product_Code").ToString
                tbName.Text = dr("Product_Name").ToString
            End If
        Catch ex As Exception
            lbStatus.Text = "tb Product Changed Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub tbQtyOH_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOH.TextChanged
        Try
            tbQtySystemPackages.Text = CFloat(tbQtySystemOH.Text) - CFloat(tbQtyOH.Text)
            tbQtyActualPackages.Text = FormatNumber(tbQtySystemPackages.Text, ViewState("DigitQty"))
            tbQtyActualOH.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbQtyOH_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            ElseIf e.CommandName = "Detail" Then
                Dim GVR As GridViewRow
                Dim Lb As Label
                Dim dbLotNo, dbqty, dbQtyActual, dbPalletNo As TextBox
                Dim tbExpiredate As BasicFrame.WebControls.BasicDatePicker

                dbLotNo = GridDt2.FooterRow.FindControl("LotNoAdd")
                dbqty = GridDt2.FooterRow.FindControl("QtyAdd")
                dbQtyActual = GridDt2.FooterRow.FindControl("QtyPackageActualAdd")
                dbPalletNo = GridDt2.FooterRow.FindControl("PalletNoAdd")
                tbExpiredate = GridDt2.FooterRow.FindControl("ExpireDateAdd")
                
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                Lb = GVR.FindControl("lbWrhs")

                LabelProduct.Text = GVR.Cells(2).Text
                LabelProductName.Text = GVR.Cells(3).Text
                LabelWrhs.Text = Lb.Text
                LabelWrhsName.Text = GVR.Cells(5).Text

                Dim drow As DataRow()
                'If ViewState("Dt2") Is Nothing Then
                '    'BindDataDt2(ViewState("Reference"))
                '    BindGridDt(ViewState("Dt2"), GridDt2)
                'Else

                drow = ViewState("Dt2").Select("Product+'|'+Warehouse = " + QuotedStr(GVR.Cells(2).Text + "|" + Lb.Text))

                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    '                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2.DataSource = DtTemp
                    GridDt2.DataBind()
                    GridDt2.Columns(0).Visible = False
                End If
                MultiView1.ActiveViewIndex = 1
                StatusButtonSave(False)
                btnGetDt.Visible = False
                'btnOK.Visible = Not ViewState("StateHd") = "View"
                'btnHome.Visible = False
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"

            End If


        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowCreated

    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr(), Dr2() As DataRow
            Dim lb As Label
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            'Dim DV As DataView

            lb = GVR.FindControl("lbWrhs")
            dr = ViewState("Dt").Select("Product+'|'+Warehouse = " + QuotedStr(GVR.Cells(2).Text + "|" + TrimStr(lb.Text)))
            dr(0).Delete()

            Dr2 = ViewState("Dt2").Select("Product+'|'+Warehouse = " + QuotedStr(GVR.Cells(2).Text + "|" + TrimStr(lb.Text)))
            For I As Integer = 0 To (Dr2.Count - 1)
                Dr2(I).Delete()
            Next

            BindGridDt(ViewState("Dt2"), GridDt2)
            BindGridDt(ViewState("Dt"), GridDt)
            'ViewState("Dt").AcceptChanges()
            'BindGridDt(ViewState("Dt"), GridDt)
            BindGridDtView()
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
            lb = GVR.FindControl("lbWrhs")
            ViewState("DtValue") = GVR.Cells(2).Text + "|" + TrimStr(lb.Text)

            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)

            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbProductCode.Focus()
            btnGetDt.Visible = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Sub BindGridDtView()
        Dim IsEmpty As Boolean
        Dim DtTemp, source As DataTable
        Dim dr As DataRow()
        Dim DV As DataView
        Try
            DV = ViewState("Dt").DefaultView
            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "Product ASC"
            End If
            DV.Sort = ViewState("SortExpressionDt")

            IsEmpty = False
            source = DV.Table
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                GridDt.DataSource = DtTemp
            Else
                GridDt.DataSource = source
            End If
            GridDt.DataBind()
            GridDt.Columns(0).Visible = Not IsEmpty
            GridDt.Columns(1).Visible = Not IsEmpty
            'Panel2.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"
            ' PnlInfo.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"
            'Panel1.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindGridDtView2()
        Dim IsEmpty As Boolean
        Dim DtTemp, source As DataTable
        Dim dr As DataRow()
        Dim DV As DataView
        Try
            DV = ViewState("Dt2").DefaultView
            If ViewState("SortExpressionDt2") = Nothing Then
                ViewState("SortExpressionDt2") = "Product ASC"
            End If
            DV.Sort = ViewState("SortExpressionDt2")

            IsEmpty = False
            source = DV.Table
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                GridDt2.DataSource = DtTemp
            Else
                GridDt2.DataSource = source
            End If
            GridDt2.DataBind()
            'GridDt2.Columns(0).Visible = Not IsEmpty
            GridDt2.Columns(1).Visible = Not IsEmpty
            'Panel2.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"
            ' PnlInfo.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"
            'Panel1.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Sub InputDetail2(ByVal Product As String, ByVal Warehouse As String)
        Dim SQLString As String
        Dim drResult As DataRow
        Dim dataDt2 As DataTable
        Try
            SQLString = "Select Warehouse, WarehouseName, Product, ProductName, LotNo, PalletNo, Qty, QtyPackage, dbo.formatdate(ExpireDate) As ExpireDate from V_STStockLotNoForOpname WHERE Product = " + QuotedStr(Product) + " AND Warehouse = " + QuotedStr(Warehouse)
            dataDt2 = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            
            If dataDt2.Rows.Count > 0 Then
                drResult = dataDt2.Rows(0)
            Else
                drResult = Nothing
            End If
            Dim Row As DataRow()
            If Not drResult Is Nothing Then
                'kasih select
                For Each drResult In dataDt2.Rows
                    Row = ViewState("Dt2").Select("Product = " + QuotedStr(Product) + " AND Warehouse = " + QuotedStr(Warehouse) + " AND LotNo = " + QuotedStr(drResult("LotNo").ToString) + " AND Qty = " + drResult("Qty").ToString)
                    If Row.Count = 0 Then
                        Dim dr As DataRow
                        dr = ViewState("Dt2").NewRow
                        dr("Product") = drResult("Product")
                        dr("ProductName") = drResult("ProductName")
                        dr("Warehouse") = drResult("Warehouse")
                        dr("WarehouseName") = drResult("WarehouseName")
                        dr("LotNo") = drResult("LotNo").ToString
                        dr("Qty") = FormatNumber(drResult("Qty"), ViewState("DigitQty"))
                        dr("QtyKey") = FormatNumber(drResult("Qty"), 2)
                        dr("QtyPackageSystem") = FormatNumber(drResult("QtyPackage"), ViewState("DigitQty"))
                        dr("QtyPackageActual") = FormatNumber(drResult("QtyPackage"), ViewState("DigitQty"))

                        'If dr("ExpireDate") Then
                        '	dr("ExpireDate") = (Format(dr("ExpireDate"), "dd MMM yyyy"))
                        'Else : dr("ExpireDate") = Nothing
                        'End If
                        'If Len(drResult("ExpireDate")) <> 0 Then
                        dr("ExpireDate") = drResult("ExpireDate")
                        'End If


                        dr("PalletNo") = drResult("PalletNo")
                        dr("Remark") = ""
                        ViewState("Dt2").Rows.Add(dr)
                    End If
                Next
            End If

            'insert
            BindGridDt(ViewState("Dt2"), GridDt2)

        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "inputDetail2 Error: " & ex.ToString
        End Try
    End Sub
    Private Sub bindDataGrid(ByVal TransNo As String, ByVal TransType As String, ByVal Product As String, ByVal Warehouse As String)
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            SqlString = "SELECT * FROM V_STStockLot WHERE TransNmbr = " + QuotedStr(TransNo) + " AND Product = " + QuotedStr(Product) + " AND Warehouse = " + QuotedStr(Warehouse)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))
            ViewState("Dt2") = tempDS.Tables(0)
            DV = tempDS.Tables(0).DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridDt2)

            Else
                GridDt2.DataSource = DV
                GridDt2.DataBind()
                GridDt2.DataSource = DV
                GridDt2.DataSource = DV
                GridDt2.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "bindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Private Sub UpdateDt(ByVal Product As String, ByVal Warehouse As String, ByVal QtySystemPackage As Double, ByVal QtyActual As Double)
        Dim lbQty As Label
        Dim tbQtyActual, tbQtySystem As Label
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim QtyPackageActual, QtyActualOH, QtySystemOH As Double
        Dim exe As Boolean
        Dim dr As DataRow
        Dim drDt As DataRow()
        Try
            exe = True
            'QtySystemPackage = 0
            QtyPackageActual = 0
            QtyActualOH = 0
            'QtySystemOH = 0
            If GridDt2.Rows.Count = 0 Then
                Dim row1 As DataRow
                row1 = ViewState("Dt").Select("Product = " + QuotedStr(LabelProduct.Text) + " And Warehouse = " + QuotedStr(LabelWrhs.Text))(0)
                row1.BeginEdit()
                row1("QtyActualPackage") = 0
                'row1("QtySystemPackage") = 0
                row1("QtyActualOH") = 0
                'row1("QtyPackageSystem") = 0
                row1.EndEdit()
                BindGridDt(ViewState("Dt"), GridDt)
            Else
                'drDt = ViewState("Dt2").Select("Product = " + QuotedStr(LabelProduct.Text) + " And Warehouse = " + QuotedStr(LabelWrhs.Text))
                For Each dr In ViewState("Dt2").Rows()
                    If Not (dr.RowState = DataRowState.Deleted) Then
                        If dr("Product").ToString = Product And dr("Warehouse").ToString = Warehouse Then
                            QtyActualOH = QtyActualOH + (CFloat(dr("Qty")) * CFloat(dr("QtyPackageActual")))
                            'QtySystemOH = QtySystemOH + (CFloat(dr("Qty")) * CFloat(dr("QtySystemPackage")))
                            QtyPackageActual = QtyPackageActual + CFloat(dr("QtyPackageActual"))
                            'QtySystemPackage = QtySystemPackage + CFloat(dr("QtySystemPackage"))
                        End If
                    End If
                Next

                Dim row As DataRow
                If exe Then
                    row = ViewState("Dt").Select("Product = " + QuotedStr(LabelProduct.Text) + " And Warehouse = " + QuotedStr(LabelWrhs.Text))(0)
                    row.BeginEdit()
                    row("QtyActualPackage") = FormatNumber(QtyPackageActual, ViewState("DigitQty"))
                    row("QtyActualOH") = FormatNumber(QtyActualOH, ViewState("DigitQty"))
                    ' row("QtyPackageSystem") = FormatNumber(QtySystemPackage, ViewState("DigitQty"))
                    row.EndEdit()
                    BindGridDt(ViewState("Dt"), GridDt)

                End If
            End If
        Catch ex As Exception
            lbStatus.Text = " btn OK error : " + ex.ToString
        End Try
    End Sub

    
    Protected Sub btnsavedt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsavedt2.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt2") = "Edit" Then
                If CekDt2() = False Then
                    Exit Sub
                End If

                Row = ViewState("Dt2").Select("Product+'|'+Warehouse+'|'+LotNo+'|'+QtyKey  = " + QuotedStr(ViewState("Dt2Value")))(0)

                Row.BeginEdit()
                Row("Product") = LabelProduct.Text
                Row("Warehouse") = LabelWrhs.Text
                Row("LotNo") = tbLotNo.Text
                Row("Qty") = FormatNumber(tbLotQty.Text, ViewState("DigitQty"))
                Row("QtyKey") = FormatNumber(tbLotQty.Text, 2)
                Row("QtyPackageActual") = FormatNumber(tbQtyActual.Text, ViewState("DigitQty"))
                If Len(tbExpireDate.Text) <> 0 Then
                    Row("ExpireDate") = tbExpireDate.Text
                End If

                Row("PalletNo") = tbPalletNo.Text
                Row("Status") = tbStatus.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()	

                UpdateDt(LabelProduct.Text, LabelWrhs.Text, tbQtyActual.Text, tbQtyActual.Text)

            Else
                'Insert
                If CekDt2() = False Then
                    Exit Sub
                End If

                ' If CekExistData(ViewState("Dt2"), "Product+'|'+Warehouse+'|'+LotNo+'|'+Qty ", TrimStr(LabelProduct.Text + "|" + LabelWrhs.Text + "|" + tbLotNo.Text+ "|" + tbLotQty.Text)) Then
                If CekExistData(ViewState("Dt2"), "Product,Warehouse,LotNo,Qty", TrimStr(LabelProduct.Text) + "|" + TrimStr(LabelWrhs.Text) + "|" + TrimStr(tbLotNo.Text) + "|" + tbLotQty.Text) Then
                    lbStatus.Text = MessageDlg("Data has already exists")
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("Product") = LabelProduct.Text
                dr("Warehouse") = LabelWrhs.Text
                dr("LotNo") = tbLotNo.Text
                dr("Qty") = FormatNumber(tbLotQty.Text, ViewState("DigitQty"))
                dr("QtyKey") = FormatNumber(tbLotQty.Text, 2)
                dr("QtyPackageSystem") = "0"
                dr("QtyPackageActual") = FormatNumber(tbQtyActual.Text, ViewState("DigitQty"))
                If Len(tbExpireDate.Text) <> 0 Then
                    dr("ExpireDate") = tbExpireDate.Text
                End If
                dr("PalletNo") = tbPalletNo.Text
                dr("Status") = tbStatus.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
                UpdateDt(LabelProduct.Text, LabelWrhs.Text, tbQtyActual.Text, tbQtyActual.Text)

                End If
                'saveDt2()
                Dim drow As DataRow()
                drow = ViewState("Dt2").Select("Product+'|'+Warehouse = " + QuotedStr(LabelProduct.Text + "|" + LabelWrhs.Text))

                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    '                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2.DataSource = DtTemp
                    GridDt2.DataBind()
                    GridDt2.Columns(0).Visible = False
                End If

                'InputDetail2(tbProductCode.Text, ddlWrhs.SelectedValue)
                MovePanel(PnlEditDt2, pnlDt2)
                EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                'BindGridDt(ViewState("Dt2"), GridDt2)

                StatusButtonSave(True)
                'GridDt2.Columns(1).Visible = True
                'btnBackDt2_Click(Nothing, Nothing)
                'ViewState("Add") = "NotClear"

        Catch ex As Exception
            lbStatus.Text = "btn saveg Dt 2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btncancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancelDt2.Click
        Try
            MovePanel(PnlEditDt2, pnlDt2)
            ' EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(False)
            btnGetDt.Visible = True
            ' GridDt2.Columns(0).Visible = True
		Viewstate("StateDt2") = "Cancel"
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click, btnAddDt4.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, PnlEditDt2)
            EnableDt2(True)
            ' EnableHd(False)
            ' StatusButtonSave(False)
            tbLotNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "btnAddDt_Click error : " + ex.ToString
        End Try
    End Sub

End Class
