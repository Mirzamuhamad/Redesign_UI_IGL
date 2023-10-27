Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrTransferInternal_TrTransferInternal
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "SELECT * From V_STCTransferHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetWrhsUserArea") = False
                FillCombo(ddlWrhsArea, "EXEC S_GetWrhsArea", True, "Wrhs_Area_Code", "Wrhs_Area_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsAreaDest, "EXEC S_GetWarehouse", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlwrhs, "EXEC S_GetWarehouse", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
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
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbUnit, Session("Result")(2).ToString)
                    'BindToText(tbSpecification, Session("Result")(3).ToString)
                    GetInfo(tbProductCode.Text)
                End If
                tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        If CekExistData(ViewState("Dt"), "ProductSrc", drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ProductSrc") = drResult("Product_Code")
                            dr("ProductName") = drResult("Product_Name")
                            'dr("Specification") = drResult("Specification")
                            dr("UnitSrc") = drResult("Unit")
                            dr("QtySrc") = FormatFloat(0, ViewState("DigitQty"))
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
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        'If Request.QueryString("ContainerId").ToString = "TGSJID" Then
        'ViewState("Type") = "FG"
        'Else
        'ViewState("Type") = "NFG"
        'End If

        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnBlur", "setformat();")
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
        Return "SELECT * From V_STCTransferDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC  S_STFormTransInternal" + Result
                Session("ReportFile") = ".../../../Rpt/FormSTI.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_STTransfer", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlWrhsArea.Enabled = State
            ddlwrhs.Enabled = State
            tbSubLed.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            ddlWrhsAreaDest.Enabled = State
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
                    If CekExistData(ViewState("Dt"), "ProductSrc", tbProductCode.Text) Then
                        lbStatus.Text = "ProductSrc " + tbProductName.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ProductSrc = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ProductSrc") = tbProductCode.Text
                Row("ProductName") = tbProductName.Text
                'Row("Specification") = tbSpecification.Text
                Row("QtySrc") = tbQty.Text
                Row("UnitSrc") = tbUnit.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "ProductSrc", tbProductCode.Text) = True Then
                    lbStatus.Text = "ProductSrc " + tbProductName.Text + " has been already exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ProductSrc") = tbProductCode.Text
                dr("ProductName") = tbProductName.Text
                'dr("Specification") = tbSpecification.Text
                dr("QtySrc") = tbQty.Text
                dr("UnitSrc") = tbUnit.Text
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
                'If ViewState("Type").ToString = "FG" Then
                tbReference.Text = GetAutoNmbr("STI", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlwrhs.SelectedValue, ViewState("DBConnection").ToString)
                'Else
                '   tbReference.Text = GetAutoNmbr("TGOM", "N", CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ddlwrhs.SelectedValue, ViewState("DBConnection").ToString)
                'End If


                SQLString = "INSERT INTO STCTransferHd (TransNmbr, Status, TransDate,WrhsArea, WrhsSrc, WrhsDest, FgSrcSubLed, FgDestSubLed,WrhsSrcSubLed,WrhsDestSubLed " + _
                ", Operator, TransferType,TGType, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlWrhsArea.SelectedValue) + ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(ddlWrhsAreaDest.SelectedValue) + ", " + _
                QuotedStr(tbFgSubLed.Text) + ", " + QuotedStr(tbFgSubLedDest.Text) + ", " + _
                QuotedStr(tbSubLed.Text) + ", " + QuotedStr(tbSubLedDest.Text) + ", " + _
                QuotedStr(tbPIC.Text) + ", 'Location','INTERNAL'," + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM STCTGSJHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE STCTransferHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", WrhsArea = " + QuotedStr(ddlWrhsArea.SelectedValue) + ", WrhsDest = " + QuotedStr(ddlWrhsAreaDest.SelectedValue) + _
                ", WrhsSrc = " + QuotedStr(ddlwrhs.SelectedValue) + ", FgSrcSubLed = " + QuotedStr(tbFgSubLed.Text) + _
                ", FgDestSubLed = " + QuotedStr(tbFgSubLedDest.Text) + ", Operator = " + QuotedStr(tbPIC.Text) + _
                ", WrhsSrcSubLed = " + QuotedStr(tbSubLed.Text) + ", WrhsDestSubLed = " + QuotedStr(tbSubLedDest.Text) + _
                " , Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ProductSrc, QtySrc, UnitSrc, Remark FROM STCTransferDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'lbStatus.Text = tbReference.Text + "Test"
            'Exit Sub

            Dim paramInsert As SqlParameter
            ' Create the UpdateCommand.
            Dim Insert_Command = New SqlCommand( _
                    "INSERT INTO STCTransferDt  (TransNmbr,ProductSrc,QtySrc,UnitSrc,Remark,ProductDest) Values (" + QuotedStr(tbReference.Text) + ",@ProductCode,  @Qty, @Unit, @Remark, @ProductCode)", con)
            ' Define output parameters.
            Insert_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductSrc")
            Insert_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "QtySrc")
            Insert_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "UnitSrc")
            Insert_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.

            da.InsertCommand = Insert_Command



            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE STCTransferDt SET ProductSrc= @ProductCode, QtySrc = @Qty, UnitSrc = @Unit, Remark = @Remark  " + _
                    " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ProductSrc = @OldProductCode ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductSrc")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "QtySrc")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "UnitSrc")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProductCode", SqlDbType.VarChar, 20, "ProductSrc")
            param.SourceVersion = DataRowVersion.Original

            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM STCTransferDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ProductSrc = @ProductCode ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductSrc")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("STCTransferDt")


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
            PnlInfo.Visible = False
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
            'tbIssuedBy.Text = ViewState("UserId")
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            ddlWrhsArea.SelectedIndex = 0
            ddlWrhsArea.Enabled = True
            'ddlwrhs.SelectedIndex = 0
            'ddlwrhs.Enabled = True
            tbFgSubLed.Text = "N"
            tbSubLedDest.Text = ""
            tbSubLed.Enabled = tbSubLedDest.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            tbSubLedName.Text = ""
            ddlWrhsAreaDest.SelectedIndex = 0
            ddlWrhsAreaDest.Enabled = True
            tbPIC.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbPartCOde.Text = ""
            tbPartName.Text = ""
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
        Dim ResultField, CriteriaField As String
        Try
            'If ViewState("Type") = "FG" Then
            'CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Merk " 'Product_Class, Product_Merk, Product_Size, Product_Grade, Product_Motif
            'Else
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Sub_Group, Product_Type"
            'End If
            Session("Filter") = "EXEC S_STAdjustReff '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ",'N'"
            ResultField = "Product_Code, Product_Name, Unit, Specification, Qn_Hand"
            Session("CriteriaField") = CriteriaField.Split(",")

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
            'If ViewState("Type") = "FG" Then
            'Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit, Product_Merk  FROM VMsProductFGSimpleUser WHERE Product_Code = '" + tbProductCode.Text + "' AND UserId = '" + ViewState("UserId") + "'", ViewState("DBConnection").ToString).Tables(0)
            'Else
            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Product_Code = " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString).Tables(0)
            'End If
            'Dr = FindMaster("Product", tbProductCode.Text, ViewState("DBConnection").ToString)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                'tbSpecification.Text = TrimStr(Dr("Specification").ToString)
                tbUnit.Text = Dr("Unit").ToString
                tbQty_TextChanged(Nothing, Nothing)
                GetInfo(tbProductCode.Text)
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                'tbSpecification.Text = ""
                tbUnit.Text = ""
                PnlInfo.Visible = False
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
            If ddlWrhsArea.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Wrhs Area Issue must have value")
                ddlWrhsArea.Focus()
                Return False
            End If
            If ddlwrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Wrhs Source must have value")
                ddlwrhs.Focus()
                Return False
            End If
            If ddlWrhsAreaDest.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse Area Dest have value")
                ddlWrhsAreaDest.Focus()
                Return False
            End If

            'If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
            '    lbStatus.Text = MessageDlg("Issue SubLed must have value")
            '    tbSubLed.Focus()
            '    Return False
            'End If

            If tbPIC.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("PIC must have value")
                tbPIC.Focus()
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
                If Dr("ProductSrc").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtySrc").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("UnitSrc").ToString.Trim = "" Then
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
            FilterName = "Reference, Date, Status, Wrhs Area Issue, Warehouse Issue, Issue Subled Name, Wrhs Area Dest Name, Operator Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, WrhsAreaSrc_Name, Wrhs_Name, Subled_Name, WrhsAreaDest_Name, Operator Remark"
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
                        ViewState("SetWrhsUserArea") = True
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
                        Session("SelectCommand") = "EXEC S_STFormTransInternal ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormSTI.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String

                    Try
                        paramgo = "TRANSFER SJ|" + GVR.Cells(2).Text
                        'lbStatus.Text = paramgo
                        'Exit Sub
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'RR Other' "
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
        dr = ViewState("Dt").Select("ProductSrc = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        EnableHd(GridDt.Rows.Count = 0)
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
            BindToDropList(ddlWrhsArea, Dt.Rows(0)("WrhsArea").ToString)
            BindToDropList(ddlwrhs, Dt.Rows(0)("WrhsSrc").ToString)
            BindToDropList(ddlWrhsAreaDest, Dt.Rows(0)("WrhsDest").ToString)
            BindToText(tbFgSubLedDest, Dt.Rows(0)("WrhsDestSubLed").ToString)
            BindToText(tbSubLedDestName, Dt.Rows(0)("WrhsDestSubLed_Name").ToString)
            BindToText(tbSubLed, Dt.Rows(0)("WrhsSrcSubLed").ToString)
            BindToText(tbSubLedName, Dt.Rows(0)("WrhsSrcSubLed_Name").ToString)
            BindToText(tbFgSubLed, Dt.Rows(0)("FgSrcSubLed").ToString)
            BindToText(tbFgSubLedDest, Dt.Rows(0)("FgDestSubLed").ToString)
            BindToText(tbPIC, Dt.Rows(0)("PIC").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ProductSrc = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProductCode, Dr(0)("ProductSrc").ToString)
                GetInfo(tbProductCode.Text)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                'BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbQty, Dr(0)("QtySrc").ToString)
                BindToText(tbUnit, Dr(0)("UnitSrc").ToString)
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

    Protected Sub btnSubLedDest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubLedDest.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubLedDest.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLedDest"
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

    Protected Sub tbSubLedDest_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubLedDest.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("SubLed", tbSubLed.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSubLedDest.Text = Dr("SubLed_No")
                tbSubLedDestName.Text = Dr("SubLed_Name")
            Else
                tbSubLedDest.Text = ""
                tbSubLedDestName.Text = ""
            End If
            AttachScript("setformat();", Page, Me.GetType())
            tbSubLedDest.Focus()
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
            'If ViewState("Type") = "FG" Then
            'CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Merk" ', Product_Class, Product_Merk, Product_Size, Product_Grade, Product_Motif
            'Else
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Sub_Group, Product_Type "
            'End If
            Session("Filter") = "EXEC S_STAdjustReff '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 'N' "
            ResultField = "Product_Code, Product_Name, specification, Unit, On_Hand"
            Session("CriteriaField") = CriteriaField.Split(",")
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
                tbFgSubLed.Text = Dr("FgSubLed")
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



    Protected Sub lbWrhsArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWrhsArea.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsWrhsArea')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Wrhs Area Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlWrhsArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsArea.SelectedIndexChanged
        ViewState("SetWrhsUserArea") = True
        If ddlWrhsArea.SelectedValue.Trim = ddlWrhsAreaDest.SelectedValue.Trim Then
            lbStatus.Text = "Wrhs Area Issue cannot same Wrhs Area Destination"
            ddlWrhsArea.Focus()
        End If
        If ViewState("SetWrhsUserArea") Then
            FillCombo(ddlwrhs, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId")) + ", " + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            ViewState("SetWrhsUserArea") = False
        End If

    End Sub

    Protected Sub ddlWrhsAreaDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsAreaDest.SelectedIndexChanged

        Dim Dr As DataRow
        Try
            Dr = FindMaster("WrhsUser", ddlWrhsAreaDest.SelectedValue + "|" + ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            ' lbStatus.Text = Dr("FgSubLed")

            If Not Dr Is Nothing Then
                tbFgSubLedDest.Text = Dr("FgSubLed")
                tbSubLedDest.Text = ""
                tbSubLedName.Text = ""
            Else
                tbFgSubLedDest.Text = "N"
                tbSubLedDest.Text = ""
                tbSubLedName.Text = ""
            End If
            tbSubLedDest.Enabled = tbFgSubLedDest.Text <> "N"
            btnSubLedDest.Visible = tbSubLedDest.Enabled
            ddlWrhsAreaDest.Focus()

            If ddlWrhsAreaDest.SelectedValue.Trim = ddlWrhsArea.SelectedValue.Trim Then
                lbStatus.Text = "Wrhs Area Issue cannot same Wrhs Area Destination"
                ddlWrhsAreaDest.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("ddlWrhsAreaDest error : " + ex.ToString)
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

    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged
        Try
            tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try

    End Sub



End Class
