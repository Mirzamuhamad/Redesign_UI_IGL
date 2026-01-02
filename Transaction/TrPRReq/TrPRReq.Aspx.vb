Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports System.IO


Partial Class TrPRReq
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "Select DISTINCT A.TransNmbr, A.Nmbr, A.TransDate, A.Status, A.Department, A.DeptName, A.ReffNmbr, A.RequestBy, A.Remark, A.UserPrep, A.DatePrep, A.UserAppr, A.DateAppr, A.SVONo From V_PRCRequestHd A INNER JOIN VMsDepartment B ON A.Department = B.Dept_Code "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lblTitle.Text = dt.Rows(0)("MenuName").ToString
                Session("AdvanceFilter") = ""
                If Not Session("PostNmbr") = Nothing Then
                    tbFilter.Text = Session("PostNmbr")
                    btnSearch_Click(Nothing, Nothing)
                    Session("PostNmbr") = Nothing
                    tbFilter.Text = ""
                End If
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnProd" Then
                    tbProdCode.Text = Session("Result")(0).ToString
                    tbProdName.Text = Session("Result")(1).ToString
                    BindToText(tbSpec, Session("Result")(2).ToString)
                    BindToDropList(ddlUnitWrhs, Session("Result")(3))
                    BindToDropList(ddlUnitReq, Session("Result")(4))
                    tbQtyreq.Text = "0"
                    tbQtyreq_TextChanged(Nothing, Nothing)
                    GetInfo()
                End If

                If ViewState("Sender") = "btnRequest" Then
                    tbRequestBy.Text = Session("Result")(0).ToString
                End If

                If ViewState("Sender") = "btnSvo" Then
                    tbSvoNo.Text = Session("Result")(0).ToString
                End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product_Code")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product_Code")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Qty") = "0"
                            dr("QtyOrder") = "0"
                            dr("RequireDate") = Date.Today
                            dr("Unit") = drResult("Unit")
                            dr("UnitOrder") = drResult("Unit_Order")
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

            If Not ViewState("deletetrans") Is Nothing Then
                Dim Result, ListSelectNmbr, msg, ActionValue, status As String
                Dim Nmbr(100) As String
                Dim j As Integer
                If HiddenRemarkDelete.Value = "true" Then
                    If sender.ID.ToString = "BtnGo" Then
                        ActionValue = ddlCommand.SelectedValue
                    Else
                        ActionValue = ddlCommand2.SelectedValue
                    End If

                    status = CekStatus(ActionValue)
                    ListSelectNmbr = ""
                    msg = ""

                    '3 = status, 2 & 3 = key, 
                    GetListCommand("G|H", GridView1, "3, 2", ListSelectNmbr, Nmbr, msg)

                    If ListSelectNmbr = "" Then Exit Sub

                    For j = 0 To (Nmbr.Length - 1)
                        If Nmbr(j) = "" Then
                            Exit For
                        Else

                            Result = ExecSPCommandGo("Delete", "S_PRRequest", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + "<br />"
                            End If
                        End If
                    Next
                    BindData("TransNmbr in (" + ListSelectNmbr + ")")
                    If msg.Trim <> "" Then
                        lbStatus.Text = MessageDlg(msg)
                    End If

                End If
                ViewState("deletetrans") = Nothing
                HiddenRemarkDelete.Value = ""
                'GridDt.Columns(0).Visible = False
            End If
            If Not ViewState("ProductClose") Is Nothing Then
                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    'lbStatus.Text = "Product '" + ViewState("ProductClose").ToString + "' Remark Close '" + HiddenRemarkClose.Value + "'"
                    'Exit Sub
                    sqlstring = "Declare @A VarChar(255) EXEC S_PRRequestClosing '" + ViewState("Reference") + "', '" + ViewState("ProductClose").ToString + "','" + HiddenRemarkClose.Value + "'," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
                    If result.Length > 2 Then
                        lbStatus.Text = result
                    Else
                        BindDataDt(ViewState("Reference"))
                    End If
                End If
                ViewState("ProductClose") = Nothing
                HiddenRemarkClose.Value = ""
                'GridDt.Columns(0).Visible = False
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
        ViewState("DigitQty") = 0
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub


    Private Sub SetInit()
        FillRange(ddlRange)
        FillCombo(ddlDept, "SELECT Dept_Code, Dept_Name  FROM VMsDepartment ", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
        FillCombo(ddlUnitReq, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
        FillCombo(ddlUnitWrhs, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
        
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
            
        End If
        Me.tbQtyreq.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQtyreq.Attributes.Add("OnBlur", "setformat();")
        Me.tbSpec.Attributes.Add("ReadOnly", "True")

        End Sub

    Private Sub GetInfo()
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_PRReqReturInfoStock " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(tbProdCode.Text.Trim)

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PnlInfo.Visible = True

            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
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
        Return "SELECT * From V_PRCRequestDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtPartial(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCRequestDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
    Public Function ExecSPCekPosting(ByVal ProcName As String, ByVal Nmbr As String, ByVal UserId As String, Optional ByVal Connection As String = "Nothing") As DataTable
        Dim Mycon As New SqlConnection
        Dim DT As DataTable

        Dim PrimaryKey() As String
        PrimaryKey = Nmbr.Split("|")
        Mycon = New SqlConnection(Connection)

        Dim sqlstring As String
        sqlstring = ""
        If PrimaryKey.Length = 1 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 2 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 3 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(UserId)
        End If
        Try
            DT = SQLExecuteQuery(sqlstring, Connection).Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("Exec SP Posting Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        'Dim sqlresult As Object
        Dim sql As New SqlCommand
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


                Session("SelectCommand") = "EXEC S_PRFormPR " + Result + "'"
                'If Result.Trim = "" Then
                '    Session("SelectCommand") = "EXEC S_PRFormPR '''0''' "
                'End If

                'Dim CekResult As String = DirectCast(SQLExecuteScalar(Session("SelectCommand"), ViewState("DBConnection").ToString), String)
                If Result.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Data Selected")
                    Exit Sub
                End If

                If ActionValue = "Print" Then
                    Session("ReportFile") = ".../../../Rpt/FormPR.frx"
               
                End If
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
           
            ElseIf ActionValue = "Delete" Then

                If HiddenRemarkDelete.Value <> "False Value" Then
                    HiddenRemarkDelete.Value = ""
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
                    ViewState("deletetrans") = ListSelectNmbr
                    AttachScript("deletetrans();", Page, Me.GetType)

                End If
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    ElseIf ActionValue = "Post" Then
                       
                        Result = ExecSPCommandGo(ActionValue, "S_PRRequest", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                        'End If
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PRRequest", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlDept.Enabled = State
            
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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)

            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbProdCode.Text Then
                    If CekExistData(ViewState("Dt"), "Product", tbProdCode.Text) Then
                        lbStatus.Text = "Product '" + tbProdCode.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Dim cekada As String
                cekada = SQLExecuteScalar("EXEC S_GetProductUnit  " + QuotedStr(ViewState("DtValue")) + ", " + QuotedStr(ddlUnitReq.SelectedValue), ViewState("DBConnection").ToString)
                If cekada = "" Then
                    lbStatus.Text = MessageDlg("Save failed... Product in Unit Order, Unit Warehouse and Unit Packing NULL ")
                    Exit Sub
                End If

                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProdCode.Text
                Row("ProductName") = tbProdName.Text
                Row("Specification") = tbSpec.Text
                Row("RequireDate") = RequireDate.SelectedDate
                Row("QtyOrder") = FormatFloat(tbQtyreq.Text, ViewState("DigitQty"))
                Row("Qty") = FormatFloat(tbQtyWrhs.Text, ViewState("DigitQty"))
                Row("UnitOrder") = ddlUnitReq.Text
                Row("Unit") = ddlUnitWrhs.Text
                Row("Remark") = tbRemarkDt.Text
                
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Product", tbProdCode.Text) Then
                    lbStatus.Text = "Product '" + tbProdCode.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProdCode.Text
                dr("ProductName") = tbProdName.Text
                dr("Specification") = tbSpec.Text
                dr("RequireDate") = RequireDate.SelectedDate
                dr("QtyOrder") = FormatFloat(tbQtyreq.Text, ViewState("DigitQty"))
                dr("Qty") = FormatFloat(tbQtyWrhs.Text, ViewState("DigitQty"))
                dr("UnitOrder") = ddlUnitReq.Text
                dr("Unit") = ddlUnitWrhs.Text
                dr("Remark") = tbRemarkDt.Text
                
                dr("QtyPO") = CFloat(0)
                ViewState("Dt").Rows.Add(dr)
            End If
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
        Dim Dep2digit As String
        Try
            'If pnlDt.Visible = False Then
            '    lbStatus.Text = "Detail Data must be saved first"
            '    Exit Sub
            'End If
            'Save Hd
            If ViewState("StateHd") = "Insert" Then
                Dep2digit = Right(ddlDept.SelectedValue.ToUpper, 2)
                tbTransNo.Text = GetAutoNmbr("PRQ", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
            
                SQLString = "INSERT INTO PRCRequestHd (TransNmbr, STATUS, Transdate, Department, RequestBy, Remark, SVONo, UserPrep, DatePrep, FgBuffer, RequestType) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + ddlDept.SelectedValue + "', " + QuotedStr(tbRequestBy.Text) + _
                ", '" + tbRemark.Text + "','" + tbSvoNo.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(),'N', ' ' "

                ViewState("Reference") = tbTransNo.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCRequestHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCRequestHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', RequestBy = " + QuotedStr(tbRequestBy.Text) + _
                ", Department = '" + ddlDept.SelectedValue + _
                "', Remark = '" + tbRemark.Text + "',SVONo = '" + tbSvoNo.Text + "', DateAppr = getDate()" + _
                " WHERE TransNmbr = '" + tbTransNo.Text + "'"
            End If
          


            SQLString = ChangeQuoteNull(SQLString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            SQLString = " SELECT TransNmbr, Product, Specification, RequireDate, QtyOrder, UnitOrder, Qty, Unit, Remark FROM PRCRequestDt WHERE TransNmbr = '" & ViewState("Reference") & "' "

            Dim cmdSql As New SqlCommand(SQLString, con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
          
            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PRCRequestDt SET Product = @Product, Specification = @Specification, RequireDate = @RequireDate, QtyOrder = @QtyOrder, UnitOrder = @UnitOrder, Qty = @Qty, Unit = @Unit ," + _
                    " Remark = @Remark " + _
                     "WHERE TransNmbr = " & QuotedStr(ViewState("Reference")) & " AND Product = @OldProduct ", con)

            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Specification", SqlDbType.VarChar, 255, "Specification")
            Update_Command.Parameters.Add("@QtyOrder", SqlDbType.Float, 18, "QtyOrder")
            Update_Command.Parameters.Add("@UnitOrder", SqlDbType.VarChar, 5, "UnitOrder")
            Update_Command.Parameters.Add("@RequireDate", SqlDbType.DateTime, 5, "RequireDate")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PRCRequestDt WHERE TransNmbr = " & QuotedStr(ViewState("Reference")) & " AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCRequestDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            ''save dt
            'Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            'con = New SqlConnection(ConnString)
            'con.Open()
            'Dim cmdSql As New SqlCommand("SELECT TransNmbr, RequireDate, Product, Specification, QtyOrder, UnitOrder, Qty, Unit, Remark FROM PRCRequestDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            'da = New SqlDataAdapter(cmdSql)
            'Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim Dt As New DataTable("PRCRequestDt")

            'Dt = ViewState("Dt")
            'da.Update(Dt)
            'Dt.AcceptChanges()
            'ViewState("Dt") = Dt

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        'Dim Row As DataRow
        Dim ExistRow As DataRow()
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
            tbFilter.Text = tbTransNo.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
            tbRequestBy.Text = ViewState("UserId")
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("Reference") = ""
            ClearHd()
            Cleardt()

            pnlDt.Visible = True
            GridDt.Columns(1).Visible = False
            BindDataDt("")

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
            ddlDept.SelectedValue = ""
            ddlDept.Enabled = True
            tbRequestBy.Text = ""
            tbSvoNo.Text = ""
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbProdCode.Text = ""
            tbProdName.Text = ""
            tbSpec.Text = ""
            tbRemarkDt.Text = ""
            ddlUnitReq.SelectedIndex = -1
            ddlUnitWrhs.SelectedIndex = -1
            tbQtyreq.Text = "0"
            tbQtyWrhs.Text = "0"
            tbQtyWrhs.Enabled = False
            tbQtyreq.Enabled = True
            PnlInfo.Visible = False
            RequireDate.SelectedDate = ViewState("ServerDate")

            
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpand.Click
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim ExistRow As DataRow()
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

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
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
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If

            If ddlDept.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                ddlDept.Focus()
                Return False
            End If

            If tbRequestBy.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("RequestBy must have value")
                tbRequestBy.Focus()
                Return False
            End If
           
            If Len(tbRemark.Text) > 255 Then
                lbStatus.Text = MessageDlg("Remark Max Lenght 255")
                tbRemark.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        'Dim Row As DataRow
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    Return False
                End If

                If Dr("RequireDate").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Require Date Must Have Value")
                    Return False
                End If

                If Len(Dr("Remark").ToString.Trim) > 255 Then
                    lbStatus.Text = MessageDlg("Remark Max Lenght 255")
                    Return False
                End If


                If CFloat(Dr("QtyOrder")) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If

                If CFloat(Dr("Qty")) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    Return False
                End If

            Else

                If tbProdCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    tbProdCode.Focus()
                    Return False
                End If


                If CFloat(tbQtyreq.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    tbQtyreq.Focus()
                    Return False
                End If

                If CFloat(tbQtyWrhs.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    tbQtyWrhs.Focus()
                    Return False
                End If

                If ddlUnitReq.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    ddlUnitReq.Focus()
                    Return False
                End If

                If ddlUnitWrhs.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Warehouse Must Have Value")
                    ddlUnitWrhs.Focus()
                    Return False
                End If


                If Len(tbRemarkDt.Text) > 255 Then
                    lbStatus.Text = MessageDlg("Remark Max Lenght 255")
                    tbRemarkDt.Focus()
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
            FilterName = "Reference, Date, Department, Request By, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), DeptName, RequestBy, Remark"
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
                    ViewState("Status") = GVR.Cells(3).Text
                    If ViewState("Status") = "P" Then
                        GridDt.Columns(1).Visible = True
                    Else
                        GridDt.Columns(1).Visible = False
                    End If

                    GridDt.PageIndex = 0
                    MultiView1.ActiveViewIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
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
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        MultiView1.ActiveViewIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False

                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Or DDL.SelectedValue = "Print 2" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PRFormPR ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(ViewState("UserId").ToString)
                        ''lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        If DDL.SelectedValue = "Print" Then
                            Session("ReportFile") = ".../../../Rpt/FormPR.frx"
                        Else
                            Session("ReportFile") = ".../../../Rpt/FormPR2.frx"
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Schedule" Or DDL.SelectedValue = "Print Schedule 2" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PRFormPRSchedule " + QuotedStr(GVR.Cells(2).Text)
                        If DDL.SelectedValue = "Print Schedule" Then
                            Session("ReportFile") = ".../../../Rpt/FormPRSchedule.frx"
                        Else
                            Session("ReportFile") = ".../../../Rpt/FormPRSchedule2.frx"
                        End If

                        Session("DBConnection") = ViewState("DBConnection")
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status Purchase Request is not Post, cannot close product")
                    Exit Sub
                End If
                If GVR.Cells(11).Text = "Y" Then
                    lbStatus.Text = MessageDlg("Product Closed Already")
                    Exit Sub
                End If
                ViewState("ProductClose") = GVR.Cells(2).Text
                AttachScript("closing();", Page, Me.GetType)
            ElseIf e.CommandName = "Detail" Then
                Dim GVR As GridViewRow
                Dim Product As String
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If GVR.Cells(3).Text = "&nbsp;" Then
                    Exit Sub
                End If
                Product = GVR.Cells(3).Text

                'If GVR.Cells(10).Text = "&nbsp;" Then
                '    lbStatus.Text = "Product " + Product + lbProductName.Text + " not exists have PO"
                '    Exit Sub
                'End If
                'If CFloat(GVR.Cells(10).Text) = 0 Then
                '    lbStatus.Text = "Product " + Product + lbProductName.Text + " not exists have PO"
                '    Exit Sub
                'End If
               
                MultiView1.ActiveViewIndex = 1
           
            ElseIf e.CommandName = "DetailPartial" Then

                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                
                ViewState("DtValue") = GVR.Cells(3).Text


                If GVR.Cells(6).Text = "N" Then
                    lbStatus.Text = MessageDlg("Cannot Detail Require Partial N ")
                    Exit Sub
                End If


              
            
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()

        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)


    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            ViewState("DtValue") = tbProdCode.Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            GetInfo()
            tbProdCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
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
            'newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRequestBy, Dt.Rows(0)("RequestBy").ToString)
            BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbSvoNo, Dt.Rows(0)("SVONo").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                BindToText(tbProdCode, Dr(0)("Product").ToString)
                BindToText(tbProdName, Dr(0)("ProductName").ToString)
                BindToText(tbSpec, Dr(0)("Specification").ToString)
                BindToText(tbQtyreq, CFloat(Dr(0)("QtyOrder").ToString))
                BindToText(tbQtyWrhs, CFloat(Dr(0)("Qty").ToString))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToDate(RequireDate, Dr(0)("RequireDate").ToString)
                BindToDropList(ddlUnitReq, Dr(0)("UnitOrder").ToString)
                BindToDropList(ddlUnitWrhs, Dr(0)("Unit").ToString)
                

                If tbQtyreq.Text = "" Then
                    tbQtyreq.Text = "0"
                End If
                If tbQtyWrhs.Text = "" Then
                    tbQtyWrhs.Text = "0"
                End If

                tbQtyreq.Enabled = True
                ddlUnitReq.Enabled = True


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

    Protected Sub btnProd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProd.Click
        Dim ResultField As String

        Try
            If ddlDept.SelectedValue = "" Then
                Exit Sub
            End If
            Session("filter") = "EXEC S_PRRequestReff " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            ResultField = "Product_Code, Product_Name, specification,Unit, Unit_Order"
            ViewState("Sender") = "btnProd"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProdCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProdCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Product", tbProdCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProdCode.Text = Dr("Product_Code").ToString
                tbProdName.Text = Dr("Product_Name").ToString
                tbQtyreq_TextChanged(Nothing, Nothing)
                BindToText(tbSpec, Dr("Specification").ToString)
                BindToDropList(ddlUnitWrhs, Dr("Unit"))
                BindToDropList(ddlUnitReq, Dr("Unit"))
                
                GetInfo()

            Else
                tbProdCode.Text = ""
                tbProdName.Text = ""
                tbSpec.Text = ""
                tbQtyreq.Text = "0"
                tbQtyreq_TextChanged(Nothing, Nothing)
                PnlInfo.Visible = False

            End If

            tbProdCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb acc Code Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbQtyreq_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyreq.TextChanged
        Try

            If CFloat(tbQtyreq.Text) = 0 Then
                tbQtyWrhs.Text = Cfloat(0)
            Else
                tbQtyWrhs.Text = FormatFloat(FindConvertUnit(tbProdCode.Text, ddlUnitReq.SelectedValue, tbQtyreq.Text, ViewState("DBConnection")).ToString, ViewState("DigitQty"))
            End If

            'If CFloat(tbQtyreq.Text) = 0 Then
            '    tbQtyWrhs.Text = FormatFloat(0, ViewState("DigitQty"))
            'Else
            '    tbQtyWrhs.Text = FormatFloat(FindConvertUnit(tbProdCode.Text, ddlUnitReq.SelectedValue, tbQtyreq.Text, ViewState("DBConnection")).ToString, ViewState("DigitQty"))
            'End If

            ddlUnitReq.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUnitReq_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnitReq.SelectedIndexChanged, tbQtyreq.TextChanged
        Try
            Dim Qty As Double
            Qty = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdCode.Text) + ", " + QuotedStr(ddlUnitReq.SelectedValue) + ", " + tbQtyreq.Text.Replace(",", "") + ", " + QuotedStr(ddlUnitWrhs.SelectedValue) + " )", ViewState("DBConnection"))
            tbQtyWrhs.Text = CFloat(Qty)
            'tbQtyWrhs.Text = FormatFloat(Qty, ViewState("DigitQty"))

            tbRemarkDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
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
            Return (dete Or piar)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If CFloat(Dr("QtyOrder").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Len(Dr("Remark").ToString) > 255 Then
                    lbStatus.Text = MessageDlg("Remark Max Lenght 255")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If Not CekHd() Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If ddlDept.SelectedValue = "" Then
                Exit Sub
            End If
            Session("filter") = "EXEC S_PRRequestReff " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            ResultField = "Product_Code, Product_Name, Specification, Qty, Unit, Unit_Order"
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, Unit_Order"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")

            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequest.Click
        Dim ResultField As String

        Try
            If ddlDept.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Department Must Have Value")
                Exit Sub
            End If
            Session("filter") = "SELECT User_Id, User_Name FROM VSAUsers "
            ResultField = "User_Id, User_Name"
            ViewState("Sender") = "btnRequest"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSvo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvo.Click
        Dim ResultField As String

        Try
            If ddlDept.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Department Must Have Value")
                Exit Sub
            End If
            Session("filter") = "SELECT TransNmbr,TransDate,Status,WOServiceNo FROM V_MtnServiceOutHd WHERE Status = 'P' "
            ResultField = "TransNmbr"
            ViewState("Sender") = "btnSvo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub
End Class
