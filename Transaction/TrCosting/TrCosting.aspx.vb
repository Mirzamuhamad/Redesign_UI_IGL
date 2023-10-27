Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrCosting_TrCosting
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Private Function GetStringHd(ByVal UserId As String) As String
        Return "SELECT A.* FROM V_PRCostingHd A "
    End Function
    'Protected GetStringHd As String = "SELECT A.* FROM V_MKReqReturHd A INNER JOIN VMsDeptUser B ON A.Department = B.Department WHERE B.UserId = '" + ViewState("UserId").ToString + "'" '+ QuotedStr(ViewState("UserId").ToString)
    'Protected GetStringHd As String = "SELECT A.* FROM V_MKReqReturHd A INNER JOIN VMsDeptUser B ON A.Department = B.Department "

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
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
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnProduct" Then
                    BindToText(tbProductCode, Session("Result")(0).ToString)
                    BindToText(tbProductName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnCosting" Then
                    BindToText(tbCostingCode, Session("Result")(1).ToString)
                    BindToText(tbCostingName, Session("Result")(2).ToString)
                    BindToText(tbQty, Session("Result")(3).ToString)
                    BindToDropList(ddlUnit, Session("Result")(4).ToString)
                    tbPrice.Text = FormatFloat(GetCostingPrice(tbEffDate.SelectedDate, ddlCurr.SelectedValue, tbRate.Text, ddlCostingType.SelectedValue, tbCostingCode.Text, ddlSheet.SelectedValue, ViewState("DBConnection")), ViewState("DigitQty"))
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
            FillCombo(ddlUnit, "SELECT * FROM V_MsUnitForCosting", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlSheet, "SELECT Sheet, SheetName FROM MsSheet ", True, "Sheet", "SheetName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))


            tbQty.Attributes.Add("OnBlur", "hitungtotal(); setformatdt();")
            tbPrice.Attributes.Add("OnBlur", "hitungtotal(); setformatdt();")
            tbWaste.Attributes.Add("OnBlur", "hitungtotal(); setformatdt();")
            tbTotal.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal.Attributes.Add("ReadOnly", "True")
            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbWaste.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            DT = BindDataTransaction(GetStringHd(ViewState("UserId").ToString), StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * From V_PRCostingDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbProductCode.Enabled = State
            btnProduct.Visible = State
            ddlSheet.Enabled = State
            ddlCurr.Enabled = State
            tbRate.Enabled = State
            tbEffDate.Enabled = State
            btnGetData.Visible = State
            ddlTypeGetData.Visible = State
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
            If ViewState("Costing") = ddlCostingType.SelectedValue + "|" + tbCostingCode.Text Then
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
                tbCode.Text = GetAutoNmbr("CST", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlSheet.SelectedValue, ViewState("DBConnection").ToString)
                CekTrans = SQLExecuteScalar("SELECT COUNT(TransNmbr) FROM PROCostingHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Standart Costing No. " + tbCode.Text + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO PROCostingHd (TransNmbr, Status, TransDate, " + _
                "Product, Sheet, Currency, ForexRate, EffectiveDate, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'," + _
                QuotedStr(tbProductCode.Text) + "," + QuotedStr(ddlSheet.SelectedValue) + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                QuotedStr(Format(tbEffDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                ViewState("TransNmbr") = tbCode.Text
            Else
                SQLString = "UPDATE PROCostingHd SET Product = " + QuotedStr(tbProductCode.Text) + _
                ", Sheet = " + QuotedStr(ddlSheet.SelectedValue) + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + _
                ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", EffectiveDate = " + QuotedStr(Format(tbEffDate.SelectedValue, "yyyy-MM-dd")) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, CostingType, CostingCode, Qty, Unit, Price, Waste, Total " + _
                                         " FROM PROCostingDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' ", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE PROCostingDt SET CostingType = @CostingType, CostingCode = @CostingCode, " + _
            "Qty = @Qty, Unit = @Unit, Price = @Price, Waste = @Waste, Total = @Total " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND CostingType = @OldCostingType AND CostingCode = @OldCostingCode", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@CostingType", SqlDbType.VarChar, 20, "CostingType")
            Update_Command.Parameters.Add("@CostingCode", SqlDbType.VarChar, 20, "CostingCode")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 22, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@Price", SqlDbType.Float, 22, "Price")
            Update_Command.Parameters.Add("@Waste", SqlDbType.Float, 18, "Waste")
            Update_Command.Parameters.Add("@Total", SqlDbType.Float, 22, "Total")


            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldCostingType", SqlDbType.VarChar, 20, "CostingType")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldCostingCode", SqlDbType.VarChar, 20, "CostingCode")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PROCostingDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND CostingType = @CostingType AND CostingCode = @CostingCode", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@CostingType", SqlDbType.VarChar, 20, "CostingType")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@CostingCode", SqlDbType.VarChar, 20, "CostingCode")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PROCostingDt")

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
            ViewState("TransNmbr") = Now.Year.ToString
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
            tbDate.SelectedDate = ViewState("ServerDate")  'Now.Date
            tbProductCode.Text = ""
            tbProductName.Text = ""
            ddlSheet.SelectedValue = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "0"
            tbEffDate.SelectedDate = ViewState("ServerDate")  'Now.Date
            tbRemark.Text = ""

            'tbRate.Text = FormatFloat(GetCurrRate("USD", tbDate.SelectedDate, ViewState("DBConnection").ToString), ViewState("DigitQty"))
            tbRate.Text = SQLExecuteScalar("Select TOP 1 dbo.FormatFloat(CurrRate,2) from MsCurrRate WHERE CurrCode IN ('USD','US$') and CurrDate <= GetDate() AND CurrRate > 0 ORDER BY CurrDate DESC ", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbCostingCode.Text = ""
            tbCostingName.Text = ""
            tbQty.Text = "0"
            ddlUnit.SelectedValue = ""
            tbPrice.Text = "0"
            tbWaste.Text = "0"
            tbTotal.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub


    Function CekHd() As Boolean
        Try
            If tbProductName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Product must have value. ")
                tbProductCode.Focus()
                Return False
            End If
            If ddlSheet.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Sheet must have value")
                ddlSheet.Focus()
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

                If Dr("CostingName").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Costing Must Have Value")
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
                If CFloat(Dr("Price").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Waste").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Waste (%) Must Have Value")
                    Return False
                End If
                
            Else
                If tbCostingName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Costing Must Have Value")
                    tbCostingCode.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Warehouse Must Have Value")
                    ddlUnit.Focus()
                    Return False
                End If
                If CFloat(tbPrice.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    tbPrice.Focus()
                    Return False
                End If
                If CFloat(tbWaste.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Waste (%) Must Have Value")
                    tbWaste.Focus()
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
                    ViewState("Status") = GVR.Cells(3).Text
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
                ElseIf DDL.SelectedValue = "Copy New" Then
                    MovePanel(PnlHd, pnlInput)
                    pnlDt.Visible = True
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0

                    CopyDataDt(ViewState("TransNmbr"))
                    FillTextBoxHd(ViewState("TransNmbr"))

                    ViewState("StateHd") = "Insert"
                    ModifyInput2(True, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = False
                    tbCode.Text = ""
                    tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
                    tbEffDate.SelectedDate = ViewState("ServerDate")
                    EnableHd(True)
                ElseIf DDL.SelectedValue = "Delete" Then
                    Dim SqlString, Result As String

                    If Not GVR.Cells(3).Text = "H" Then
                        lbStatus.Text = MessageDlg("Data Must be Hold Before Deleted")
                        Exit Sub
                    End If

                    SqlString = "Declare @A VarChar(255) EXEC S_MKReqReturDelete " + QuotedStr(GVR.Cells(2).Text) + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "

                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                    'SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                    BindData(Session("AdvanceFilter"))
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_PDCostingForm " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormPDCosting.frx"
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


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("CostingType+'|'+CostingCode = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text))
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
            ViewState("Costing") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text 'Costing Type | Costing Code

            FillTextBoxDt(ViewState("Costing"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            'tbProduct.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Taon As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd(ViewState("UserId").ToString), "TransNmbr = " + QuotedStr(Taon), ViewState("DBConnection").ToString)
            tbCode.Text = Taon
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbCode, Dt.Rows(0)("TransNmbr").ToString)
            BindToText(tbProductCode, Dt.Rows(0)("Product").ToString)
            BindToText(tbProductName, Dt.Rows(0)("Product_Name").ToString)
            BindToDropList(ddlSheet, Dt.Rows(0)("Sheet").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToDate(tbEffDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("CostingType+'|'+CostingCode = " + QuotedStr(Product))

            If Dr.Length > 0 Then
                BindToDropList(ddlCostingType, Dr(0)("CostingType").ToString)
                BindToText(tbCostingCode, Dr(0)("CostingCode").ToString)
                BindToText(tbCostingName, Dr(0)("CostingName").ToString)
                tbQty.Text = FormatFloat(Dr(0)("Qty").ToString, ViewState("DigitQty"))
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                tbPrice.Text = FormatFloat(Dr(0)("Price").ToString, ViewState("DigitQty"))
                tbWaste.Text = FormatFloat(Dr(0)("Waste").ToString, ViewState("DigitQty"))
                tbTotal.Text = FormatFloat(Dr(0)("Total").ToString, ViewState("DigitQty"))                
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindGridDtExtended()
        Try
            BindGridDt(ViewState("Dt"), GridDt)
            If GetCountRecord(ViewState("Dt")) > 0 Then
                GridDt.Columns(0).Visible = True
            Else
                GridDt.Columns(0).Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("BindGridDtExtended Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            pnlDt.Visible = True

            ModifyInput2(True, pnlInput, pnlDt, GridDt)
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

                    Result = ExecSPCommandGo(ActionValue, "S_MKReqRetur", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

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
            ddlCostingType.Focus()
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
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("CostingType+'|'+CostingCode = " + QuotedStr(ddlCostingType.SelectedValue + "|" + tbCostingCode.Text))

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                Row = ViewState("Dt").Select("CostingType+'|'+CostingCode = " + QuotedStr(ViewState("Costing")))(0)

                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("CostingType") = ddlCostingType.SelectedValue
                Row("CostingCode") = tbCostingCode.Text
                Row("CostingName") = tbCostingName.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row("Price") = tbPrice.Text
                Row("Waste") = tbWaste.Text            
                Row("Total") = tbTotal.Text                
                Row.EndEdit()
                ViewState("Costing") = Nothing
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
                dr("CostingType") = ddlCostingType.SelectedValue
                dr("CostingCode") = tbCostingCode.Text
                dr("CostingName") = tbCostingName.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = ddlUnit.SelectedValue
                dr("Price") = tbPrice.Text
                dr("Waste") = tbWaste.Text
                dr("Total") = tbTotal.Text
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
            Session("Filter") = "SELECT Product_Code, Product_Name FROM VMsProduct WHERE Fg_Active = 'Y' AND FgProduce = 'Y' "
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Product_Code, Product_Name"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name FROM VMsProduct WHERE Fg_Active = 'Y' AND FgProduce = 'Y' AND Product_Code = " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
            End If
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnCosting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCosting.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT CostingType, CostingCode, CostingName, Qty, Unit FROM V_MsCosting WHERE CostingType = " + QuotedStr(ddlCostingType.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "CostingType, CostingCode, CostingName, Qty, Unit "
            ViewState("Sender") = "btnCosting"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnCosting Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCostingCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCostingCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("SELECT CostingType, CostingCode, CostingName, Qty, Unit FROM V_MsCosting WHERE CostingType = " + QuotedStr(ddlCostingType.SelectedValue) + " AND CostingCode = " + QuotedStr(tbCostingCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCostingCode.Text = Dr("CostingCode")
                tbCostingName.Text = Dr("CostingName")
                tbQty.Text = Dr("Qty")
                ddlUnit.SelectedValue = Dr("Unit")
            Else
                tbCostingCode.Text = ""
                tbCostingName.Text = ""
                tbQty.Text = "0"
                ddlUnit.SelectedValue = ""
            End If
            tbCostingCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbCostingCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try
            ' tbRate.Text = FormatFloat(GetCurrRate("USD", tbDate.SelectedDate, ViewState("DBConnection").ToString), ViewState("DigitRate"))
        Catch ex As Exception
            Throw New Exception("tbDate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCostingType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCostingType.TextChanged
        Try
            Cleardt()
        Catch ex As Exception
            Throw New Exception("ddlCostingType_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Function GetCostingPrice(ByVal tgl As DateTime, ByVal currency As String, ByVal rate As String, ByVal costtype As String, ByVal costcode As String, ByVal sheet As String, ByVal DBConnection As String) As Double
        Dim Dr As DataRow
        Dim DT As DataTable
        Try
            DT = SQLExecuteQuery("EXEC S_PDCostingGetPrice " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + "," + QuotedStr(currency) + "," + rate.Replace(",", "") + "," + QuotedStr(costtype) + "," + QuotedStr(costcode) + "," + QuotedStr(sheet), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                Return Dr("Price")
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("GetCostingPrice Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Try
            Dim dt As New DataTable
            Dim drResult, dr As DataRow
            Dim ExistRow As DataRow()
            Dim CostingCode As String
            dt = SQLExecuteQuery("EXEC S_PDCostingGetDt " + QuotedStr(Format(tbEffDate.SelectedValue, "yyyyMMdd")) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + QuotedStr(tbProductCode.Text) + "," + QuotedStr(ddlTypeGetData.SelectedValue), ViewState("DBConnection").ToString).Tables(0)

            For Each drResult In dt.Rows

                CostingCode = drResult("Material").ToString.Trim
                ExistRow = ViewState("Dt").Select("CostingType = 'Material' AND CostingCode = " + QuotedStr(CostingCode))
                If ExistRow.Count = 0 Then
                    dr = ViewState("Dt").NewRow
                    dr("CostingType") = "Material"
                    dr("CostingCode") = drResult("Material")
                    dr("CostingName") = TrimStr(drResult("MaterialName").ToString)
                    dr("Qty") = drResult("Qty")
                    dr("Unit") = drResult("Unit")
                    dr("Price") = drResult("Price")
                    dr("Waste") = 0
                    dr("Total") = drResult("Total")
                    ViewState("Dt").Rows.Add(dr)
                End If

            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(ViewState("Dt").Rows.count = 0)
        Catch ex As Exception
            lbStatus.Text = "btnGetData Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub CopyDataDt(ByVal Nmbr As String)
        Try
            Dim dt, dtSource As New DataTable
            dtSource = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(""), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            For Each R In dtSource.Rows
                Dim dr As DataRow               
                dr = ViewState("Dt").NewRow
                dr("CostingType") = R("CostingType")
                dr("CostingCode") = R("CostingCode")
                dr("CostingName") = TrimStr(R("CostingName").ToString)
                dr("Qty") = R("Qty")
                dr("Unit") = R("Unit")
                dr("Price") = R("Price")
                dr("Waste") = 0
                dr("Total") = R("Total")
                ViewState("Dt").Rows.Add(dr)
            Next
            'GridDt.Columns(6).Visible = ViewState("Status") = "P"
            BindGridDt(ViewState("Dt"), GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
End Class