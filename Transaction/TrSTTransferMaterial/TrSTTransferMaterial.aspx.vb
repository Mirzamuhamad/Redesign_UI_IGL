Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_TrSTTransferMaterial_TrSTTransferMaterial
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "Select * From v_SttransferMaterialHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CurrFilter, Value As String

        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                FillCombo(ddlWrhsArea, "EXEC S_GetWrhsArea", True, "Wrhs_Area_Code", "Wrhs_Area_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsAreaDest, "EXEC S_GetWrhsArea", True, "Wrhs_Area_Code", "Wrhs_Area_Name", ViewState("DBConnection"))

                If (ViewState("StrKode") = "TRL") Then
                    FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                Else
                    FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                End If
                FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))

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
                If ViewState("Sender") = "btnProdSrc" Then
                    tbProdSrcCode.Text = Session("Result")(0).ToString
                    tbProdSrcName.Text = Session("Result")(1).ToString
                    tbUnitSrc.Text = Session("Result")(2).ToString
                    
                    tbQtySrc.Text = FormatNumber(Session("Result")(3).ToString, ViewState("DigitQty"))
      
                    tbProdSrcCode.Focus()
                    'GetInfo(tbProdSrcCode.Text)
                End If

                If ViewState("Sender") = "btnReffNo" Then
                    tbReffNo.Text = Session("Result")(0).ToString
                    'ddlWrhsArea.SelectedValue = Session("Result")(1).ToString

                    'FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                    'FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                    'ddlWrhsSrc.SelectedValue = Session("Result")(2).ToString
                    'tbFgSubledSrc.Text = Session("Result")(3).ToString
                    'tbSubledSrc.Text = Session("Result")(4).ToString
                    'tbSubledSrcName.Text = Session("Result")(5).ToString
                    'tbRemark.Text = Session("Result")(6).ToString
                    'ddlWrhsArea.Focus()
                End If
               
                If ViewState("Sender") = "btnSubLedSrc" Then
                    tbSubledSrc.Text = Session("Result")(0).ToString
                    tbSubledSrcName.Text = Session("Result")(1).ToString
                    tbSubledSrc.Focus()
                End If
                If ViewState("Sender") = "btnSubLedDest" Then
                    tbSubledDest.Text = Session("Result")(0).ToString
                    tbSubledDestName.Text = Session("Result")(1).ToString
                    tbSubledDest.Focus()
                End If

                If ViewState("Sender") = "btnGetLHP" Then

                    Dim drResult, dr As DataRow
                    Dim Row, Row2 As DataRow()
                    For Each drResult In Session("Result").Rows

                        Row = ViewState("Dt").Select("ProductSrc = " + QuotedStr(drResult("Product")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("ProductDest") = drResult("Product")
                            dr("ProductSrc") = drResult("Product")
                            dr("Product_Dest_Name") = drResult("Product_Name")
                            dr("Product_Src_Name") = drResult("Product_Name")
                            dr("QtyDest") = FormatNumber(drResult("Qty"), ViewState("DigitQty"))
                            dr("QtySrc") = FormatNumber(drResult("Qty"), ViewState("DigitQty"))
                            dr("UnitDest") = drResult("Unit")
                            dr("UnitSrc") = drResult("Unit")
                            ViewState("Dt").Rows.Add(dr)
                        Else
                            dr = ViewState("Dt").Select("ProductSrc = " + QuotedStr(drResult("Product")))(0)
                            dr.BeginEdit()
                            If drResult("Qty") <> dr("QtySrc") Then
                                dr("QtySrc") = dr("QtySrc") + CFloat(drResult("Qty"))
                            End If
                            If drResult("Qty") <> dr("QtyDest") Then
                                dr("QtyDest") = dr("QtyDest") + CFloat(drResult("Qty"))
                            End If
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)

                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                      
                        dr("ProductCode") = drResult("Product_Code")
                        dr("ProductName") = drResult("Product_Name")
                        dr("Qty") = FormatNumber(drResult("Qty"), ViewState("DigitHome"))
                        dr("LocationSrc") = "GEN"
                        dr("LocationDest") = "GEN"
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
        'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
        '    ddlCommand.Items.Add("Print")
        '    ddlCommand2.Items.Add("Print")
        'End If

        
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
        Return "SELECT * From v_SttransferMaterialdt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

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
                Session("SelectCommand") = "EXEC S_STFormTransferMaterial " + Result + "," + QuotedStr(lbTrans.Text)
                Session("ReportFile") = ".../../../Rpt/FormSTTransferMaterial.frx"

                'If ViewState("TransferType").ToString = "Location" Or ViewState("TransferType").ToString = "Blokir" Then
                '    Session("ReportFile") = ".../../../Rpt/FormSttransferMaterialLoc.frx"
                'Else : Session("ReportFile") = ".../../../Rpt/FormSttransferMaterialQC.frx"
                'End If
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
                        Result = ExecSPCommandGo(ActionValue, "S_STCTransferMaterial", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlWrhsArea.Enabled = State
            ddlWrhsSrc.Enabled = State
            tbSubledSrc.Enabled = State And tbFgSubledSrc.Text.Trim <> "N"
            btnSubledSrc.Visible = tbSubledSrc.Enabled
            btnReffNo.Visible = State
            btnGetDt.Visible = State
            ddlWrhsDest.Enabled = State
            tbSubledDest.Enabled = State And tbFgSubledDest.Text.Trim <> "N"
            BtnSubledDest.Visible = tbSubledDest.Enabled
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

                tbTransNo.Text = GetAutoNmbr("TRM", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO stctransferMaterialhd(TransNmbr,Status,TransDate,WoServiceNo,WrhsArea,WrhsSrc,FgSrcSubLed,SrcSubLed,WrhsDest,WrhsAreaDest,FgDestSubLed,DestSubLed,TechInCharge,Remark,UserPrep,DatePrep) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'," + QuotedStr(tbReffNo.Text) + "," + _
                QuotedStr(ddlWrhsArea.SelectedValue) + ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbFgSubledSrc.Text) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ddlWrhsDest.SelectedValue) + ", " + _
                QuotedStr(ddlWrhsAreaDest.SelectedValue) + ", " + QuotedStr(tbFgSubledDest.Text) + ", " + QuotedStr(tbSubledDest.Text) + "," + QuotedStr(tbOperator.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() "
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM stctransferhd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE stctransferMaterialhd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "', WrhsSrc = " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", SrcSubled = " + QuotedStr(tbSubledSrc.Text) + _
                ", FgSrcSubLed = " + QuotedStr(tbFgSubledSrc.Text) + ", WrhsDest = " + QuotedStr(ddlWrhsDest.Text) + _
                ", DestSubLed = " + QuotedStr(tbSubledDest.Text) + ", FgDestSubLed = " + QuotedStr(tbFgSubledDest.Text) + _
                ", TechInCharge = " + QuotedStr(tbOperator.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", WoServiceNo = " + QuotedStr(tbReffNo.Text) + ", " + _
                "DateAppr = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ProductCode, Qty, WrhsCode,LocationSrc, LocationDest, Unit, Remark FROM STCTransferMaterialDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '        "UPDATE STCTransferMaterialDt SET ProductCode = @ProductCode, Qty = @Qty, Unit = @Unit, Remark = @Remark, " + _
            '        " WHERE TransNmbr = '" & ViewState("Reference") & "' AND ProductCode = @ProductCode  ", con)
            '' Define output parameters.
            'Update_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductCode")
            'Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            'Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 15, "Unit")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            '' Define intput (WHERE) parameters.
            'param = Update_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductCode")
            'param.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM STCTransferMaterialDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND ProductCode = @ProductCode ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductCode")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("STCTransferMaterialDt")

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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows

                If CekDt(dr, "ProductCode") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbTransNo.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
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
            tbTransNo.Text = ""
            tbReffNo.Text = ""
            ddlWrhsArea.SelectedIndex = 0
            ddlWrhsSrc.SelectedIndex = 0
            ddlWrhsDest.SelectedIndex = 0
            tbFgSubledSrc.Text = "N"
            tbSubledSrc.Text = ""
            tbSubledSrc.Enabled = tbFgSubledSrc.Text <> "N"
            btnSubledSrc.Visible = tbSubledSrc.Enabled
            tbSubledSrc.Text = ""
            tbFgSubledDest.Text = "N"
            tbSubledDest.Text = ""
            tbSubledDest.Enabled = tbFgSubledDest.Text <> "N"
            BtnSubledDest.Visible = tbSubledDest.Enabled
            tbSubledDest.Text = ""
            tbRemark.Text = ""
            tbOperator.Text = ViewState("UserId")
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProdSrcCode.Text = ""
            tbProdSrcName.Text = ""
            tbUnitSrc.Text = ""
            tbQtySrc.Text = FormatNumber(0, ViewState("DigitQty"))
           
            tbLevelProduct.Text = "0"
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
                If CekDt(dr, "ProductSrc,ProductDest") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"

        FillCombo(ddlLocationSrc, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsSrc.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
        FillCombo(ddlLocationDest, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsDest.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
        ViewState("SetLocation") = False

        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If ddlWrhsArea.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse Area must have value")
                ddlWrhsArea.Focus()
                Return False
            End If

            
            If ddlWrhsSrc.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse Src must have value")
                ddlWrhsSrc.Focus()
                Return False
            End If

            If tbSubledSrc.Text.Trim = "" And tbFgSubledSrc.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed Src must have value")
                tbSubledSrc.Focus()
                Return False
            End If

            If ddlWrhsDest.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse Dest must have value")
                ddlWrhsDest.Focus()
                Return False
            End If

            If tbSubledDest.Text.Trim = "" And tbFgSubledDest.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed Dest must have value")
                tbSubledDest.Focus()
                Return False
            End If

            If ddlWrhsSrc.SelectedValue = ddlWrhsDest.SelectedValue Then
                lbStatus.Text = MessageDlg("Warehouse Src can't be the same as Warehouse Dest !")
                ddlWrhsSrc.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("ProductCode").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Src Must Have Value")
                    Return False
                End If

                If Dr("LocationSrc").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Src Must Have Value")
                    Return False
                End If

                If Dr("LocationDest").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Dest Must Have Value")
                    Return False
                End If
               
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
                
              
            Else

                If tbProdSrcCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Src Must Have Value")
                    tbProdSrcCode.Focus()
                    Return False
                End If
                If ddlLocationSrc.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Location Src Must Have Value")
                    ddlLocationSrc.Focus()
                    Return False
                End If

                If ddlLocationDest.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Location Dest Must Have Value")
                    ddlLocationDest.Focus()
                    Return False
                End If


                If CFloat(tbQtySrc.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Src Must Have Value")
                    tbQtySrc.Focus()
                    Return False
                End If
                If tbUnitSrc.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Src Must Have Value")
                    tbUnitSrc.Focus()
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
            FilterName = "Reference, Date, Status, Warehouse Area, Warehouse Src, Subled Src, Warehouse Dest Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, wrhs_area_Name, Wrhs_Src_Name, Remark"
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
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
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
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
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_STFormTransferMaterial ''" + QuotedStr(GVR.Cells(2).Text) + "''"

                        Session("ReportFile") = ".../../../Rpt/FormSTTransferMaterial.frx"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        'If ViewState("TransferType").ToString = "Location" Or ViewState("TransferType").ToString = "Blokir" Then
                        '    Session("ReportFile") = ".../../../Rpt/FormSttransferMaterialLoc.frx"
                        'Else : Session("ReportFile") = ".../../../Rpt/FormSttransferMaterialQC.frx"
                        'End If

                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String

                    Try
                        paramgo = "TI|" + GVR.Cells(2).Text
                        'lbStatus.Text = paramgo
                        'Exit Sub
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'TI'"

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

    Dim QtySrc As Decimal = 0
    Dim QtyDest As Decimal = 0
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ProductSrc")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
    '                ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
    '                'DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
    '                ''DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                QtySrc = GetTotalSum(ViewState("Dt"), "QtySrc")
    '                QtyDest = GetTotalSum(ViewState("Dt"), "QtyDest")
    '                e.Row.Cells(8).Text = "Total :"
    '                ' for the Footer, display the running totals
    '                e.Row.Cells(5).Text = FormatNumber(QtySrc, ViewState("DigitQty"))
    '                e.Row.Cells(7).Text = FormatNumber(QtyDest, ViewState("DigitQty"))
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr(), dr2() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        'row = ViewState("Dt").Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ProductCode= " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()

        ' ViewState("Dt").AcceptChanges()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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

            FillCombo(ddlLocationSrc, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsDest.SelectedValue), False, "Location_Code", "Location_Name", ViewState("DBConnection"))
            FillCombo(ddlLocationDest, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsDest.SelectedValue), False, "Location_Code", "Location_Name", ViewState("DBConnection"))
            'ViewState("SetLocation") = False

            tbProdSrcCode.Focus()
            'btnGetDt.Enabled = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbReffNo, Dt.Rows(0)("WoserviceNo").ToString)
            BindToDropList(ddlWrhsArea, Dt.Rows(0)("WrhsArea").ToString)
            BindToDropList(ddlWrhsAreaDest, Dt.Rows(0)("WrhsAreaDest").ToString)
            FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' '", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection")) 'And Wrhs_Type <> ''Reject'' 
            FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            BindToDropList(ddlWrhsSrc, Dt.Rows(0)("WrhsSrc").ToString)
            BindToText(tbFgSubledSrc, Dt.Rows(0)("FgSrcSubled").ToString)
            BindToText(tbSubledSrc, Dt.Rows(0)("SrcSubled").ToString)
            BindToText(tbSubledSrcName, Dt.Rows(0)("Subled_Src_Name").ToString)
            BindToDropList(ddlWrhsDest, Dt.Rows(0)("WrhsDest").ToString)
            BindToText(tbSubledDest, Dt.Rows(0)("DestSubled").ToString)
            BindToText(tbFgSubledDest, Dt.Rows(0)("FgDestSubled").ToString)
            BindToText(tbSubledDestName, Dt.Rows(0)("Subled_Dest_Name").ToString)
            BindToText(tbOperator, Dt.Rows(0)("TechInCharge").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ProductCode = " + QuotedStr(ItemNo))
            'lbStatus.Text = Dr(0)("LocationSrc").ToString
            If Dr.Length > 0 Then
                BindToText(tbProdSrcCode, Dr(0)("ProductCode").ToString)
                GetInfo(tbProdSrcCode.Text)
                BindToText(tbProdSrcName, Dr(0)("ProductName").ToString)
                BindToText(tbUnitSrc, Dr(0)("Unit").ToString)
                BindToText(tbQtySrc, Dr(0)("Qty").ToString)
                BindToDropList(ddlProductWrhsSrc, Dr(0)("WrhsCode").ToString)
                BindToDropList(ddlLocationSrc, Dr(0)("LocationSrc").ToString)
                BindToDropList(ddlLocationDest, Dr(0)("LocationDest").ToString)
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbProdSrcCode.Text Then
                    If CekExistData(ViewState("Dt"), "ProductCode", tbProdSrcCode.Text) Then
                        lbStatus.Text = "Product Code '" + tbProdSrcName.Text + "'  has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ProductCode = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ProductCode") = tbProdSrcCode.Text
                Row("ProductName") = tbProdSrcName.Text
                Row("Unit") = tbUnitSrc.Text
                Row("Qty") = tbQtySrc.Text
                Row("WrhsCode") = ddlProductWrhsSrc.Text
                Row("LocationSrc") = ddlLocationSrc.Text
                Row("LocationDest") = ddlLocationDest.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ProductCode", tbProdSrcCode.Text) Then
                    lbStatus.Text = "Product Src '" + tbProdSrcName.Text + "'  has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ProductCode") = tbProdSrcCode.Text
                dr("ProductName") = tbProdSrcName.Text
                dr("Unit") = tbUnitSrc.Text
                dr("Qty") = tbQtySrc.Text
                dr("WrhsCode") = ddlProductWrhsSrc.Text
                dr("LocationSrc") = ddlLocationSrc.Text
                dr("LocationDest") = ddlLocationDest.Text
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
            PnlInfo.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProdSrcCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProdSrcCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit, LevelProduct  FROM VMsProduct WHERE Product_Code = " + QuotedStr(tbProdSrcCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProdSrcCode.Text = Dr("Product_Code")
                tbProdSrcName.Text = Dr("Product_Name") + " " + TrimStr(Dr("Specification").ToString)
                tbUnitSrc.Text = Dr("Unit")
                If ViewState("StrKode") = "TRG" Then
                    tbLevelProduct.Text = Dr("LevelProduct").ToString
                End If
                GetInfo(tbProdSrcCode.Text)
            Else
                tbProdSrcCode.Text = ""
                tbProdSrcName.Text = ""
                tbUnitSrc.Text = ""
               
            End If
           
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub

   

    Protected Sub ddlWrhsSrc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsSrc.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Warehouse", QuotedStr(ddlWrhsSrc.SelectedValue), ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubledSrc.Text = Dr("FgSubLed")
                tbSubledSrc.Text = ""
                tbSubledSrcName.Text = ""
            Else
                tbFgSubledSrc.Text = "N"
                tbSubledSrc.Text = ""
                tbSubledSrcName.Text = ""
            End If
            tbSubledSrc.Enabled = tbFgSubledSrc.Text <> "N"
            btnSubledSrc.Visible = tbSubledSrc.Enabled
            ddlWrhsSrc.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlWrhsDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsDest.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Warehouse", QuotedStr(ddlWrhsDest.SelectedValue), ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubledDest.Text = Dr("FgSubLed")
                tbSubledDest.Text = ""
                tbSubledDestName.Text = ""
            Else
                tbFgSubledDest.Text = "N"
                tbSubledDest.Text = ""
                tbSubledDestName.Text = ""
            End If
            'CekHd()
            tbSubledDest.Enabled = tbFgSubledDest.Text <> "N"
            BtnSubledDest.Visible = tbSubledDest.Enabled
            ddlWrhsDest.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSubledSrc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubledSrc.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubledSrc.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLedSrc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSubledDest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSubledDest.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubledDest.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLedDest"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSubledSrc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubledSrc.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SPName As String
        Try
            Try
                SPName = "EXEC S_FindSubled " + QuotedStr(tbFgSubledSrc.Text) + ", " + QuotedStr(tbSubledSrc.Text)
                DT = SQLExecuteQuery(SPName, ViewState("DBConnection").ToString).Tables(0)
                If DT.Rows.Count > 0 Then
                    Dr = DT.Rows(0)
                Else
                    Dr = Nothing
                End If
            Catch ex As Exception
                Throw New Exception("EXEC S_FindSubled Error : " + ex.ToString)
            End Try
            If Not Dr Is Nothing Then
                tbSubledSrc.Text = Dr("Subled_No")
                tbSubledSrcName.Text = Dr("Subled_Name")
            Else
                tbSubledSrc.Text = ""
                tbSubledSrcName.Text = ""
            End If
            tbSubledSrc.Focus()
        Catch ex As Exception
            Throw New Exception("tbSubledSrc_TextChanged : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbSubledDest_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubledDest.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SPName As String
        Try
            Try
                SPName = "EXEC S_FindSubled " + QuotedStr(tbFgSubledDest.Text) + ", " + QuotedStr(tbSubledDest.Text)
                DT = SQLExecuteQuery(SPName, ViewState("DBConnection").ToString).Tables(0)
                If DT.Rows.Count > 0 Then
                    Dr = DT.Rows(0)
                Else
                    Dr = Nothing
                End If
            Catch ex As Exception
                Throw New Exception("EXEC S_FindSubled Error : " + ex.ToString)
            End Try
            If Not Dr Is Nothing Then
                tbSubledDest.Text = Dr("Subled_No")
                tbSubledDestName.Text = Dr("Subled_Name")
            Else
                tbSubledDest.Text = ""
                tbSubledDestName.Text = ""
            End If
            tbSubledDest.Focus()
        Catch ex As Exception
            Throw New Exception("tbSubledDest_TextChanged : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnProdSrc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProdSrc.Click
        Dim ResultField, CriteriaField As String
        Try
            'CriteriaField = "Product_Code, Product_Name, Specification, Unit, LevelProduct"
            'Session("Filter") = "EXEC S_SttransferMaterialReffNR '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
            'ResultField = "Product_Code, Product_Name, Unit, Specification, LevelProduct"

            Session("Filter") = "SELECT * FROM V_GetProductStock WHERE Warehouse = " + QuotedStr(ddlWrhsSrc.SelectedValue)

            ResultField = "Product_Code, Product_Name, Unit, Qty, Specification, LevelProduct"
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, Qty, LevelProduct "
            Session("CriteriaField") = CriteriaField.Split(",")

            ViewState("Sender") = "btnProdSrc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd2.Click, BtnAdd.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
            PnlInfo.Visible = False
            btnReffNo.Visible = True
            ' ddlReffType.Enabled = Request.QueryString("ContainerId").ToString = "TransLocationID" 'Or Request.QueryString("ContainerId").ToString = "TransLocationFGID"
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlWrhsArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsArea.SelectedIndexChanged

        FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection")) 'And Wrhs_Type <> ''Reject''
        FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
    End Sub


    Protected Sub ddlWrhsAreaDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsAreaDest.SelectedIndexChanged

        'FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsAreaDest.SelectedValue) + ", ''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection")) 'And Wrhs_Type <> ''Reject''
        'FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsAreaDest.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
    End Sub

    Protected Sub tbQtySrc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtySrc.TextChanged
        Try
            tbQtySrc.Text = FormatNumber(tbQtySrc.Text, ViewState("DigitQty"))
            'tbQtyDest.Text = FormatNumber(tbQtySrc.Text, ViewState("DigitQty"))
            tbRemarkDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try

    End Sub

    Private Sub GetInfo(ByVal Product As String)
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_GetInfoStock " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(Product)

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PnlInfo.Visible = True
            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnReffNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReffNo.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT DISTINCT TransNmbr, Wrhs_Area, Warehouse, WarehouseName, FgSubled, Subled, Subled_Name, Operator, Remark, OpnameNo from V_SttransferMaterialGetProduct " 'WHERE ReffType = " + QuotedStr(ddlReffType.SelectedValue)
    '        ResultField = "TransNmbr, Wrhs_Area, Warehouse, FgSubled, Subled, Subled_Name, Remark"
    '        ViewState("Sender") = "btnReffNo"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Reff No Error : " + ex.ToString
    '    End Try
    'End Sub


    Protected Sub btnReffNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReffNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TransNmbr, TransDate, Status, ServiceReqNo, AreaCode, KavlingCode, RequestBy FROM MTNWOServiceHd WHERE Status = 'P'"
            ResultField = "TransNmbr"
            ViewState("Sender") = "btnReffNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Reff No Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            'If tbReffNo.Text <> "" Then
            '    CriteriaField = "Product_Code, Product_Name, Specification, Unit, Qty " '
            '    Session("Filter") = "EXEC S_SttransferMaterialReff " + QuotedStr(tbReffNo.Text) + ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledDest.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
            '    ResultField = "Product_Code, Product_Name, Specification, Unit, Qty "
            '    Session("CriteriaField") = CriteriaField.Split(",")
            'Else
            '    Session("Filter") = "EXEC S_SttransferMaterialReffNR '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
            '    ResultField = "Product_Code, Product_Name, Unit, Specification, LevelProduct"
            '    CriteriaField = "Product_Code, Product_Name, Specification, Unit, Qty, LevelProduct "
            'End If

            Session("Filter") = "SELECT * FROM V_GetProductStock WHERE Warehouse = " + QuotedStr(ddlWrhsSrc.SelectedValue)
            ResultField = "Product_Code, Product_Name, Unit, Qty,Specification, LevelProduct"
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, Qty, LevelProduct "

            Session("CriteriaField") = CriteriaField.Split(",")

            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetLHP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetLHP.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If tbReffNo.Text = "" Then
                CriteriaField = "Product, Product_Name, Unit, Qty" '

                Session("filter") = "SELECT * From V_SttransferMaterialGetLHPLot Where DoneTT = 'N' And Warehouse = " + QuotedStr(ddlWrhsSrc.SelectedValue)
                Session("CriteriaField") = CriteriaField.Split(",")
            End If

            ResultField = "Product, Product_Name, RollNo, Qty, Unit, Warehouse"

            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetLHP"

            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
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

    

End Class
