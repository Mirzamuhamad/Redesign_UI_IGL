Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
Imports System.Xml
Imports System.Data.OleDb

Partial Class Transaction_TrSTOpname_TrSTOpname
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "SELECT DISTINCT Reference, Nmbr, TransDate, Status, positionDate, Warehouse_Code, Warehouse_Name, FgSubLed, SubLed_Code, SubLed_Name, Operator, Remark  From V_STOpnameHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()

                FillCombo(ddlWrhs, "EXEC S_GetWrhsUser " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                'FillCombo(ddlWrhs, "EXEC S_GetWarehouse", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))

                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnproduct" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                    tbSpecification.Text = Session("Result")(2).ToString
                    tbUnit.Text = Session("Result")(3).ToString
                    tbQtyOpname.Text = Session("Result")(4).ToString
                    tbQtyActual.Text = Session("Result")(4).ToString
                    tbQtyOpname.Text = FormatFloat(0, ViewState("DigitQty"))
                  
                End If
                If ViewState("Sender") = "btnSubLed" Then
                    tbSubled.Text = Session("Result")(0).ToString
                    tbSubledName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Specification") = drResult("Specification")
                            dr("Unit") = drResult("Unit")
                            dr("QtySystem") = drResult("On_Hand")
                            dr("QtyActual") = drResult("On_Hand")
                            dr("QtyOpname") = FormatFloat(0, ViewState("DigitQty"))
                            dr("Remark") = ""

                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    WriteToXML(ViewState("UserId").ToString + Format(tbPositionDate.SelectedValue, "yyyyMMdd") + ".xml") '+ ViewState("OpnameType").ToString
                    'BindGridDt(ViewState("Dt"), GridDt)
                    BindGridDtView()
                    EnableHd(Not DtExist())
                    GridDt.Columns(1).Visible = True
                    'Session("ResultSame") = Nothing
                End If
                If ViewState("Sender") = "btnGetPre" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Specification") = drResult("Specification")
                            dr("Unit") = drResult("Unit")
                            dr("QtySystem") = drResult("On_Hand")
                            dr("QtyActual") = drResult("Qty")
                            dr("QtyOpname") = FormatFloat(CFloat(drResult("Qty")) - CFloat(drResult("On_Hand")), ViewState("DigitQty"))
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    WriteToXML(ViewState("UserId").ToString + Format(tbPositionDate.SelectedValue, "yyyyMMdd") + ViewState("OpnameType").ToString + ".xml")
                    'BindGridDt(ViewState("Dt"), GridDt)
                    BindGridDtView()
                    EnableHd(Not DtExist())
                    GridDt.Columns(1).Visible = True
                    'Session("ResultSame") = Nothing
                End If
                If ViewState("Sender") = "btnGetDtNon" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Specification") = drResult("Specification")
                            dr("Unit") = drResult("Unit")
                            dr("QtySystem") = FormatFloat(0, ViewState("DigitQty"))
                            dr("QtyActual") = FormatFloat(0, ViewState("DigitQty"))
                            dr("QtyOpname") = FormatFloat(0, ViewState("DigitQty"))
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If

                    Next
                    WriteToXML(ViewState("UserId").ToString + Format(tbPositionDate.SelectedValue, "yyyyMMdd") + ".xml") '+ ViewState("OpnameType").ToString 
                    'BindGridDt(ViewState("Dt"), GridDt)
                    BindGridDtView()
                    EnableHd(Not DtExist())
                    GridDt.Columns(1).Visible = True
                    'Session("ResultSame") = Nothing
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
        ViewState("SortExpressionDt") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        'If Request.QueryString("ContainerId").ToString = "STOpnameFGID" Then
        'ViewState("OpnameType") = "FG"
        'lblTitle.Text = "Stock Opname Finish Good"
        'Else
        'ViewState("OpnameType") = "NFG"
        'lblTitle.Text = "Stock Opname Material"
        'End If
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        GridDt.PageSize = CInt(ViewState("PageSizeGrid"))
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        tbQtyOpname.Attributes.Add("ReadOnly", "True")
        tbQtyOpname.Attributes.Add("ReadOnly", "True")
        Me.tbQtyActual.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbQtyOpname.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQtyOpname.Attributes.Add("OnBlur", "setformat();")
        Me.tbQtyActual.Attributes.Add("OnBlur", "setformat();")
        Me.tbQtyOpname.Attributes.Add("OnBlur", "setformat();")
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
        Return "SELECT * From V_STOpnameDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
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
                Session("SelectCommand") = "EXEC S_STFormOpname " + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormOpname.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_STOpname", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("Reference in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            '            btnGetDt.Visible = ((ViewState("StateHd") = "Insert") Or (ViewState("StateHd") = "Edit") Or ddlWrhs.Enabled) 'And (ViewState("OpnameType") = "NFG")
            'btnGetDtNon.Visible = (ViewState("StateHd") = "Insert") Or (ViewState("StateHd") = "Edit") Or ddlWrhs.Enabled
            tbPositionDate.Enabled = State
            ddlWrhs.Enabled = State
            tbSubled.Enabled = State And tbFgSubled.Text.Trim <> "N"
            btnSubled.Visible = tbSubled.Enabled
          
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
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
            PnlInfo.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"
            'Panel1.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable

            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDtView()
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
                'If ViewState("OpnameType").ToString = "FG" Then
                'tbRef.Text = GetAutoNmbr("SOPF", "N", CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ddlWrhs.SelectedValue, ViewState("DBConnection").ToString)
                'Else
                tbRef.Text = GetAutoNmbr("SOP", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlWrhs.SelectedValue, ViewState("DBConnection").ToString)
                'End If

                SQLString = "INSERT INTO STCOpnameHd (TransNmbr, Status, TransDate, PositionDate, Warehouse, Subled, FgSubled, Operator, Remark,  UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text) + ", " + QuotedStr(tbFgSubled.Text) + "," + QuotedStr(tbOperator.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM STCOpnameHD WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE STCOpnameHD SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', PositionDate = '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + _
                "', Warehouse = " + QuotedStr(ddlWrhs.SelectedValue) + ", Subled = " + QuotedStr(tbSubled.Text) + _
                ", FgSubled = " + QuotedStr(tbFgSubled.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + "," + _
                " DateAppr = getDate()" + _
                " WHERE TransNmbr = '" + tbRef.Text + "'"
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

            WriteToXML(ViewState("UserId").ToString + Format(tbPositionDate.SelectedValue, "yyyyMMdd") + ".xml")

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, QtySystem, QtyActual, QtyOpname, Unit, Remark FROM STCOpnameDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE STCOpnameDt SET Product = @Product, " + _
            "QtySystem = @QtySystem, QtyActual = @QtyActual, QtyOpname = @QtyOpname, " + _
             "Unit = @Unit, Remark = @Remark " + _
            "WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @OldProduct ", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@QtySystem", SqlDbType.Float, 18, "QtySystem")
            Update_Command.Parameters.Add("@QtyActual", SqlDbType.Float, 18, "QtyActual")
            Update_Command.Parameters.Add("@QtyOpname", SqlDbType.Float, 18, "QtyOpname")
           
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command


            Dim Delete_Command = New SqlCommand( _
            "DELETE FROM STCOpnameDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("STCOpnameDt")
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
                If CekDt(dr, "Product") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "Reference"
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
            GridDt.Columns(1).Visible = False
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
            tbPositionDate.SelectedDate = ViewState("ServerDate") 'Today
            tbOperator.Text = ViewState("UserId").ToString
            'btnGetDt.Visible = ViewState("OpnameType") = "NFG"
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            ddlWrhs.SelectedIndex = 0
            tbFgSubled.Text = "N"
            tbSubled.Text = ""
            tbSubled.Enabled = tbFgSubled.Text <> "N"
            btnSubled.Visible = tbSubled.Enabled
            tbSubledName.Text = ""
            tbOperator.Text = ""
            tbRemark.Text = ""
           Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            tbSpecification.Text = ""
            tbUnit.Text = ""
            tbRemarkDt.Text = ""
            tbQtyOpname.Text = FormatFloat(0, ViewState("DigitQty"))
            tbQtyActual.Text = FormatFloat(0, ViewState("DigitQty"))
            tbQtySystem.Text = FormatFloat(0, ViewState("DigitQty"))
          
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
                If CekDt(dr, "Product") = False Then
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
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbPositionDate.SelectedDate < tbDate.SelectedDate Then
                lbStatus.Text = MessageDlg("position Date must be greater than " + tbDate.SelectedDate.ToString)
                tbPositionDate.Focus()
                Return False
            End If
            If ddlWrhs.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlWrhs.Focus()
                Return False
            End If
            If tbSubled.Text.Trim = "" And tbFgSubled.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed must have value")
                tbSubled.Focus()
                Return False
            End If
            If tbOperator.Text = "" Then
                lbStatus.Text = MessageDlg("Operator must have value")
                tbOperator.Focus()
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
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyActual").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Qty Actual Must Have Value")
                    Return False
                End If
            Else
                If tbCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbCode.Focus()
                    Return False
                End If
                If CFloat(tbQtyActual.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Qty Actual Must Have Value")
                    tbQtyActual.Focus()
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
            FilterName = "Reference,  Date, Status, Position Date, Warehouse, Subled, Remark"
            FilterValue = "Reference, dbo.FormatDate(TransDate), Status, dbo.FormatDate(PositionDate), Warehouse_Name, Subled_Name, Remark"
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    GridDt.Columns(1).Visible = False
                    btnHome.Visible = True
                    'Panel2.Visible = False
                    PnlInfo.Visible = False
                    'Panel1.Visible = False

                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        GridDt.Columns(1).Visible = True
                        btnHome.Visible = False
                        EnableHd(Not DtExist())
                        'Panel2.Visible = True
                        PnlInfo.Visible = True
                        'Panel1.Visible = True


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
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_STFormOpname ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormOpname.frx"
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
            BindGridDtView()
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
            Dim lb As Label
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            'Dim DV As DataView

            dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDtView()
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)

            ViewState("DtValue") = GVR.Cells(2).Text

             FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"

            tbCode.Focus()
            'btnGetDt.Enabled = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbPositionDate, Dt.Rows(0)("PositionDate").ToString)
            BindToDropList(ddlWrhs, Dt.Rows(0)("Warehouse_Code").ToString)
            BindToText(tbSubled, Dt.Rows(0)("Subled_Code").ToString)
            BindToText(tbSubledName, Dt.Rows(0)("Subled_Name").ToString)
            BindToText(tbFgSubled, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbCode, Dr(0)("Product").ToString)
                BindToText(tbName, Dr(0)("Product_Name").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
               BindToText(tbQtySystem, Dr(0)("QtySystem").ToString)
                BindToText(tbQtyActual, Dr(0)("QtyActual").ToString)
                BindToText(tbQtyOpname, Dr(0)("QtyOpname").ToString)
                BindToText(tbRemarkDt, TrimStr(Dr(0)("Remark").ToString))
               
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

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField, DefaultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Sub_Group, Product_Type "
            Session("Filter") = "EXEC S_STOpnameReff '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
            ResultField = "Product_Code, Product_Name, specification, Unit, On_Hand"
            Session("CriteriaField") = CriteriaField.Split(",")
            DefaultField = "Product_Name"
            Session("ColumnDefault") = DefaultField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbCode.Text Then
                    If CekExistData(ViewState("Dt"), "Product", tbCode.Text) Then
                        lbStatus.Text = "Product '" + tbName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbCode.Text
                Row("Product_Name") = tbName.Text
                Row("specification") = tbSpecification.Text
               Row("Remark") = TrimStr(tbRemarkDt.Text)
                Row("unit") = tbUnit.Text
                Row("QtySystem") = FormatFloat(CFloat(tbQtySystem.Text), ViewState("DigitQty"))
                Row("QtyActual") = FormatFloat(CFloat(tbQtyActual.Text), ViewState("DigitQty"))
                Row("QtyOpname") = FormatFloat(CFloat(tbQtyOpname.Text), ViewState("DigitQty"))
               Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Product", tbCode.Text) Then
                    lbStatus.Text = "Product '" + tbName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbCode.Text
                dr("Product_Name") = tbName.Text
                dr("specification") = tbSpecification.Text
                dr("Remark") = TrimStr(tbRemarkDt.Text)
                dr("unit") = tbUnit.Text
                dr("QtySystem") = FormatFloat(CFloat(tbQtyOpname.Text), ViewState("DigitQty"))
                dr("QtyActual") = FormatFloat(CFloat(tbQtyActual.Text), ViewState("DigitQty"))
                dr("QtyOpname") = FormatFloat(CFloat(tbQtyOpname.Text), ViewState("DigitQty"))
                ViewState("Dt").Rows.Add(dr)
            End If
            WriteToXML(ViewState("UserId").ToString + Format(tbPositionDate.SelectedValue, "yyyyMMdd") + ".xml")
            'lbStatus.Text = ConvertDataTableToXML(ViewState("Dt"), "test")
            'Exit Sub

            MovePanel(pnlEditDt, pnlDt)
            EnableHd(Not DtExist())
            BindGridDtView()
            StatusButtonSave(True)
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
            EnableHd(Not DtExist())

            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btn1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn1.Click
        Dim ResultField, CriteriaField, DefaultField As String
        Try
            'If ViewState("OpnameType") = "FG" Then
            'CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Merk" 'Product_Class, Product_Size, Product_Grade, Product_Motif
            'Else
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Sub_Group, Product_Type "
            'End If
            Session("Filter") = "EXEC S_STOpnameReff '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ""

            ResultField = "Product_Code, Product_Name, Specification, Unit, On_Hand"
            Session("CriteriaField") = CriteriaField.Split(",")
            DefaultField = "Product_Name"
            Session("ColumnDefault") = DefaultField.Split(",")
            ViewState("Sender") = "btnproduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Error : " + ex.ToString
        End Try
    End Sub

    Private Function GetQtySystem(ByVal tgl As DateTime, ByVal wrhs As String, ByVal subled As String, ByVal product As String, ByVal location As String) As Double
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_STOpnameGetQtySystem " + QuotedStr(wrhs) + ", " + QuotedStr(subled) + " , " + QuotedStr(product) + ", '" + Format(tgl, "yyyyMMdd") + "'", ViewState("DBConnection").ToString)
        dr.Read()
        Return dr("QtySystem")
    End Function

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim QSystem As Double
        Try
            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Product_Code = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString).Tables(0) 'AND UserId = '" + ViewState("UserId") + "'
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCode.Text = Dr("Product_Code")
                tbName.Text = Dr("Product_Name")
                tbSpecification.Text = TrimStr(Dr("Specification").ToString)
                tbUnit.Text = Dr("Unit")
                QSystem = GetQtySystem(Format(tbPositionDate.SelectedValue, "yyyy-MM-dd"), ddlWrhs.SelectedValue, tbSubled.Text, tbCode.Text, "")
                tbQtySystem.Text = FormatFloat(QSystem, ViewState("DigitQty"))
                tbQtyActual.Text = FormatFloat(QSystem, ViewState("DigitQty"))
                tbQtyOpname.Text = FormatFloat(0, ViewState("DigitQty"))

                tbQtyActual_TextChanged(Nothing, Nothing)
            Else
                tbCode.Text = ""
                tbName.Text = ""
                tbSpecification.Text = ""
                tbUnit.Text = ""
           End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbCode_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            ViewState("InputProduct") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlWrhs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhs.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Warehouse", ddlWrhs.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                'tbFgSubled.Text = Dr("FgSubLed")
                tbSubled.Text = ""
                tbSubledName.Text = ""
            Else
                'tbFgSubled.Text = "N"
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If
            tbSubled.Enabled = tbFgSubled.Text <> "N"
            'btnSubled.Visible = tbSubled.Enabled
            btnSubled.Visible = True

            ddlWrhs.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed " 'WHERE FgSubLed = " + QuotedStr(tbFgSubled.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLed"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyActual_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyActual.TextChanged
        Try

            tbQtyOpname.Text = FormatFloat(CFloat(tbQtyActual.Text) - CFloat(tbQtySystem.Text), ViewState("DigitQty"))
            tbRemarkDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb Qty Actual Error : " + ex.ToString
        End Try
    End Sub



    Private Sub bindDataGetZero()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim Dt As DataTable
            Dim lb As Label
            Dim Pertamax As Boolean
            Dim ListSelectProd, ListSelectProdLoc, Result1, Result2 As String
            Pertamax = True
            Result1 = ""
            Result2 = ""

            For Each GVR In GridDt.Rows
                CB = GVR.FindControl("cbSelect")
                If CB.Checked Then
                    ListSelectProdLoc = GVR.Cells(2).Text
                    ListSelectProd = GVR.Cells(2).Text

                    If Pertamax Then
                        Result1 = "'''" + ListSelectProd + "''"
                        Result2 = "'''" + ListSelectProdLoc + "''"

                        Pertamax = False
                    Else
                        Result1 = Result1 + ",''" + ListSelectProd + "''"
                        Result2 = Result2 + ",''" + ListSelectProdLoc + "''"
                    End If

                End If
            Next
            If Result1 = "" And Result2 = "" Then
                lbStatus.Text = MessageDlg("Please Check Product for Process")
                Exit Sub
            Else
                lbStatus.Text = MessageDlg("Set Qty Actual = 0 Success for Selected Product")
            End If
            Result1 = Result1 + "'"
            Result2 = Result2 + "'"
            Dt = SQLExecuteQuery("EXEC S_STOpnameForLocation '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ViewState("OpnameType").ToString) + ", " + Result1 + "," + Result2, ViewState("DBConnection")).Tables(0)
            Dim drResult1 As DataRow
            For Each drResult1 In Dt.Rows
                'insert
                If CekExistData(ViewState("Dt"), "Product", drResult1("Product_Code")) = False Then
                    Dim dr As DataRow
                    dr = ViewState("Dt").NewRow
                    dr("Product") = drResult1("Product_Code")
                    dr("Product_Name") = drResult1("Product_Name")
                    dr("Specification") = drResult1("Specification")
                    dr("Unit") = drResult1("Unit")
                    dr("QtySystem") = drResult1("QtySystem")
                    dr("QtyActual") = drResult1("QtyActual")
                    dr("QtyOpname") = FormatFloat(drResult1("QtyOpname"), ViewState("DigitQty"))
                    ViewState("Dt").Rows.Add(dr)
                End If
            Next
            'BindGridDt(ViewState("Dt"), GridDt)
            BindGridDtView()
            EnableHd(Not DtExist())
        Catch ex As Exception
            Throw New Exception("bindDataGetZero Error : " + ex.ToString)
        End Try
    End Sub


    'Protected Sub btnGetSetZero_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetSetZero.Click
    '    Try
    '        bindDataGetZero()
    '    Catch ex As Exception
    '        Throw New Exception("btnGetSetZero_Click Error : " + ex.ToString)
    '    End Try

    'End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    '   btnGetSetZero.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub

    Dim TQtySystem As Decimal = 0
    Dim TQtyActual As Decimal = 0
    Dim TQtyOpname As Decimal = 0
    'Dim Adjust As Decimal = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    'TQtySystem += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtySystem"))
                    'TQtyActual += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyActual"))
                    'TQtyOpname += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyOpname"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    TQtySystem = GetTotalSum(ViewState("Dt"), "QtySystem")
                    TQtyActual = GetTotalSum(ViewState("Dt"), "QtyActual")
                    TQtyOpname = GetTotalSum(ViewState("Dt"), "QtyOpname")
                    e.Row.Cells(4).Text = "Total : "
                    e.Row.Cells(5).Text = FormatFloat(TQtySystem, ViewState("DigitQty"))
                    e.Row.Cells(6).Text = FormatFloat(TQtyActual, ViewState("DigitQty"))
                    e.Row.Cells(7).Text = FormatFloat(TQtyOpname, ViewState("DigitQty"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub WriteToXML(ByVal iFileName As String)
        Try
            Dim ds As New DataSet
            ds.DataSetName = "DS"
            Dim dt As DataTable
            dt = ViewState("Dt")
            ds.Tables.Add(dt)
            ds.Tables(0).TableName = "Opname"
            ds.WriteXml(Server.MapPath(iFileName))
        Catch Ex As Exception
            lbStatus.Text = "Write XML error: " & Ex.Message
        End Try
    End Sub

    Protected Sub ReadToXML(ByVal iFileName As String)
        Dim ds As New DataSet
        Dim dt As DataTable
        Try

            ds.DataSetName = "DS"
           If System.IO.File.Exists(Server.MapPath(iFileName)) = False Then
                lbStatus.Text = Server.MapPath(iFileName) + " File does not found"
                Exit Sub
            End If
            ds.ReadXml(Server.MapPath(iFileName))
            If ds Is Nothing Then
                lbStatus.Text = "Dataset not exists"
                Exit Sub
            End If
            dt = ds.Tables(0)
            'insert into Dt
            Dim drResult As DataRow
            For Each drResult In dt.Rows
                'insert
                If CekExistData(ViewState("Dt"), "Product", drResult("Product")) = False Then
                    Dim dr As DataRow
                    dr = ViewState("Dt").NewRow
                    dr("Product") = drResult("Product")
                    dr("Product_Name") = drResult("Product_Name")
                    dr("Specification") = drResult("Specification")
                    dr("Unit") = drResult("Unit")
                    dr("Remark") = drResult("Remark")
                    dr("QtySystem") = drResult("QtySystem")
                    dr("QtyActual") = drResult("QtyActual")
                    dr("QtyOpname") = drResult("QtyOpname")
                    ViewState("Dt").Rows.Add(dr)
                End If
            Next
            BindGridDtView()
            EnableHd(Not DtExist())
            GridDt.Columns(1).Visible = True
        Catch Ex As Exception
            lbStatus.Text = "Read XML error: " & Ex.Message
        End Try
    End Sub


    Protected Sub btnXML_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnXML.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            ReadToXML(ViewState("UserId").ToString + Format(tbPositionDate.SelectedValue, "yyyyMMdd") + ".xml")
            WriteToXML(ViewState("UserId").ToString + Format(tbPositionDate.SelectedValue, "yyyyMMdd") + ".xml")
        Catch Ex As Exception
            lbStatus.Text = "btn XML Click error: " & Ex.Message
        End Try
    End Sub

    Protected Sub GridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridDt.Sorting
        Try
            If ViewState("SortOrderDt") = Nothing Or ViewState("SortOrderDt") = "DESC" Then
                ViewState("SortOrderDt") = "ASC"
            Else
                ViewState("SortOrderDt") = "DESC"
            End If
            ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrderDt")
            BindGridDtView()
            'BindDataDt(ViewState("Reference"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try
            tbPositionDate.SelectedValue = tbDate.SelectedValue
        Catch ex As Exception
            lbStatus.Text = "tbDate SelectionChange Error : " + ex.ToString
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

            If ViewState("DtPR") Is Nothing Then
                piar = False
            Else
                piar = ViewState("DtPR").Rows.Count > 0
            End If

            Return (dete Or piar)


        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnGetDtNon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDtNon.Click
        Dim ResultField, CriteriaField, DefaultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            CriteriaField = "Product_Code, Product_Name, Specification, Unit "
            ResultField = "Product_Code, Product_Name, specification, Unit"
            'Session("Filter") = "EXEC S_STOpnameReffNon '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + ViewState("OpnameType").ToString
            Session("Filter") = "EXEC S_STOpnameReff '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
           
            Session("CriteriaField") = CriteriaField.Split(",")
            DefaultField = "Product_Name"
            Session("ColumnDefault") = DefaultField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDtNon"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataDeleteAll()
        Try
            Dim GVR As GridViewRow
            Dim Product As String
            Dim lb As Label
            Dim Row As DataRow
            If GridDt.Rows.Count = 0 Then
                lbStatus.Text = "No Product Select"
                Exit Sub
            Else
                For Each GVR In GridDt.Rows
                    Product = GVR.Cells(2).Text
                    Row = ViewState("Dt").Select("Product = " + QuotedStr(Product))(0)
                    Row.Delete()
                Next
                lbStatus.Text = "Delete All Success for Product"
                GridDt.Columns(0).Visible = False
            End If

        Catch ex As Exception
            Throw New Exception("bindDatadelete All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnProcessDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProcessDel.Click
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lb As Label
            Dim Row As DataRow
            Dim Product As String
            For Each GVR In GridDt.Rows
                CB = GVR.FindControl("cbSelect")

                If CB.Checked Then
                    Product = GVR.Cells(2).Text
                    Row = ViewState("Dt").Select("Product = " + QuotedStr(Product))(0)
                    Row.Delete()
                End If
            Next
            EnableHd(Not DtExist())
            lbStatus.Text = "Delete Selected Data Success"
            'bindDataDeleteAll()
            BindGridDtView()
        Catch ex As Exception
            Throw New Exception("btnProcesDel_Click Error : " + ex.ToString)
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
    Private Sub GetExcelSheets(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03 
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                Exit Select
            Case ".xlsx"
                'Excel 07 
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select

        'Get the Sheets in Excel WorkBoo 
        conStr = String.Format(conStr, FilePath, isHDR)
        'lbStatus.Text = conStr
        'Exit Sub
        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        cmdExcel.Connection = connExcel
        If connExcel.State = ConnectionState.Closed Then
            connExcel.Open()
        End If
        'Bind the Sheets to DropDownList 
        ddlSheets.Items.Clear()
        'ddlSheets.Items.Add(New ListItem("--Select Sheet--", ""))
        ddlSheets.DataSource = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        ddlSheets.DataTextField = "TABLE_NAME"
        ddlSheets.DataValueField = "TABLE_NAME"
        ddlSheets.DataBind()
        connExcel.Close()

        ddlSheets.SelectedIndex = 0
        DataExcel(ddlSheets.SelectedValue, FilePath, Extension, isHDR)
          End Sub
    Private Sub DataExcel(ByVal sheet As String, ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Try
            Dim conStr As String = ""
            Select Case Extension
                Case ".xls"
                    'Excel 97-03 
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                    Exit Select
                Case ".xlsx"
                    'Excel 07 
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                    Exit Select
            End Select

            'Get the Sheets in Excel WorkBoo 
            conStr = String.Format(conStr, FilePath, isHDR)
            Dim query As String = "SELECT * FROM [" + sheet + "]"
            Dim conn As New OleDbConnection(conStr)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim cmd As New OleDbCommand(query, conn)
            Dim da As New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            GridExcel.DataSource = ds.Tables(0)
            GridExcel.DataBind()

            Dim count As Integer
            count = ds.Tables(0).Columns.Count - 1
            ddlFindProductCode.Items.Clear()
            ddlFindProductCode.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindQtyOpname.Items.Clear()
            ddlFindQtyOpname.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindRemark.Items.Clear()
            ddlFindRemark.Items.Add(New ListItem("--Choose One--", ""))
           For j = 0 To count
                ddlFindProductCode.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindQtyOpname.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindRemark.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
           
            Next
            da.Dispose()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            lbStatus.Text = "DataExcel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUploadExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadExcel.Click
        If fileuploadExcel.HasFile Then
            Dim FileName As String = Path.GetFileName(fileuploadExcel.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(fileuploadExcel.PostedFile.FileName)
            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

            Dim FilePath As String = Server.MapPath("~/ExcelHSE/" + FileName)
            fileuploadExcel.SaveAs(FilePath)
            ViewState("FilePath") = FilePath
            ViewState("Extension") = Extension
            GetExcelSheets(FilePath, Extension, "Yes")
        End If
    End Sub
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try
            ' call sp S_STOpnameTempDelete
            SQLExecuteNonQuery("EXEC S_STOpnameTempDelete " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text.Trim), ViewState("DBConnection").ToString)
            importtoTemp()
            importsave()

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(Not DtExist())
            GridDt.Columns(1).Visible = True
            PnlInfo.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"

            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            'BindDataDt(tbCode.Text)
        Catch ex As Exception
            lbStatus.Text = "btnGenerate_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub importtoTemp()
        Try
            Dim GVR As GridViewRow
            Dim SQLstring As String
            If (ddlFindProductCode.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Product Code must have value")
                ddlFindProductCode.Focus()
                Exit Sub
            End If
           If (ddlFindQtyOpname.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Qty Opname must have value")
                ddlFindQtyOpname.Focus()
                Exit Sub
            End If

            Dim IdFindProductCode, IdFindQtyOpname, IdFindRemark As Integer
            Dim VarFindProductCode, VarFindQtyOpname, VarFindRemark As String
            IdFindProductCode = ddlFindProductCode.SelectedIndex - 1
           IdFindQtyOpname = ddlFindQtyOpname.SelectedIndex - 1
            IdFindRemark = ddlFindRemark.SelectedIndex - 1
           
            For Each GVR In GridExcel.Rows
             If IdFindProductCode < 0 Then
                    VarFindProductCode = ""
                Else
                    VarFindProductCode = GVR.Cells(IdFindProductCode).Text
                End If
                'lbStatus.Text = VarFindItem
                'Exit Sub
                If IdFindQtyOpname < 0 Then
                    VarFindQtyOpname = ""
                Else
                    VarFindQtyOpname = GVR.Cells(IdFindQtyOpname).Text
                    ' tbQtyActual_TextChanged(Nothing, Nothing)
                End If
                If IdFindRemark < 0 Then
                    VarFindRemark = ""
                Else
                    VarFindRemark = GVR.Cells(IdFindRemark).Text
                End If
                
              
                VarFindProductCode = TrimStr(VarFindProductCode)
                VarFindQtyOpname = TrimStr(VarFindQtyOpname)
                VarFindRemark = TrimStr(VarFindRemark)
                If VarFindProductCode.Length > 0 Then
                    SQLstring = "EXEc S_STOpnameTempImportDt " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text.Trim) + ", " + _
                    QuotedStr(VarFindProductCode) + ", '', " + VarFindQtyOpname.Replace(",", "") + ", 0, 0, " + QuotedStr(VarFindRemark)
                    SQLExecuteNonQuery(SQLstring, ViewState("DBConnection"))
                End If
            Next
        Catch ex As Exception
            lbStatus.Text = "importtoTemp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub importsave()
        Dim DrProduct, Row, Dr As DataRow
        Dim DtProduct As DataTable

        Try
            DtProduct = SQLExecuteQuery("EXEC S_STOpnameTempView " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text.Trim), ViewState("DBConnection").ToString).Tables(0)
            'lbStatus.Text = "EXEC S_STOpnameTempView " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text.Trim)
            'Exit Sub

            For Each DrProduct In DtProduct.Rows
                If CekExistData(ViewState("Dt"), "Product", TrimStr(DrProduct("Product_Code"))) Then
                    Row = ViewState("Dt").Select("Product = " + QuotedStr(TrimStr(DrProduct("Product_Code"))))(0)
                    Row.BeginEdit()
                    Row("Product") = DrProduct("Product_Code")
                    Row("Product_Name") = DrProduct("Product_Name")
                    Row("Specification") = DrProduct("Specification")
                    Row("Remark") = DrProduct("Remark")
                    Row("Unit") = DrProduct("Unit")
                    Row("QtySystem") = FormatFloat(DrProduct("QtySystem"), ViewState("DigitQty"))
                    Row("QtyActual") = FormatFloat(DrProduct("QtyActual"), ViewState("DigitQty"))
                    Row("QtyOpname") = FormatFloat(DrProduct("QtyOpname"), ViewState("DigitQty"))
                  Row.EndEdit()
                Else
                    Dr = ViewState("Dt").NewRow
                    Dr("Product") = DrProduct("Product_Code")
                    Dr("Product_Name") = DrProduct("Product_Name")
                    Dr("Specification") = DrProduct("Specification")
                    Dr("Remark") = DrProduct("Remark")
                    Dr("Unit") = DrProduct("Unit")
                   Dr("QtySystem") = FormatFloat(DrProduct("QtySystem"), ViewState("DigitQty"))
                    Dr("QtyActual") = FormatFloat(DrProduct("QtyActual"), ViewState("DigitQty"))
                    Dr("QtyOpname") = FormatFloat(DrProduct("QtyOpname"), ViewState("DigitQty"))
                    ViewState("Dt").Rows.Add(Dr)
                End If
            Next
        Catch ex As Exception
            lbStatus.Text = "importsave Error : " + ex.ToString
        End Try
    End Sub

End Class
