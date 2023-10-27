Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrMKReqRetur_TrMKReqRetur
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Private Function GetStringHd(ByVal UserId As String) As String
        Return "SELECT DISTINCT A.TransNmbr, A.Nmbr, A.TransDate, A.Status, A.Fg_Report, A.Customer, A.Customer_Name, A.Attn, A.CustReturNo, A.Remark, A.UserPrep, A.DatePrep, A.UserAppr, A.DateAppr, A.Department, A.DepartmentName FROM V_MKReqReturHd A INNER JOIN VMsDeptUser B ON A.Department = B.Department WHERE B.UserId = " + QuotedStr(UserId)
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
                If ViewState("Sender") = "btnCustomer" Then
                    BindToText(tbCustCode, Session("Result")(0).ToString)
                    BindToText(tbCustName, Session("Result")(1).ToString)
                    BindToText(tbAttn, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnSJNo" Then
                    BindToText(tbSJNo, Session("Result")(0).ToString)
                    BindToText(tbSONo, Session("Result")(1).ToString)
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
                            dr("Remark") = TrimStr(drResult("Remark").ToString)
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
                If ViewState("Sender") = "btnProduct" Then
                    'ResultField = "Product, ProductName, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, Remark "
                    BindToText(tbProduct, Session("Result")(0).ToString)
                    BindToText(tbProductName, Session("Result")(1).ToString)
                    BindToText(tbQtyWrhs, Session("Result")(2).ToString)
                    BindToDropList(ddlUnitWrhs, Session("Result")(3).ToString)
                    BindToText(tbQtyM2, Session("Result")(4).ToString)
                    BindToText(tbQtyRoll, Session("Result")(5).ToString)
                    BindToText(tbRemarkDt, Session("Result")(6).ToString)
                End If
                If ViewState("Sender") = "btnRRVehicle" Then
                    tbRRVehicleCode.Text = Session("Result")(0).ToString
                    tbRRVehicleName.Text = Session("Result")(1).ToString
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If
            If (Not ViewState("ProductClose") Is Nothing) And (Not ViewState("SJClose") Is Nothing) Then
                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    sqlstring = "Declare @A VarChar(255) EXEC S_MKReqReturClosing " + QuotedStr(tbCode.Text) + "," + QuotedStr(ViewState("ProductClose").ToString) + "," + QuotedStr(ViewState("SJClose").ToString) + "," + QuotedStr(HiddenRemarkClose.Value) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                    If result.Length > 2 Then
                        lbStatus.Text = MessageDlg(result)
                    Else
                        BindDataDt(ViewState("TransNmbr"))
                    End If
                End If
                ViewState("ProductClose") = Nothing
                ViewState("SJClose") = Nothing
                HiddenRemarkClose.Value = ""
                GridDt.Columns(0).Visible = False
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
            FillCombo(ddlUnitWrhs, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlDept, "SELECT Department, DepartmentName FROM VMsDeptUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "Department", "DepartmentName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))


            tbQtyM2.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyRoll.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyWrhs.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyM2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyRoll.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyWrhs.Attributes.Add("OnKeyDown", "return PressNumeric();")

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
        Return "SELECT * From V_MKReqReturDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'ddlReport.Enabled = State And ViewState("StateHd") = "Insert"
            tbCustCode.Enabled = State
            btnCust.Visible = State
            tbAttn.Enabled = State
            ddlDept.Enabled = State
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
            If ViewState("Product") = tbProduct.Text + "|" + tbSJNo.Text Then
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
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("SRQ", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlDept.SelectedValue, ViewState("DBConnection").ToString)

                CekTrans = SQLExecuteScalar("SELECT COUNT(TransNmbr) FROM STCSJHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Sales Retur Request No. " + tbCode.Text + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MKTReqReturHd (TransNmbr, Status, TransDate, FgReport," + _
                "Customer, Attn, Department, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'," + QuotedStr("Y") + "," + _
                QuotedStr(tbCustCode.Text) + "," + QuotedStr(tbAttn.Text) + "," + QuotedStr(ddlDept.SelectedValue) + ", " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                ViewState("TransNmbr") = tbCode.Text
            Else
                SQLString = "UPDATE MKTReqReturHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                "Customer = " + QuotedStr(tbCustCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + ", Department = " + QuotedStr(ddlDept.SelectedValue) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, SJNo, SONo, Product, Qty, Unit, QtyM2, QtyRoll, Reason, Remark " + _
                                         " FROM MKTReqReturDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' ", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE MKTReqReturDt SET SJNo = @SJNo, SONo = @SONo, " + _
            "Product = @Product, Qty = @Qty, Unit = @Unit, " + _
            "QtyM2 = @QtyM2, QtyRoll = @QtyRoll, Reason = @Reason, Remark = @Remark " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND SJNo = @OldSJNo AND Product = @OldProduct", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@SJNo", SqlDbType.VarChar, 20, "SJNo")
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@SONo", SqlDbType.VarChar, 20, "SONo")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@QtyM2", SqlDbType.Float, 18, "QtyM2")
            Update_Command.Parameters.Add("@QtyRoll", SqlDbType.Float, 18, "QtyRoll")
            Update_Command.Parameters.Add("@Reason", SqlDbType.VarChar, 255, "Reason")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldSJNo", SqlDbType.VarChar, 20, "SJNo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM MKTReqReturDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND SJNo = @SJNo AND Product = @Product", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@SJNo", SqlDbType.VarChar, 20, "SJNo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("MKTReqReturDt")

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
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbAttn.Text = ""
            ddlDept.SelectedValue = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbSJNo.Text = ""
            tbSONo.Text = ""
            tbProduct.Text = ""
            tbProductName.Text = ""
            tbQtyM2.Text = "0"
            tbQtyRoll.Text = "0"
            tbQtyWrhs.Text = "0"
            tbReason.Text = ""
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub


    Function CekHd() As Boolean
        Try
            If tbCustName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value. ")
                tbCustCode.Focus()
                Return False
            End If
            If ddlDept.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                ddlDept.Focus()
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
            Else
                If tbProduct.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProduct.Focus()
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
                If CFloat(tbQtyWrhs.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    tbQtyWrhs.Focus()
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
                        GridDt.Columns(1).Visible = False
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
                        Session("SelectCommand") = "EXEC S_MKFormReqRetur " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormMKReqRetur.frx"
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
                If ViewState("Status") = "P" Then
                    Dim GVR As GridViewRow
                    GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                    ViewState("ProductClose") = GVR.Cells(4).Text
                    ViewState("SJClose") = GVR.Cells(2).Text
                    AttachScript("closing();", Page, Me.GetType)
                Else
                    lbStatus.Text = MessageDlg("Status Sales Retur Request is not Post, cannot close detail")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("Product+'|'+SJNo = " + QuotedStr(GVR.Cells(4).Text + "|" + GVR.Cells(2).Text))
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
            ViewState("Product") = GVR.Cells(4).Text + "|" + GVR.Cells(2).Text 'Product Code | SJ No
            
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
            Dt = BindDataTransaction(GetStringHd(ViewState("UserId").ToString), "TransNmbr = " + QuotedStr(Taon), ViewState("DBConnection").ToString)
            tbCode.Text = Taon
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbCode, Dt.Rows(0)("TransNmbr").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product+'|'+SJNo = " + QuotedStr(Product))

            If Dr.Length > 0 Then
                BindToText(tbSJNo, Dr(0)("SJNo").ToString)
                BindToText(tbSONo, Dr(0)("SONo").ToString)
                BindToText(tbProduct, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                tbQtyWrhs.Text = FormatFloat(Dr(0)("Qty").ToString, ViewState("DigitQty"))
                BindToDropList(ddlUnitWrhs, Dr(0)("Unit").ToString)
                tbQtyM2.Text = FormatFloat(Dr(0)("QtyM2").ToString, ViewState("DigitQty"))
                tbQtyRoll.Text = FormatFloat(Dr(0)("QtyRoll").ToString, ViewState("DigitQty"))
                BindToText(tbReason, Dr(0)("Reason").ToString)
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
            SQL = "EXEC S_MKReqReturGetSJDt " + QuotedStr(tbSJNo.Text) + ", " + QuotedStr(tbProduct.Text.Trim)

            Dt = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProduct.Text = Dr("Product")
                tbProductName.Text = TrimStr(Dr("ProductName").ToString)
                ddlUnitWrhs.SelectedValue = Dr("UnitWrhs")
                tbQtyWrhs.Text = FormatFloat(Dr("QtyWrhs"), ViewState("DigitQty"))
                tbQtyM2.Text = FormatFloat(Dr("QtyM2"), ViewState("DigitQty"))
                tbQtyRoll.Text = FormatFloat(Dr("QtyRoll"), ViewState("DigitQty"))                
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                ddlUnitWrhs.SelectedValue = ""
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
            If GetCountRecord(ViewState("Dt")) > 0 Then
                GridDt.Columns(0).Visible = True
                GridDt.Columns(1).Visible = True
            Else
                GridDt.Columns(0).Visible = False
                GridDt.Columns(1).Visible = False
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
            btnSJNo.Focus()
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
            ExistRow = ViewState("Dt").Select("Product+'|'+SJNo = " + QuotedStr(tbProduct.Text + "|" + tbSJNo.Text))

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                Row = ViewState("Dt").Select("Product+'|'+SJNo = " + QuotedStr(ViewState("Product")))(0)

                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("Product") = tbProduct.Text
                Row("Product_Name") = tbProductName.Text
                Row("Qty") = CFloat(tbQtyWrhs.Text)
                Row("Unit") = ddlUnitWrhs.SelectedValue
                If tbQtyM2.Text <> "" Then
                    Row("QtyM2") = CFloat(tbQtyM2.Text)
                Else
                    Row("QtyM2") = 0
                End If
                If tbQtyRoll.Text <> "" Then
                    Row("QtyRoll") = CFloat(tbQtyRoll.Text)
                Else
                    Row("QtyRoll") = 0
                End If
                Row("Reason") = tbReason.Text
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
                dr("SJNo") = tbSJNo.Text
                dr("SONo") = tbSONo.Text
                dr("Product") = tbProduct.Text
                dr("Product_Name") = tbProductName.Text
                dr("Qty") = CFloat(tbQtyWrhs.Text)
                dr("Unit") = ddlUnitWrhs.SelectedValue
                If tbQtyM2.Text <> "" Then
                    dr("QtyM2") = CFloat(tbQtyM2.Text)
                Else
                    dr("QtyM2") = 0
                End If
                If tbQtyRoll.Text <> "" Then
                    dr("QtyRoll") = CFloat(tbQtyRoll.Text)
                Else
                    dr("QtyRoll") = 0
                End If
                dr("Reason") = tbReason.Text
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
            Session("Filter") = "EXEC S_MKReqReturGetSJDt " + QuotedStr(tbSJNo.Text) + ", '' "
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Product, ProductName, QtyWrhs, UnitWrhs, QtyM2, QtyRoll, Remark "
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
            Session("filter") = "SELECT * FROM VMsCustomer WHERE FgActive = 'Y'"
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Customer_Code, Customer_Name, Contact_Person"
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
                tbAttn.Text = Dr("Contact_Person")
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                tbAttn.Text = ""
            End If
            tbCustCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub tbQtyWrhs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyWrhs.TextChanged
        Try
            Dim dt As New DataTable

            dt = SQLExecuteQuery("EXEC S_MKReqReturGetQtySJ " + QuotedStr(tbSJNo.Text) + "," + QuotedStr(tbProduct.Text) + "," + (CFloat(tbQtyWrhs.Text)).ToString, ViewState("DBConnection").ToString).Tables(0)
            tbQtyM2.Text = FormatNumber(dt.Rows(0)("QtyM2Req").ToString, ViewState("DigitQty"))
            tbQtyRoll.Text = FormatNumber(dt.Rows(0)("QtyRollReq").ToString, ViewState("DigitQty"))

        Catch ex As Exception
            Throw New Exception("tbQtyWrhs_TextChanged Error : " + ex.ToString)
        End Try

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
            lbQtyWrhs = GVR.FindControl("QtyWrhsEdit")
            lbUnitWrhs = GVR.FindControl("UnitWrhsEdit")
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
            lbQtyWrhs = GVR.FindControl("QtyWrhsEdit")
            lbUnitWrhs = GVR.FindControl("UnitWrhsEdit")
            tbQtyRR = GVR.FindControl("QtyRREdit")
            tbQtyLoss = GVR.FindControl("QtyLossEdit")

            SQLString = "UPDATE STCSJDt SET QtyRR =" + (CFloat(tbQtyRR.Text)).ToString + ", QtyLoss =" + (CFloat(tbQtyLoss.Text)).ToString + _
            " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")) + " AND Product = " + QuotedStr(lbProduct.Text) + " AND Location = " + QuotedStr(lbLocation.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

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
            lbQtyWrhs = GVR.FindControl("QtyWrhsEdit")

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
            lbQtyWrhs = GVR.FindControl("QtyWrhsEdit")

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
            If Trim(Result) <> "" Then
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

    
    Protected Sub btnSJNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSJNo.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT * FROM V_MKReqReturGetSJ WHERE Fg_Report = " + QuotedStr("Y") + " AND Customer_Code = " + QuotedStr(tbCustCode.Text) 'ddlReport.SelectedValue
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "DN_No, SONo"
            ViewState("Sender") = "btnSJNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn SJNo Click Error : " + ex.ToString
        End Try
    End Sub
End Class