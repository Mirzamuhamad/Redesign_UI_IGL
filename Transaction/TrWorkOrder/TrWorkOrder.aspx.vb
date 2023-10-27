Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
Imports System.Windows.Forms
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrWorkOrder_TrWorkOrder
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PROWOHdUser "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not ViewState("ItemNoClose") Is Nothing Then
                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    sqlstring = "Declare @A VarChar(255) EXEC S_PDWOClosing '" + ViewState("Reference") + "', " + ViewState("ItemNoClose").ToString + ", '" + HiddenRemarkClose.Value + "'," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    'lbStatus.Text = sqlstring
                    'Exit Sub
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
                    If result.Length > 2 Then
                        lbStatus.Text = result
                    Else
                        BindDataDt(ViewState("Reference"))
                        GridDt.Columns(0).Visible = False
                        GridDt.Columns(1).Visible = False
                        GridDt.Columns(2).Visible = True
                    End If
                End If
                ViewState("ItemNoClose") = Nothing
                HiddenRemarkClose.Value = ""
            End If
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then                
                If ViewState("Sender") = "btnReffNmbr" Then
                    'ResultField = "Reference, Product, Product_Name, Specification, Qty, Unit, BOM_No"
                    BindToText(tbReffNmbr, Session("Result")(0).ToString)
                    BindToText(tbProductFG, Session("Result")(1).ToString)
                    BindToText(tbProductNameFG, Session("Result")(2).ToString)
                    BindToText(tbSpecificationFG, Session("Result")(3).ToString)
                    'BindToText(tbQtyFG, Session("Result")(4).ToString)
                    tbQtyFG.Text = FormatNumber(Session("Result")(4).ToString, ViewState("DigitQty"))
                    BindToText(tbUnitFG, Session("Result")(5).ToString)
                    'BindToText(tbBOMNo, Session("Result")(6).ToString)
                    Dim dt As New DataTable
                    dt = SQLExecuteQuery("EXEC S_PDWOGetQtyConvert " + QuotedStr(tbProductFG.Text) + "," + QuotedStr(tbUnitFG.Text) + "," + (CFloat(tbQtyFG.Text)).ToString + ", '''M2'',''SQM'''", ViewState("DBConnection").ToString).Tables(0)
                    tbQtyM2.Text = FormatNumber(dt.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
                    tbUnitPerM2.Text = FormatNumber(dt.Rows(0)("QtyWrhs").ToString, ViewState("DigitQty"))

                    Dim dt2 As New DataTable
                    dt2 = SQLExecuteQuery("EXEC S_PDWOGetQtyConvert " + QuotedStr(tbProductFG.Text) + "," + QuotedStr(tbUnitFG.Text) + "," + (CFloat(tbQtyFG.Text)).ToString + ", 'Roll'", ViewState("DBConnection").ToString).Tables(0)
                    tbQtyRoll.Text = FormatNumber(dt2.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
                    tbUnitPerRoll.Text = FormatNumber(dt2.Rows(0)("QtyWrhs").ToString, ViewState("DigitQty"))
                End If

                If ViewState("Sender") = "btnBOM" Then
                    'ResultField = "BOM_No, Process, Process_Name, OutputType, ProductOutput, ProductOutput_Name, Unit, FormulaNo, FormulaName"
                    BindToText(tbBOMNo, Session("Result")(0).ToString)
                    'tbProcessCode.Text = Session("Result")(1).ToString
                    'tbProcessName.Text = Session("Result")(2).ToString
                    'lbOutputType.Text = Session("Result")(3).ToString
                    'tbProductCode.Text = Session("Result")(4).ToString
                    'tbProductName.Text = Session("Result")(5).ToString
                    'tbUnit.Text = Session("Result")(6).ToString
                    'tbFormulaCode.Text = Session("Result")(7).ToString
                    'tbFormulaName.Text = Session("Result")(8).ToString
                End If
                If ViewState("Sender") = "btnProcess" Then
                    tbProcessCode.Text = Session("Result")(0).ToString
                    tbProcessName.Text = Session("Result")(1).ToString
                    lbOutputType.Text = Session("Result")(2).ToString
                    tbProductCode.Text = Session("Result")(3).ToString
                    tbProductName.Text = Session("Result")(4).ToString
                    tbUnit.Text = Session("Result")(5).ToString
                    tbFormulaCode.Text = Session("Result")(6).ToString
                    tbFormulaName.Text = Session("Result")(7).ToString
                End If

                If ViewState("Sender") = "btnMachine" Then
                    BindToText(tbMachine, Session("Result")(0).ToString)
                    BindToText(tbMachineName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnMaterial" Then
                    BindToText(tbMaterialAltCode, Session("Result")(0).ToString)
                    BindToText(tbMaterialAltName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    Dim i As Integer
                    i = 1
                    For Each drResult In Session("Result").Rows
                        If CekExistData(ViewState("Dt"), "Process", drResult("Process")) = False Then
                            Dim dr As DataRow
                            'ResultField = "Process, ProcessName, OutputType, ProductOutput, ProductOutputName, QtyOutput, UnitOutput, FormulaNo "
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = i
                            dr("Process") = drResult("Process")
                            dr("Process_Name") = TrimStr(drResult("ProcessName").ToString)
                            dr("Product") = TrimStr(drResult("ProductOutput").ToString)
                            dr("Product_Name") = TrimStr(drResult("ProductOutputName").ToString)
                            dr("Specification") = drResult("Specification")
                            dr("Qty") = drResult("QtyOutput")
                            dr("Unit") = drResult("UnitOutput")
                            dr("Formula") = drResult("FormulaNo")
                            dr("OutputType") = drResult("OutputType")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                        i = i + 1
                        Dim dt2 As New DataTable
                        Dim drResult2, dr2 As DataRow
                        Dim Row As DataRow()

                        dt2 = SQLExecuteQuery("EXEC S_PDWOGetDtMaterial " + QuotedStr(tbBOMNo.Text) + "," + (CFloat(tbQtyFG.Text)).ToString + "," + (CFloat(tbWaste.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
                        For Each drResult2 In dt2.Rows
                            'If Row.Count = 0 Then

                            dr2 = ViewState("Dt2").NewRow
                            dr2("BOMNo") = tbBOMNo.Text
                            dr2("Product") = tbProductFG.Text
                            dr2("ProductFG_Name") = tbProductNameFG.Text
                            dr2("Process") = drResult2("Process")
                            dr2("Process_Name") = drResult2("ProcessName")
                            dr2("Material") = drResult2("Material")
                            dr2("Material_Name") = drResult2("ProductName")
                            dr2("Specification") = drResult2("Specification")
                            dr2("Qty") = FormatFloat(drResult2("Qty"), 4)
                            dr2("Unit") = drResult2("Unit")
                            dr2("QtyWaste") = FormatFloat(drResult2("QtyWaste"), 4)
                            dr2("QtyTotal") = FormatFloat(drResult2("QtyTotal"), 4)
                            ViewState("Dt2").Rows.Add(dr2)
                            BindGridDt2(ViewState("Dt2"), GridDt2)                            
                        Next
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(ViewState("Dt").Rows.count = 0)
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing
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
        'FillCombo(ddlWorkCtr, "EXEC S_GetWorkCtr", False, "WorkCtr_Code", "WorkCtr_Name", ViewState("DBConnection"))
        FillCombo(ddlWorkCtr, "Select WorkCtr, WorkCtr_Name from VMsWorkCtrUser WHERE UserId = " + QuotedStr(ViewState("UserId")), False, "WorkCtr", "WorkCtr_Name", ViewState("DBConnection"))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            'ddlCommand.Items.Add("Print")
            'ddlCommand2.Items.Add("Print")
        End If

        lbjudul.Text = "Work Order"
        tbChartingPosition.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbDesignPrinting.Attributes.Add("OnKeyDown", "return PressNumeric();")

        'FillCombo(ddlShift, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
        tbQtyFG.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyRoll.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyM2.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbQtyM2.Attributes.Add("OnBlur", "setformat();")
        tbQtyRoll.Attributes.Add("OnBlur", "setformat();")
        tbQtyFG.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
	Dim SQLString AS String
        Dim StrFilter As String
        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            Else
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            End If
            SQLString = GetStringHd + " WHERE UserId = " + QuotedStr(ViewState("UserId"))
            DT = BindDataTransaction(SQLString, StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * FROM V_PROWODt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_PROWODt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                'Dim GVR As GridViewRow
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
                Session("SelectCommand") = "EXEC S_PDFormWO " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormWO.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PDWO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlPurpose.Enabled = State
            tbDate.Enabled = State
            tbWaste.Enabled = State
            'ddlWOGroup.Enabled = State
            ddlWorkCtr.Enabled = State
            ddlReffType.Enabled = State
            tbQtyFG.Enabled = State
            tbQtyRoll.Enabled = State
            tbQtyM2.Enabled = State
            btnGetData.Visible = State
            btnReffNmbr.Visible = State
            tbBOMNo.Enabled = State
            btnBOM.Visible = State
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
            BindGridDt2(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt 2 Error : " + ex.ToString)
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
            If pnlDt.Visible = False And ViewState("StateDt") = "Edit" And pnlDt2.Visible = False Then
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
                tbRef.Text = GetAutoNmbr("WO", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlWorkCtr.SelectedValue, ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PROWOHd (TransNmbr, Status, TransDate, WorkCtr, PercentWaste, " + _
                "ReffType, ReffNmbr, BOMNo, ProductFG, Qty, Unit, QtyM2, UnitPerM2, QtyRoll, UnitPerRoll, " + _
                "Purpose, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlWorkCtr.SelectedValue) + "," + tbWaste.Text + _
                "," + QuotedStr(ddlReffType.SelectedValue) + "," + QuotedStr(tbReffNmbr.Text) + _
                "," + QuotedStr(tbBOMNo.Text) + "," + QuotedStr(tbProductFG.Text) + _
                "," + CFloat(tbQtyFG.Text).ToString.Replace(",", "") + "," + QuotedStr(tbUnitFG.Text) + _
                "," + CFloat(tbQtyM2.Text).ToString.Replace(",", "") + "," + QuotedStr(tbUnitPerM2.Text) + _
                "," + CFloat(tbQtyRoll.Text).ToString.Replace(",", "") + "," + QuotedStr(tbUnitPerRoll.Text) + _
                ", " + QuotedStr(ddlPurpose.SelectedValue) + _
                ", " + QuotedStr(tbRemark.Text) + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PROWOHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                'If cekStatus = "P" Then -- gak bisa dipakai, karena machine masih bisa edit walau P
                '    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                '    Exit Sub
                'End If
                SQLString = "UPDATE PROWOHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", WorkCtr = " + QuotedStr(ddlWorkCtr.SelectedValue) + ", PercentWaste = " + (CFloat(tbWaste.Text)).ToString + _
                ", ReffType = " + QuotedStr(ddlReffType.SelectedValue) + _
                ", ReffNmbr = " + QuotedStr(tbReffNmbr.Text) + _
                ", BOMNo = " + QuotedStr(tbBOMNo.Text) + _
                ", ProductFG = " + QuotedStr(tbProductFG.Text) + _
                ", Qty = " + tbQtyFG.Text.Replace(",", "") + _
                ", Unit = " + QuotedStr(tbUnitFG.Text) + _
                ", QtyM2 = " + tbQtyM2.Text.Replace(",", "") + _
                ", UnitPerM2 = " + QuotedStr(tbUnitPerM2.Text) + _
                ", QtyRoll = " + tbQtyRoll.Text.Replace(",", "") + _
                ", UnitPerRoll = " + QuotedStr(tbUnitPerRoll.Text) + _
                ", Purpose = " + QuotedStr(ddlPurpose.SelectedValue) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate() " + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
                
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, LotNo, Process, Product, Qty, Unit, ProductionDate, Machine, Remark, ChartingPosition, DesignPrinting, DesignCode, FgSubkon, DODate, OutputType FROM PROWODt WHERE TransNmbr = '" & ViewState("Reference") & "' ", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE PROWODt SET ItemNo = @ItemNo, LotNo = @LotNo, Process = @Process, Product = @Product, " + _
            "Formula = @Formula, ChartingPosition = @ChartingPosition, " + _
            "DesignPrinting = @DesignPrinting, DesignCode = @DesignCode, " + _
            "Qty = @Qty, DODate = @DODate, FgSubkon = @FgSubkon, Unit = @Unit, OutputType = @OutputType," + _
            "Remark = @Remark, ProductionDate = @ProductionDate, Machine = @Machine " + _
            "WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @OldItemNo", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@Process", SqlDbType.VarChar, 5, "Process")
            Update_Command.Parameters.Add("@LotNo", SqlDbType.VarChar, 50, "LotNo")
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Formula", SqlDbType.VarChar, 20, "Formula")
            Update_Command.Parameters.Add("@ChartingPosition", SqlDbType.Float, 18, "ChartingPosition")
            Update_Command.Parameters.Add("@DesignPrinting", SqlDbType.VarChar, 50, "DesignPrinting")
            Update_Command.Parameters.Add("@DesignCode", SqlDbType.VarChar, 30, "DesignCode")
            Update_Command.Parameters.Add("@ProductionDate", SqlDbType.DateTime, 8, "ProductionDate")
            Update_Command.Parameters.Add("@DODate", SqlDbType.DateTime, 8, "DODate")
            Update_Command.Parameters.Add("@FgSubkon", SqlDbType.VarChar, 1, "FgSubkon")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@OutputType", SqlDbType.VarChar, 30, "OutputType")
            Update_Command.Parameters.Add("@Machine", SqlDbType.VarChar, 5, "Machine")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PROWODt WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PROWODt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt


            'save dt2
            Dim cmdSql2 As New SqlCommand("SELECT TransNmbr, BOMNo, Process, Product, Material, Qty, Unit, MaterialAlt, QtyWaste, QtyTotal FROM PROWODt2 WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql2)
            Dim dbcommandBuilder2 As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder2.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder2.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder2.GetUpdateCommand

            Dim param2 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command2 = New SqlCommand( _
            "UPDATE PROWODt2 SET BOMNo = @BOMNo, Process = @Process, " + _
            "Product = @Product, Material = @Material, Qty = @Qty, " + _
            "Unit = @Unit, MaterialAlt = @MaterialAlt, QtyWaste = @QtyWaste, QtyTotal = @QtyTotal " + _
            "WHERE TransNmbr = '" & ViewState("Reference") & "' AND BOMNo = @OldBOMNo AND Process = @OldProcess AND Product = @OldProduct AND Material = @OldMaterial", con)

            ' Define output parameters.
            Update_Command2.Parameters.Add("@BOMNo", SqlDbType.VarChar, 20, "BOMNo")
            Update_Command2.Parameters.Add("@Process", SqlDbType.VarChar, 5, "Process")
            Update_Command2.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command2.Parameters.Add("@Material", SqlDbType.VarChar, 20, "Material")
            Update_Command2.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command2.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command2.Parameters.Add("@MaterialAlt", SqlDbType.VarChar, 20, "MaterialAlt")
            Update_Command2.Parameters.Add("@QtyWaste", SqlDbType.Float, 18, "QtyWaste")
            Update_Command2.Parameters.Add("@QtyTotal", SqlDbType.Float, 18, "QtyTotal")

            '' Define intput (WHERE) parameters.
            param2 = Update_Command2.Parameters.Add("@OldBOMNo", SqlDbType.VarChar, 20, "BOMNo")
            param.SourceVersion = DataRowVersion.Original
            param2 = Update_Command2.Parameters.Add("@OldProcess", SqlDbType.VarChar, 5, "Process")
            param.SourceVersion = DataRowVersion.Original
            param2 = Update_Command2.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            param2 = Update_Command2.Parameters.Add("@OldMaterial", SqlDbType.VarChar, 20, "Material")
            param.SourceVersion = DataRowVersion.Original

            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command2

            ' Create the DeleteCommand.
            Dim Delete_Command2 = New SqlCommand( _
                "DELETE FROM PROWODt2 WHERE TransNmbr = '" & ViewState("Reference") & "' AND BOMNo = @BOMNo AND Process = @Process AND Product = @Product AND Material = @Material", con)
            ' Add the parameters for the DeleteCommand.
            param2 = Delete_Command2.Parameters.Add("@BOMNo", SqlDbType.VarChar, 20, "BOMNo")
            param.SourceVersion = DataRowVersion.Original
            param2 = Delete_Command2.Parameters.Add("@Process", SqlDbType.VarChar, 5, "Process")
            param.SourceVersion = DataRowVersion.Original
            param2 = Delete_Command2.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            param2 = Delete_Command2.Parameters.Add("@Material", SqlDbType.VarChar, 20, "Material")
            param.SourceVersion = DataRowVersion.Original

            da.DeleteCommand = Delete_Command2

            Dim Dt2 As New DataTable("PROWODt2")
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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
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
            'tbAdjPercent.Text = "0"
            
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            EnableHd(True)
            btnHome.Visible = False
            pnlDt2.Visible = False
            pnlEditDt2.Visible = False
            tbDate.Focus()
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
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            GridDt.Columns(2).Visible = False
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlPurpose.SelectedIndex = 0
            ddlReffType.SelectedValue = "SO"
            tbReffNmbr.Text = ""
            tbBOMNo.Text = ""
            tbProductFG.Text = ""
            tbProductNameFG.Text = ""
            tbSpecificationFG.Text = ""
            tbWaste.Text = "0"
            tbQtyFG.Text = "0"
            tbUnitFG.Text = ""
            tbQtyRoll.Text = "0"
            tbQtyM2.Text = "0"
            tbUnitPerM2.Text = "0"
            tbUnitPerRoll.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbLotNo.Text = ""
            tbFormulaCode.Text = ""
            tbFormulaName.Text = ""
            tbProcessCode.Text = ""
            tbProcessName.Text = ""
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbQty.Text = "0"
            tbUnit.Text = ""
            tbProdDate.SelectedDate = tbDate.SelectedDate
            tbChartingPosition.Text = "0"
            'tbDesignCode.Text = ""
            'tbDODate.SelectedDate = tbDate.SelectedDate
            tbDODate.Clear()
            ddlSubkon.SelectedValue = "N"
            tbDesignPrinting.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbMaterialCode.Text = ""
            tbMaterialName.Text = ""
            tbMaterialAltCode.Text = ""
            tbMaterialAltName.Text = ""            
            tbQtyDt2.Text = "0"
            tbUnitDt2.Text = ""
            tbSpecificationDt2.Text = ""
            tbQtyWaste.Text = "0"
            tbQtyTotal.Text = "0"
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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
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
            newTrans()
            EnableHd(True)
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
        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        tbLotNo.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If                    
            If tbReffNmbr.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Reference Must Have Value")
                tbReffNmbr.Focus()
                Return False
            End If
            If tbBOMNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("BOM No Must Have Value")
                tbBOMNo.Focus()
                Return False
            End If
            If CFloat(tbQtyFG.Text.Trim) <= 0 Then
                lbStatus.Text = MessageDlg("Qty Must Have Value")
                tbQtyFG.Focus()
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
                If Dr("Process").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Process Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
            Else
                
                If tbProcessName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Process Must Have Value")
                    tbProcessCode.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
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
        'Dim FDateName, FDateValue, FilterName, FilterValue As String
        'Try
        '    FDateName = "Date"
        '    FDateValue = "TransDate"
        '    FilterName = "Reference, Status, Request No, Machine, Subkon, Est Complete Date, Project Manager, WorkCtr By, Approved By, Acknowled By, Remark"
        '    FilterValue = "TransNmbr, Status, ReffNmbr, MachineName, FgSubkon, dbo.FormatDate(EstCompleteDate), MachineName, WorkCtrByName, ApprovedByName, AcknowledByName, Remark"
        '    Session("DateFieldName") = FDateName.Split(",")
        '    Session("DateFieldValue") = FDateValue.Split(",")
        '    Session("FieldName") = FilterName.Split(",")
        '    Session("FieldValue") = FilterValue.Split(",")
        '    AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        'Catch ex As Exception
        '    lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        'End Try
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    ViewState("Status") = GVR.Cells(3).Text
                    ViewState("Waste") = GVR.Cells(7).Text                    
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    BindDataDt2(ViewState("Reference"))
                    pnlDt.Visible = True
                    GridDt.PageIndex = 0                    
                    GridDt.Columns(0).Visible = False
                    GridDt.Columns(1).Visible = False
                    GridDt.Columns(2).Visible = GVR.Cells(3).Text = "P"
                    GridDt2.Columns(0).Visible = False
                    btnAddDt.Visible = False
                    btnAddDt2.Visible = False
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    pnlDt2.Visible = False
                    btnHome.Visible = True
                    MovePanel(PnlHd, pnlInput)                    
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text <> "D" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("Status") = GVR.Cells(3).Text
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        BindDataDt2(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        pnlDt.Visible = True
                        GridDt.Columns(0).Visible = True
                        GridDt.Columns(1).Visible = GVR.Cells(3).Text <> "P"
                        GridDt.Columns(2).Visible = False
                        'GridDt.Columns(3).Visible = GVR.Cells(3).Text <> "P"
                        btnAddDt.Visible = GVR.Cells(3).Text <> "P"
                        btnAddDt2.Visible = GVR.Cells(3).Text <> "P"
                        GridDt2.Columns(0).Visible = True
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        pnlDt2.Visible = False
                        btnHome.Visible = False
                        MovePanel(PnlHd, pnlInput)
                        'tbAdjPercent.Text = "0"
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must not Delete to edit")
                    End If
                    ElseIf DDL.SelectedValue = "Print" Then
                        Try
                            Dim Param0, Param1 As String
                            Param0 = GVR.Cells(2).Text.Replace("/", ".")
                            Param1 = GVR.Cells(4).Text

                            Dim filePaths, ImageFile As String

                            filePaths = Server.MapPath("~/image/WorkOrder")
                            ImageFile = SQLExecuteScalar("Select ImageFile from SAUploadImage WHERE ImageGroup = 'WorkOrder' and ImageCode = '" + Param0 + "' ", ViewState("DBConnection").ToString)

                            filePaths = filePaths + "\" + ImageFile

                            Session("DBCOnnection") = ViewState("DBConnection")
                            CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                            If CekMenu <> "" Then
                                lbStatus.Text = CekMenu
                                Exit Sub
                            End If
                            Session("SelectCommand") = "EXEC S_PDFormWO ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(filePaths)

                            Session("ReportFile") = ".../../../Rpt/FormWO.frx"
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Catch ex As Exception
                            lbStatus.Text = "btn print Error = " + ex.ToString
                        End Try
                    ElseIf DDL.SelectedValue = "Photo" Then
                        Try
                            Dim paramgo As String
                            'lbStatus.Text = GVR.Cells(2).Text.Replace("/", ".")
                            'Exit Sub
                            'paramgo = GVR.Cells(2).Text + "|" + GVR.Cells(4).Text + "|WorkOrder"
                            paramgo = GVR.Cells(2).Text.Replace("/", ".") + "|" + GVR.Cells(4).Text + "|WorkOrder"
                            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenTransaction('TrUploadImage', '" + paramgo + "' );", True)
                            'End If
                            AttachScript("OpenTransaction('TrUploadImage', '" + Request.QueryString("KeyId") + "', '" + paramgo + "' );", Page, Me.GetType())
                        Catch ex As Exception
                            lbStatus.Text = "DDL.SelectedValue = Menu Error : " + ex.ToString
                        End Try
                    End If
                End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated

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
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status Work Order is not Post, cannot close Detail Work Order")
                    Exit Sub
                End If
                ViewState("ItemNoClose") = GVR.Cells(4).Text
                If GVR.Cells(15).Text <> "Y" Then
                    AttachScript("closing();", Page, Me.GetType)
                Else
                    ViewState("ItemNoClose") = Nothing
                End If
            ElseIf e.CommandName = "DetailMaterial" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                'lbProductFGCode.Text = GVR.Cells(7).Text
                'lbProductFGName.Text = " - " + GVR.Cells(8).Text

                'lbBOMNo.Text = GVR.Cells(10).Text
                'lbFormula.Text = " - " + GVR.Cells(11).Text

                lbProcessCode.Text = GVR.Cells(7).Text
                lbProcessName.Text = " - " + GVR.Cells(8).Text

                ' ''If GVR.Cells(4).Text = "&nbsp;" Then
                ' ''    lbStatus.Text = "Product Finish Goods " + lbProductFGCode.Text + lbProductFGName.Text + " not exists have Detail"
                ' ''    Exit Sub
                ' ''End If


                ''lbStatus.Text = QuotedStr(GVR.Cells(10).Text) + "," + QuotedStr(GVR.Cells(7).Text) + "," + QuotedStr(GVR.Cells(12).Text)
                ''Exit Sub

                'If ViewState("StateHd") <> "View" Then
                '    Dim dt2 As New DataTable
                '    Dim drResult, dr As DataRow
                '    Dim Row As DataRow()

                '    dt2 = SQLExecuteQuery("EXEC S_PDWOGetBOMMaterial " + QuotedStr(GVR.Cells(10).Text) + "," + QuotedStr(GVR.Cells(7).Text) + "," + QuotedStr(GVR.Cells(12).Text) + "," + (CFloat(tbWaste.Text)).ToString + "," + (CFloat(GVR.Cells(16).Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
                '    For Each drResult In dt2.Rows
                '        Row = ViewState("Dt2").Select("BOMNo = " + QuotedStr(drResult("BOM_No")) + " AND Product = " + QuotedStr(drResult("ProductFG_Code")) + " AND Process = " + QuotedStr(drResult("Process_Code")))

                '        'If Row.Count = 0 Then
                '        If CekExistData(ViewState("Dt2"), "BOMNo,Product,Process", drResult("BOM_No") + "|" + drResult("ProductFG_Code") + "|" + drResult("Process_Code")) = False Then
                '            dr = ViewState("Dt2").NewRow
                '            dr("BOMNo") = drResult("BOM_No")
                '            dr("Product") = drResult("ProductFG_Code")
                '            dr("ProductFG_Name") = TrimStr(drResult("ProductFG_Name"))
                '            dr("Process") = drResult("Process_Code")
                '            dr("Process_Name") = drResult("Process_Name")
                '            dr("Material") = drResult("Material_Code")
                '            dr("Material_Name") = drResult("Material_Name")
                '            dr("Specification") = drResult("Specification")
                '            dr("Qty") = FormatFloat(drResult("Qty"), ViewState("DigitQty"))
                '            dr("Unit") = drResult("Unit")
                '            dr("QtyWaste") = FormatFloat(drResult("QtyWaste"), ViewState("DigitQty"))
                '            dr("QtyTotal") = FormatFloat(drResult("QtyTotal"), ViewState("DigitQty"))
                '            ViewState("Dt2").Rows.Add(dr)
                '            BindGridDt2(ViewState("Dt2"), GridDt2)                            
                '        Else
                '            'Dim drow As DataRow
                '            'If tbWaste.Text <> ViewState("Waste") Then
                '            '    drow = ViewState("Dt2").Select("BOMNo = " + QuotedStr(drResult("BOM_No")) + " AND Product = " + QuotedStr(drResult("ProductFG_Code")) + " AND Process = " + QuotedStr(drResult("Process_Code")))
                '            '    drow.BeginEdit()
                '            '    drow("QtyWaste") = drow("Qty") * (CFloat(tbWaste.Text) / 100)
                '            '    drow("QtyTotal") = drow("Qty") + drow("QtyWaste")
                '            '    drow.EndEdit()
                '            'End If

                '        End If
                '    Next
                'Else
                'BindGridDt2(ViewState("Dt2"), GridDt2)
                'End If
                

                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    drow = ViewState("Dt2").Select("Process = " + QuotedStr(GVR.Cells(7).Text))
                    If drow.Length > 0 Then
                        BindGridDt2(drow.CopyToDataTable, GridDt2)
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

                MovePanel(pnlDt, pnlDt2)

                GridDt2.Columns(0).Visible = ViewState("StateHd") <> "View"
                btnSaveAll.Visible = ViewState("StateHd") <> "View"
                btnSaveTrans.Visible = ViewState("StateHd") <> "View"
                btnBack.Visible = ViewState("StateHd") <> "View"
                btnHome.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            Dim ExistRowDt() As DataRow
            ExistRowDt = ViewState("Dt2").Select("Process = " + QuotedStr(GVR.Cells(7).Text))
            If ExistRowDt.Length > 0 Then
                Dim drDt() As DataRow
                For Each R As DataRow In ExistRowDt
                    drDt = ViewState("Dt2").Select("Process = " + QuotedStr(GVR.Cells(7).Text) + " AND Material = " + QuotedStr(R("Material").ToString))
                    drDt(0).Delete()
                Next
            End If
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(4).Text))
            dr(0).Delete()
            'ViewState("Dt").AcceptChanges()
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
            ViewState("DtValue") = GVR.Cells(4).Text
            FillTextBoxDt(ViewState("DtValue"))
            If ViewState("Status") = "P" Then
                DisableDt()
            End If
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
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
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlWorkCtr, Dt.Rows(0)("WorkCtr").ToString)
            BindToDropList(ddlPurpose, Dt.Rows(0)("Purpose").ToString)
            BindToDropList(ddlReffType, Dt(0)("ReffType").ToString)
            BindToText(tbReffNmbr, Dt(0)("ReffNmbr").ToString)
            BindToText(tbBOMNo, Dt(0)("BOMNo").ToString)
            BindToText(tbProductFG, Dt(0)("ProductFG").ToString)
            BindToText(tbProductNameFG, Dt(0)("ProductFg_Name").ToString)
            BindToText(tbSpecificationFG, Dt(0)("SpecificationFg").ToString)
            BindToText(tbQtyFG, Dt(0)("Qty").ToString, ViewState("DigitQty"))
            BindToText(tbUnitFG, Dt(0)("Unit").ToString)
            BindToText(tbQtyM2, Dt(0)("QtyM2").ToString, ViewState("DigitQty"))
            BindToText(tbUnitPerM2, Dt(0)("UnitPerM2").ToString)
            BindToText(tbQtyRoll, Dt(0)("QtyRoll").ToString, ViewState("DigitQty"))
            BindToText(tbUnitPerRoll, Dt(0)("UnitPerRoll").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbWaste, Dt.Rows(0)("PercentWaste").ToString, ViewState("DigitPercent"))
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + Product)
            If Dr.Length > 0 Then
                lbItemNo.Text = Product.ToString                
                BindToText(tbLotNo, Dr(0)("LotNo").ToString)                
                BindToText(tbFormulaCode, Dr(0)("Formula").ToString)
                BindToText(tbFormulaName, Dr(0)("FormulaName").ToString)                
                BindToText(tbProcessCode, Dr(0)("Process").ToString)
                BindToText(tbProcessName, Dr(0)("Process_Name").ToString)
                BindToText(tbMachine, Dr(0)("Machine").ToString)
                BindToText(tbMachineName, Dr(0)("Machine_Name").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToDate(tbProdDate, Dr(0)("ProductionDate").ToString)
                BindToText(tbChartingPosition, Dr(0)("ChartingPosition").ToString)
                BindToText(tbDesignPrinting, Dr(0)("DesignPrinting").ToString)
                'BindToText(tbDesignCode, Dr(0)("DesignCode").ToString)
                BindToDate(tbDODate, Dr(0)("DODate").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                If IsDBNull(Dr(0)("FgSubkon")) = True Then
                    ddlSubkon.SelectedValue = "N"
                Else
                    BindToDropList(ddlSubkon, Dr(0)("FgSubkon").ToString)
                End If
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                lbOutputType.Text = TrimStr(Dr(0)("OutputType").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub DisableDt()
        Try
            tbLotNo.Enabled = False
            tbProcessCode.Enabled = False
            tbQty.Enabled = False
            btnProcess.Visible = False
            tbChartingPosition.Enabled = False
            tbProdDate.Enabled = False
            tbDODate.Enabled = False
            tbDesignPrinting.Enabled = False
            ddlSubkon.Enabled = False
            tbRemarkDt.Enabled = False
        Catch ex As Exception
            Throw New Exception("DisableDt error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal BOMNo As String, ByVal Product As String, ByVal Process As String, ByVal Material As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").Select("BOMNo = " + QuotedStr(BOMNo) + " AND Product = " + QuotedStr(Product) + " AND Process = " + QuotedStr(Process) + " AND Material = " + QuotedStr(Material))
            If Dr.Length > 0 Then
                BindToText(tbMaterialCode, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("Material_Name").ToString)
                BindToText(tbSpecificationDt2, Dr(0)("Specification").ToString)
                BindToText(tbQtyDt2, Dr(0)("Qty").ToString, 4)
                BindToText(tbQtyWaste, Dr(0)("QtyWaste").ToString, 4)
                BindToText(tbQtyTotal, Dr(0)("QtyTotal").ToString, 4)
                BindToText(tbUnitDt2, Dr(0)("Unit").ToString)
                BindToText(tbMaterialAltCode, Dr(0)("MaterialAlt").ToString)
                BindToText(tbMaterialAltName, Dr(0)("MaterialAlt_Name").ToString)
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

    Sub BindGridDt(ByVal source As DataTable, ByVal gv As GridView)
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
            gv.Columns(1).Visible = (Not IsEmpty) And (ViewState("Status") <> "P")
            gv.Columns(2).Visible = False
            gv.Columns(3).Visible = Not IsEmpty
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
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
            gv.Columns(0).Visible = Not IsEmpty
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                'If ViewState("DtValue") <> TrimStr(tbProductCode.Text) Then
                '    If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) Then
                '        lbStatus.Text = "Product " + tbProductCode.Text + " has already exists"
                '        Exit Sub
                '    End If
                'End If
                'Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ItemNo") = lbItemNo.Text                
                Row("LotNo") = tbLotNo.Text
                Row("Formula") = tbFormulaCode.Text
                Row("FormulaName") = tbFormulaName.Text
                Row("Process") = tbProcessCode.Text
                Row("Process_Name") = tbProcessName.Text
                Row("Machine") = tbMachine.Text
                Row("Machine_Name") = tbMachineName.Text
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = tbUnit.Text
                If Format(tbProdDate.SelectedDate, "dd MMM yyyy") = "01 Jan 0001" Then
                    Row("ProductionDate") = DBNull.Value
                Else
                    Row("ProductionDate") = Format(tbProdDate.SelectedDate, "dd MMM yyyy")
                End If
                If tbChartingPosition.Text = "" Then
                    Row("ChartingPosition") = "0"
                Else
                    Row("ChartingPosition") = tbChartingPosition.Text
                End If
                If tbDesignPrinting.Text = "" Then
                    Row("DesignPrinting") = "0"
                Else
                    Row("DesignPrinting") = tbDesignPrinting.Text
                End If
                'Row("DesignCode") = tbDesignCode.Text
                If Format(tbDODate.SelectedDate, "dd MMM yyyy") = "01 Jan 0001" Then
                    Row("DODate") = DBNull.Value
                Else
                    Row("DODate") = Format(tbDODate.SelectedDate, "dd MMM yyyy")
                End If
                Row("OutputType") = lbOutputType.Text
                Row("FgSubkon") = ddlSubkon.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                'If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) Then
                '    lbStatus.Text = "Product " + tbProductCode.Text + " has already exists"
                '    Exit Sub
                'End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("LotNo") = tbLotNo.Text
                dr("Formula") = tbFormulaCode.Text
                dr("FormulaName") = tbFormulaName.Text
                dr("Process") = tbProcessCode.Text
                dr("Process_Name") = tbProcessName.Text
                dr("Machine") = tbMachine.Text
                dr("Machine_Name") = tbMachineName.Text
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = tbUnit.Text
                If Format(tbProdDate.SelectedDate, "dd MMM yyyy") = "01 Jan 0001" Then
                    dr("ProductionDate") = DBNull.Value
                Else
                    dr("ProductionDate") = Format(tbProdDate.SelectedDate, "dd MMM yyyy")
                End If
                If tbChartingPosition.Text = "" Then
                    dr("ChartingPosition") = "0"
                Else
                    dr("ChartingPosition") = tbChartingPosition.Text
                End If
                If tbDesignPrinting.Text = "" Then
                    dr("DesignPrinting") = "0"
                Else
                    dr("DesignPrinting") = tbDesignPrinting.Text
                End If
                'dr("DesignCode") = tbDesignCode.Text
                If Format(tbDODate.SelectedDate, "dd MMM yyyy") = "01 Jan 0001" Then
                    dr("DODate") = DBNull.Value
                Else
                    dr("DODate") = Format(tbDODate.SelectedDate, "dd MMM yyyy")
                End If
                dr("OutputType") = lbOutputType.Text
                dr("FgSubkon") = ddlSubkon.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            ViewState("Add") = "NotClear"
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
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Dim ResultField As String
        Try
            If tbProductNameFG.Text = "" Then
                lbStatus.Text = MessageDlg("Product Finish Goods must have value")
                tbProductFG.Focus()
                Exit Sub
            End If
            If tbBOMNo.Text = "" Then
                lbStatus.Text = MessageDlg("BOM No must have value")
                tbBOMNo.Focus()
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "SELECT Process, Process_Name, OutputType, ProductOutput, ProductOutput_Name, Unit, FormulaNo, FormulaName FROM V_PDWOGetBOM WHERE Product = " + QuotedStr(tbProductFG.Text) + " AND BOM_No = " + QuotedStr(tbBOMNo.Text)
            ResultField = "Process, Process_Name, OutputType, ProductOutput, ProductOutput_Name, Unit, FormulaNo, FormulaName"
            ViewState("Sender") = "btnProcess"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnProcess_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProcessCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProcessCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT Process, Process_Name, OutputType, ProductOutput, ProductOutput_Name, Unit, FormulaNo, FormulaName FROM V_PDWOGetBOM WHERE Product = " + QuotedStr(tbProductFG.Text) + " AND BOM_No = " + QuotedStr(tbBOMNo.Text) + " AND Process = " + QuotedStr(tbProcessCode.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProcessCode.Text = Dr("Process")
                tbProcessName.Text = Dr("Process_Name")
                lbOutputType.Text = Dr("OutputType")
                tbProductCode.Text = Dr("ProductOutput")
                tbProductName.Text = Dr("ProductOutput_Name")
                tbUnit.Text = Dr("Unit")
                tbFormulaCode.Text = Dr("FormulaNo")
                tbFormulaName.Text = Dr("FormulaName")
            Else
                tbProcessCode.Text = ""
                tbProcessName.Text = ""
                lbOutputType.Text = ""
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbUnit.Text = ""
                tbFormulaCode.Text = ""
                tbFormulaName.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("tbProcessCode_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbQtyFG_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyFG.TextChanged
        Try
            Dim dt As New DataTable
            dt = SQLExecuteQuery("EXEC S_PDWOGetQtyConvert " + QuotedStr(tbProductFG.Text) + "," + QuotedStr(tbUnitFG.Text) + "," + (CFloat(tbQtyFG.Text)).ToString + ", '''M2'',''SQM'''", ViewState("DBConnection").ToString).Tables(0)
            tbQtyM2.Text = FormatNumber(dt.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
            tbUnitPerM2.Text = FormatNumber(dt.Rows(0)("QtyWrhs").ToString, ViewState("DigitQty"))

            Dim dt2 As New DataTable
            dt2 = SQLExecuteQuery("EXEC S_PDWOGetQtyConvert " + QuotedStr(tbProductFG.Text) + "," + QuotedStr(tbUnitFG.Text) + "," + (CFloat(tbQtyFG.Text)).ToString + ", 'Roll'", ViewState("DBConnection").ToString).Tables(0)
            tbQtyRoll.Text = FormatNumber(dt2.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
            tbUnitPerRoll.Text = FormatNumber(dt2.Rows(0)("QtyWrhs").ToString, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("tbQty_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    'Protected Sub tbQtyRoll_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyRoll.TextChanged
    '    Try
    '        Dim dt As New DataTable
    '        dt = SQLExecuteQuery("EXEC S_PDWOGetQtyConvert " + QuotedStr(tbProductFG.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + ", '''M2'', ''SQM'''", ViewState("DBConnection").ToString).Tables(0)
    '        tbQtyM2.Text = FormatNumber(dt.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))
    '        tbUnitPerM2.Text = FormatNumber(dt.Rows(0)("QtyWrhs").ToString, ViewState("DigitQty"))

    '        Dim dt2 As New DataTable
    '        dt2 = SQLExecuteQuery("EXEC S_PDWOGetQtyConvert " + QuotedStr(tbProductFG.Text) + ",'Roll'," + (CFloat(tbQtyRoll.Text)).ToString + ", " + QuotedStr(tbUnitFG.Text), ViewState("DBConnection").ToString).Tables(0)
    '        tbQtyFG.Text = FormatNumber(dt2.Rows(0)("QtyConvert").ToString, ViewState("DigitQty"))

    '    Catch ex As Exception
    '        Throw New Exception("tbQtyRoll_TextChanged error: " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub tbBOMNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBOMNo.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String

        Try
            SQLString = "SELECT BOM_No, Product FROM V_PDBOMEffective WHERE BOM_No = " + QuotedStr(tbBOMNo.Text) + " AND Product = " + QuotedStr(tbProductFG.Text) '+ " AND " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " BETWEEN Start_Effective AND End_Effective "

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbBOMNo, Trim(Dr("BOM_No").ToString))
            Else
                tbBOMNo.Text = ""
            End If
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbBOMNo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBOM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBOM.Click
        Dim ResultField As String

        Try
            Session("Result") = Nothing
            'Session("filter") = "SELECT * FROM V_PDWOGetBOM WHERE Product = " + QuotedStr(tbProductFG.Text)
            'lbStatus.Text = "SELECT BOM_No FROM V_PDBOMEffective B WHERE Product = " + QuotedStr(tbProductFG.Text) + " AND " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " BETWEEN Start_Effective AND End_Effective "
            'Exit Sub
            'Session("filter") = "SELECT BOM_No,Product,ProductName,Specification,BOM_Date,Start_Effective,Remark FROM V_PDBOMEffective WHERE Product = " + QuotedStr(tbProductFG.Text) + " AND " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " BETWEEN Start_Effective AND End_Effective "
            Session("filter") = "SELECT BOM_No,Product,ProductName,Specification,convert(varchar(20), BOM_Date, 106) As BOM_Date,convert(varchar(20), Start_Effective, 106) As Start_Effective,Remark FROM V_PDBOMEffective WHERE Product = " + QuotedStr(tbProductFG.Text) '+ " AND " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " BETWEEN Start_Effective AND End_Effective "
            'ResultField = "BOM_No, Process, Process_Name, OutputType, ProductOutput, ProductOutput_Name, Unit, FormulaNo, FormulaName"
            ResultField = "BOM_No, Product"
            ViewState("Sender") = "btnBOM"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnBOM_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProductFG_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductFG.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE WorkCtr = " + QuotedStr(ddlWorkCtr.SelectedValue) + " AND Product_Code = " + QuotedStr(tbProductFG.Text)
            
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductFG.Text = Dr("Product_Code")
                tbProductNameFG.Text = Dr("Product_Name")
                tbSpecificationFG.Text = Dr("Specification")
            Else
                tbProductFG.Text = ""
                tbProductNameFG.Text = ""
                tbSpecificationFG.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbProductFG_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2.Click, btnBackDt2ke2.Click
        Try
            MovePanel(pnlDt2, pnlDt)            
            btnSaveAll.Visible = ViewState("StateHd") <> "View"
            btnSaveTrans.Visible = ViewState("StateHd") <> "View"
            btnBack.Visible = ViewState("StateHd") <> "View"
            btnHome.Visible = ViewState("StateHd") = "View"
        Catch ex As Exception
            Throw New Exception("btnBackDt2_Click error: " + ex.ToString)
        End Try
    End Sub
    Dim Qty As Decimal = 0
    Dim QtyWaste As Decimal = 0
    Dim QtyTotal As Decimal = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Material")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    Qty += FloorKoma(Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty")), 4)
                    QtyWaste += FloorKoma(Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyWaste")), 4)
                    QtyTotal += FloorKoma(Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyTotal")), 4)
                    'Qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
                    'QtyWaste += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyWaste"))
                    'QtyTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyTotal"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    'Qty = GetTotalSum(ViewState("Dt2"), "Qty")
                    'QtyWaste = GetTotalSum(ViewState("Dt2"), "QtyWaste")
                    'QtyTotal = GetTotalSum(ViewState("Dt2"), "QtyTotal")
                    e.Row.Cells(3).Text = "Total :"
                    ' for the Footer, display the running totals
                    e.Row.Cells(4).Text = FormatFloat(Qty, 4)
                    e.Row.Cells(8).Text = FormatFloat(QtyWaste, 4)
                    e.Row.Cells(9).Text = FormatFloat(QtyTotal, 4)
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt2 Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            'dr = ViewState("Dt2").Select("ItemNo = " + QuotedStr(GVR.Cells(3).Text))
            dr = ViewState("Dt2").Select("BOMNo = " + QuotedStr(tbBOMNo.Text) + " AND Product = " + QuotedStr(tbProductFG.Text) + " AND Process = " + QuotedStr(lbProcessCode.Text) + " AND Material = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            'BindGridDt2(ViewState("Dt2"), GridDt2)

            Dim drow As DataRow()
            If ViewState("Dt2") Is Nothing Then
                BindDataDt2(ViewState("TransNmbr"))
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                drow = ViewState("Dt2").Select("Process = " + QuotedStr(lbProcessCode.Text))
                If drow.Length > 0 Then
                    BindGridDt2(drow.CopyToDataTable, GridDt2)
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
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(tbBOMNo.Text, tbProductFG.Text, lbProcessCode.Text, GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyDt2.TextChanged
        Try
            tbQtyTotal.Text = FormatFloat(CFloat(tbQtyDt2.Text) + CFloat(tbQtyWaste.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbQtyDt2_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt 2 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt2") = "Edit" Then                
                Row = ViewState("Dt2").Select("BOMNo = " + QuotedStr(tbBOMNo.Text) + " AND Product = " + QuotedStr(tbProductFG.Text) + " AND Process = " + QuotedStr(lbProcessCode.Text) + " AND Material = " + QuotedStr(tbMaterialCode.Text))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row("MaterialAlt") = tbMaterialAltCode.Text
                Row("MaterialAlt_Name") = tbMaterialAltName.Text
                Row("Qty") = FormatFloat(tbQtyDt2.Text, 4)
                Row("QtyWaste") = FormatFloat(tbQtyWaste.Text, 4)
                Row("QtyTotal") = FormatFloat(tbQtyTotal.Text, 4)
                Row.EndEdit()
            Else

            End If
            
            Dim drow As DataRow()
            If ViewState("Dt2") Is Nothing Then
                BindDataDt2(ViewState("TransNmbr"))
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                drow = ViewState("Dt2").Select("Process = " + QuotedStr(lbProcessCode.Text))
                If drow.Length > 0 Then
                    BindGridDt2(drow.CopyToDataTable, GridDt2)
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
            'BindGridDt2(ViewState("Dt2"), GridDt2)
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt 2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnReffNmbr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReffNmbr.Click
        Dim ResultField As String

        Try
            Session("Result") = Nothing
            'Session("filter") = "SELECT * FROM V_PDWOReff WHERE Type = " + QuotedStr(ddlReffType.SelectedValue)
            Session("filter") = "SELECT A.*, COALESCE(B.BOM_No,'') AS BOM_No FROM V_PDWOReff A LEFT OUTER JOIN V_PDBOMEffective B ON A.Product = B.Product WHERE " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " BETWEEN B.Start_Effective AND B.End_Effective AND A.Type = " + QuotedStr(ddlReffType.SelectedValue) + " AND A.WorkCtr = " + QuotedStr(ddlWorkCtr.SelectedValue)
            ResultField = "Reference, Product, Product_Name, Specification, Qty, Unit, BOM_No"
            ViewState("Sender") = "btnReffNmbr"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnReffNmbr error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlReffType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReffType.SelectedIndexChanged
        Try
            tbReffNmbr.Text = ""
            tbProductFG.Text = ""
            tbProductNameFG.Text = ""
            tbSpecificationFG.Text = ""
            tbBOMNo.Text = ""
            tbQtyFG.Text = "0"
            tbUnitFG.Text = ""
            tbQtyRoll.Text = "0"
            tbQtyM2.Text = "0"
        Catch ex As Exception
            Throw New Exception("ddlReffType_SelectedIndexChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim dt As New DataTable
        Dim drResult, dr As DataRow
        Dim i As Integer = 1

        Try
            If Not CekHd() Then
                Exit Sub
            End If

            dt = SQLExecuteQuery("EXEC S_PDWOGetDtProcess " + QuotedStr(tbBOMNo.Text) + ", " + tbQtyFG.Text.Replace(",", ""), ViewState("DBConnection").ToString).Tables(0)
            For Each drResult In dt.Rows
                If CekExistData(ViewState("Dt"), "Process", drResult("Process")) = False Then
                    dr = ViewState("Dt").NewRow
                    dr("ItemNo") = i
                    dr("LotNo") = ""
                    dr("Process") = drResult("Process")
                    dr("Process_Name") = TrimStr(drResult("ProcessName").ToString)
                    dr("Product") = TrimStr(drResult("ProductOutput").ToString)
                    dr("Product_Name") = TrimStr(drResult("ProductOutputName").ToString)
                    dr("Specification") = drResult("Specification")
                    dr("Qty") = FormatNumber(drResult("QtyOutput"), ViewState("DigitQty"))
                    dr("Unit") = drResult("UnitOutput")
                    dr("Formula") = drResult("FormulaNo")
                    dr("FormulaName") = drResult("FormulaName")
                    dr("ProductionDate") = tbDate.SelectedDate
                    dr("DODate") = DBNull.Value  'tbDate.SelectedDate
                    dr("OutputType") = drResult("OutputType")
                    ViewState("Dt").Rows.Add(dr)
                End If
                i = i + 1
            Next


            Dim dt2 As New DataTable
            Dim drResult2, dr2 As DataRow
            Dim drow2 As DataRow

            For Each drow2 In ViewState("Dt2").rows
                drow2.Delete()
            Next

            'lbStatus.Text = "EXEC S_PDWOGetDtMaterial " + QuotedStr(tbBOMNo.Text) + "," + (CFloat(tbQtyFG.Text)).ToString.Replace(",", "") + "," + (CFloat(tbWaste.Text)).ToString.Replace(",", "")
            'Exit Sub
            dt2 = SQLExecuteQuery("EXEC S_PDWOGetDtMaterial " + QuotedStr(tbBOMNo.Text) + "," + (CFloat(tbQtyFG.Text)).ToString.Replace(",", "") + "," + (CFloat(tbWaste.Text)).ToString.Replace(",", ""), ViewState("DBConnection").ToString).Tables(0)

            For Each drResult2 In dt2.Rows
                If CekExistData(ViewState("Dt2"), "Process, Material", drResult2("Process") + "|" + drResult2("Material")) = False Then
                    dr2 = ViewState("Dt2").NewRow
                    dr2("BOMNo") = tbBOMNo.Text
                    dr2("Product") = tbProductFG.Text
                    dr2("ProductFG_Name") = tbProductNameFG.Text
                    dr2("Process") = drResult2("Process")
                    dr2("Process_Name") = drResult2("ProcessName")
                    dr2("Material") = drResult2("Material")
                    dr2("Material_Name") = drResult2("ProductName")
                    dr2("Specification") = drResult2("Specification")
                    dr2("Qty") = FormatFloat(drResult2("Qty"), 4)
                    dr2("Unit") = drResult2("Unit")
                    dr2("QtyWaste") = FormatFloat(drResult2("QtyWaste"), 4)
                    dr2("QtyTotal") = FormatFloat(drResult2("QtyTotal"), 4)
                    ViewState("Dt2").Rows.Add(dr2)
                End If
            Next

            BindGridDt(ViewState("Dt"), GridDt)
            BindGridDt2(ViewState("Dt2"), GridDt2)
            EnableHd(ViewState("Dt").Rows.count = 0)
        Catch ex As Exception
            lbStatus.Text = "Btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMaterialAlt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterialAlt.Click
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String

        Try
            SQLString = "EXEC S_PDWOGetMaterialAlt " + QuotedStr(tbBOMNo.Text) + " , " + QuotedStr(lbProcessCode.Text) + " , " + QuotedStr(tbMaterialCode.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMaterialAltCode.Text = Dr("MaterialAltCode")
                tbMaterialAltName.Text = Dr("MaterialAltName")
            Else
                tbMaterialAltCode.Text = ""
                tbMaterialAltName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("btnMaterial_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterial.Click
        Dim ResultField As String

        Try
            Session("Result") = Nothing
            'Session("filter") = "SELECT * FROM V_PDWOReff WHERE Type = " + QuotedStr(ddlReffType.SelectedValue)
            Session("filter") = "Exec S_PDWOGetMaterial " + QuotedStr(tbProductFG.Text) + "," + QuotedStr(tbMaterialCode.Text)
            ResultField = "ProductCode,ProductName"
            ViewState("Sender") = "btnMaterial"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnMaterial_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnResMaterialAlt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResMaterialAlt.Click
        Try
            tbMaterialAltCode.Text = ""
            tbMaterialAltName.Text = ""
        Catch ex As Exception
            Throw New Exception("btnMaterialAlt_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlWorkCtr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWorkCtr.SelectedIndexChanged
        Try
            ddlReffType.SelectedValue = "SO"
            tbReffNmbr.Text = ""
            tbProductFG.Text = ""
            tbProductNameFG.Text = ""
            tbSpecificationFG.Text = ""
            tbUnitFG.Text = ""
            tbQtyM2.Text = "0"
            tbQtyRoll.Text = "0"
            tbQtyFG.Text = "0"
            tbBOMNo.Text = ""
        Catch ex As Exception
            Throw New Exception("ddlWorkCtr_SelectedIndexChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMachine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMachine.Click
        Dim ResultField As String
        Try           
            Session("Result") = Nothing
            Session("filter") = "SELECT MachineCode, MachineName, MachineGroup, Specification, Unit, UserId, UserDate FROM MsMachine"
            ResultField = "MachineCode, MachineName, MachineGroup, Specification, Unit"
            ViewState("Sender") = "btnMachine"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnMachine_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbMachine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMachine.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT MachineCode, MachineName, MachineGroup, Specification, Unit, UserId, UserDate FROM MsMachine WHERE MachineCode = " + QuotedStr(tbMachine.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMachine.Text = Dr("MachineCode")
                tbMachineName.Text = Dr("MachineName")
            Else
                tbMachine.Text = ""
                tbMachineName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbMachine_TextChanged error: " + ex.ToString)
        End Try

    End Sub
End Class
