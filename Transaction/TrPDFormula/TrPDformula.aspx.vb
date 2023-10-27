Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrPDFormula_TrPDFormula
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PDFormulaHd"

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub

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

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnMaterial" Then
                    tbMaterial.Text = Session("Result")(0).ToString
                    tbMaterialName.Text = Session("Result")(1).ToString
                    tbSpecification.Text = TrimStr(Session("Result")(2).ToString)
                    'tbStdKadarAir.Text = TrimStr(Session("Result")(3).ToString)
                End If

                
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows

                        If Not CekExistData(ViewState("Dt"), "Material", TrimStr(drResult("Product_Code").ToString)) Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Material") = drResult("Product_Code")
                            dr("MaterialName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Percentage") = 0
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
                    GetListCommand("G|H", GridView1, "4,2,3", ListSelectNmbr, Nmbr, msg)

                    If ListSelectNmbr = "" Then Exit Sub

                    For j = 0 To (Nmbr.Length - 1)
                        If Nmbr(j) = "" Then
                            Exit For
                        Else

                            Result = ExecSPCommandGo("Delete", "S_PDFormula", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + "<br />"
                            End If
                        End If
                    Next

                    BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
                    If msg.Trim <> "" Then
                        lbStatus.Text = MessageDlg(msg)
                    End If

                End If
                ViewState("deletetrans") = Nothing
                HiddenRemarkDelete.Value = ""
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
        ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub
    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            'FillCombo(ddlProcess, "EXEC S_GetProcess ''", True, "ProcessCode", "ProcessName", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            End If

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
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_PDFormulaDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function
    

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
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
                'Dim CB As CheckBox
                'Dim Pertamax As Boolean

                'Pertamax = True
                'Result = ""

                'For Each GVR In GridView1.Rows
                '    CB = GVR.FindControl("cbSelect")
                '    If CB.Checked Then
                '        ListSelectNmbr = GVR.Cells(2).Text
                '        If Pertamax Then
                '            Result = "'''" + ListSelectNmbr + "''"
                '            Pertamax = False
                '        Else
                '            Result = Result + ",''" + ListSelectNmbr + "''"
                '        End If
                '    End If
                'Next
                'Result = Result + "'"
                'Session("DBConnection") = ViewState("DBConnection")
                'Session("SelectCommand") = "EXEC S_STFormSJ " + Result
                'Session("ReportFile") = ".../../../Rpt/FormSTSJ.frx"
                'AttachScript("openprintdlg();", Page, Me.GetType)
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
                            ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
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
                '4 = status, 2 & 3 = key, 
                GetListCommand(Status, GridView1, "4,2,3", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    'lbStatus.Text = Nmbr.Length
                    'Exit Sub
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        'Result = ExecSPCommandGo(ActionValue, "S_PDFormula", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserID").ToString, ViewState("DBConnection").ToString)
                        Result = ExecSPCommandGo(ActionValue, "S_PDFormula", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result
                        End If
                    End If
                Next

                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
                'If msg.Trim <> "" Then
                '    lbStatus.Text = MessageDlg(msg)
                'End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbEffDate.Enabled = State
            'tbDate.Enabled = State
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
            'GridDt.Columns(6).Visible = ViewState("Status") = "P"
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CopyDataDt(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt, dtSource As New DataTable
            dtSource = SQLExecuteQuery(GetStringDt(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt("", "0"), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            For Each R In dtSource.Rows
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Material") = R("Material")
                dr("MaterialName") = R("MaterialName")
                dr("Specification") = R("Specification")
                dr("Percentage") = R("Percentage")
                ViewState("Dt").Rows.Add(dr)
            Next
            'GridDt.Columns(6).Visible = ViewState("Status") = "P"
            BindGridDt(ViewState("Dt"), GridDt)
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

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(Not DtExist())
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
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

            If ViewState("DtMotif") Is Nothing Then
                piar = False
            Else
                piar = ViewState("DtMotif").Rows.Count > 0
            End If

            Return (dete Or piar)

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtMotif").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            lbRevisi.Text = "0"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbEffDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbFormulaName.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbMaterial.Text = ""
            tbMaterialName.Text = ""
            tbSpecification.Text = ""
            tbStdKadarAir.Text = FormatNumber("0", ViewState("DigitPercent"))
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbEffDate.IsNull Then
                lbStatus.Text = MessageDlg("Effective Date must have value")
                tbEffDate.Focus()
                Return False
            End If
            If tbFormulaName.Text = "" Then
                lbStatus.Text = MessageDlg("Formula Name must have value")
                tbFormulaName.Focus()
                Return False
            End If
            
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
               
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Function CekDtMotif(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
            

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Motif Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(tbMaterial.Text) Then
                    If CekExistData(ViewState("Dt"), "Material", TrimStr(tbMaterial.Text)) Then
                        lbStatus.Text = "Material " + tbMaterialName.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Dim Row As DataRow
                Row = ViewState("Dt").Select("Material = " + QuotedStr(ViewState("DtValue")))(0)

                Row.BeginEdit()
                Row("Material") = tbMaterial.Text
                Row("MaterialName") = tbMaterialName.Text
                Row("Specification") = tbSpecification.Text
                Row("Percentage") = FormatNumber(tbStdKadarAir.Text, ViewState("DigitPercent"))
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                If ViewState("DtValue") <> TrimStr(tbMaterial.Text) Then
                    If CekExistData(ViewState("Dt"), "Material", TrimStr(tbMaterial.Text)) Then
                        lbStatus.Text = "Material " + tbMaterialName.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                dr = ViewState("Dt").NewRow
                dr("Material") = tbMaterial.Text
                dr("MaterialName") = tbMaterialName.Text
                dr("Specification") = tbSpecification.Text
                dr("Percentage") = FormatNumber(tbStdKadarAir.Text, ViewState("DigitPercent"))
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(Not DtExist())
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            btnSaveTrans.Focus()
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
            If ViewState("StateHd") = "Insert" Then
                tbCode.Text = GetAutoNmbr("FRM", "", CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), "", ViewState("DBConnection").ToString)

                'ubah disini
                SQLString = "INSERT INTO PROFormulaHd (TransNmbr,Revisi,Status,TransDate,EffectiveDate, FormulaName, Remark,UserPrep,DatePrep,FgActive) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 0, 'H', " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEffDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbFormulaName.Text) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate(), 'Y'"
                ViewState("TransNmbr") = tbCode.Text
                ViewState("Revisi") = "0"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PROFormulaHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text, ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PROFormulaHd SET TransDate =" + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", EffectiveDate = " + QuotedStr(Format(tbEffDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                " FormulaName= " + QuotedStr(tbFormulaName.Text) + _
                ", Remark= " + QuotedStr(tbRemark.Text) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Revisi, Material, Percentage FROM PROFormulaDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PROFormulaDt SET Material = @Material, Percentage = @Percentage " + _
                     "WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = @OldRevisi AND Material = @OldMaterial ", con)

            Update_Command.Parameters.Add("@Material", SqlDbType.VarChar, 20, "Material")
            Update_Command.Parameters.Add("@Percentage", SqlDbType.Float, 18, "Percentage")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldRevisi", SqlDbType.Int, 4, "Revisi")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldMaterial", SqlDbType.VarChar, 20, "Material")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PROFormulaDt WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = @Revisi AND Material = @Material ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Revisi", SqlDbType.Int, 4, "Revisi")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Material", SqlDbType.VarChar, 20, "Material")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PROFormulaDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            

            
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Material must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            'If ddlForAllMotif.SelectedValue = "N" Then
            '    If ViewState("DtMotif") Is Nothing Then
            '        lbStatus.Text = MessageDlg("Detail Motif must have at least 1 record")
            '        Exit Sub
            '    End If
            '    If ViewState("DtMotif").Rows.Count = 0 Then
            '        lbStatus.Text = MessageDlg("Detail Motif must have at least 1 record")
            '        Exit Sub
            '    End If
            'End If
            SaveAll()
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            'ModifyInput2(True, pnlInput, pnlDtMotif, GridDtMotif)
            ViewState("TransNmbr") = ""
            ViewState("Revisi") = "0"
            ViewState("Status") = "H"
            newTrans()
            btnHome.Visible = False
            Menu1.Items.Item(0).Enabled = True
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            'GridDtMotif.Columns(0).Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ClearHd()
            Cleardt()
            btnAddDt.Visible = True
            btnAddDt2.Visible = True
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            PnlDt.Visible = True
            BindDataDt("", 0)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Formula No, Revisi, Remark"
            FilterValue = "TransNmbr, Revisi, Remark"
            ViewState("DateFieldName") = FDateName.Split(",")
            ViewState("DateFieldValue") = FDateValue.Split(",")
            ViewState("FieldName") = FilterName.Split(",")
            ViewState("FieldValue") = FilterValue.Split(",")
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
        Dim GVR As GridViewRow
        Dim index As Integer
        Dim CekMenu As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)

                DDL = GridView1.Rows(index).FindControl("ddl")

                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(3).Text
                    ViewState("Status") = GVR.Cells(4).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    'BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    Menu1.TabIndex = 0

                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"

                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    'ModifyInput2(False, pnlInput, pnlDtMotif, GridDtMotif)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Enabled = True
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("Status") = GVR.Cells(4).Text
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        Menu1.TabIndex = 0
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        'ModifyInput2(True, pnlInput, pnlDtMotif, GridDtMotif)
                        btnHome.Visible = False
                        btnAddDt.Visible = True
                        btnAddDt2.Visible = True
                        Menu1.Items.Item(0).Enabled = True
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(Not DtExist())
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
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    If Not GVR.Cells(4).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must Post Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_PDFormulaRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + GVR.Cells(3).Text + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = tbCode.Text
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_PDFormFormula '" + GVR.Cells(2).Text + "'"
                        Session("ReportFile") = ".../../../Rpt/FormPDFormula.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Copy New" Then
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(3).Text
                    ViewState("StateHd") = "Insert"
                    ViewState("DigitCurr") = 0
                    MovePanel(PnlHd, pnlInput)
                    ModifyInput2(True, pnlInput, PnlDt, GridDt)

                    newTrans()
                    btnHome.Visible = False
                    Menu1.Items.Item(0).Enabled = True
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    Menu1.TabIndex = 0
                    ViewState("Status") = "H"

                    CopyDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    'ViewState("StateHd") = "View"
                    'ModifyInput2(False, pnlInput, pnlDtMotif, GridDtMotif)
                    'ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    tbCode.Text = ""
                    lbRevisi.Text = "0"
                    tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
                    tbEffDate.SelectedDate = ViewState("ServerDate") 'Now.Date

                    ViewState("TransNmbr") = ""
                    ViewState("Revisi") = "0"
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Enabled = True
                    Menu1.Items.Item(0).Selected = True
                    tbCode.Focus()

                End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        'Try
        '    lbStatus.Text = MessageDlg("A")
        '    If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
        '        ViewState("SortOrder") = "ASC"
        '    Else
        '        ViewState("SortOrder") = "DESC"
        '    End If
        '    ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
        '    BindData(ViewState("AdvanceFilter"))
        'Catch ex As Exception
        '    lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        'End Try
    End Sub
    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(PnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalStandard, TotalAdjust As Decimal
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Material")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
                    ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
                    'DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
                    ''DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    TotalAdjust = GetTotalSum(ViewState("Dt"), "Percentage")
                    e.Row.Cells(3).Text = "Total : "
                    ' for the Footer, display the running totals
                    e.Row.Cells(4).Text = FormatNumber(TotalAdjust, ViewState("DigitPercent"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As DataRow = ViewState("Dt").Rows(e.RowIndex)
            dr = ViewState("Dt").Select("Material = " + QuotedStr(GVR("Material").ToString))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbDate.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            'newTrans()
            tbCode.Text = Nmbr
            lbRevisi.Text = Revisi
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbEffDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToText(tbFormulaName, Dt.Rows(0)("FormulaName").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            'AttachScript("setformathd();", Me.Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Material = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbMaterial, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("MaterialName").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbStdKadarAir, Dr(0)("Percentage").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        If CekHd() = False Then
            Exit Sub
        End If
        Try
            index = Int32.Parse(e.Item.Value)
            If index = 1 Then
                
                MultiView1.ActiveViewIndex = 1
                Menu1.Items.Item(1).Selected = True
            Else
                MultiView1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True
            End If
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Material must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            'If ViewState("DtMotif").Rows.Count = 0 Then
            '    lbStatus.Text = MessageDlg("Detail PR must have at least 1 record")
            '    Exit Sub
            'End If
            SaveAll()
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save All Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMaterial.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select Product_Code, Product_Name, Specification, Unit from VMsProduct"
            ResultField = "Product_Code, Product_Name, Specification, Unit"
            ViewState("Sender") = "btnMaterial"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Material Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbMaterial_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterial.TextChanged
        Dim dt As DataTable
        Dim Dr As DataRow
        Try
            dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit, UnitPack, UnitM2, CostCtr, CostCtrName FROM VMsProduct Where Product_Code = " + QuotedStr(tbMaterial.Text), ViewState("DBConnection").ToString).Tables(0)

            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbMaterial.Text = Dr("Product_Code")
                tbMaterialName.Text = Dr("Product_Name")
                tbSpecification.Text = TrimStr(Dr("Specification").ToString)
            Else
                tbMaterial.Text = ""
                tbMaterialName.Text = ""
                tbSpecification.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbMaterial.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("filter") = "Select Product_Code, Product_Name, Specification, Unit from VMsProduct"
            ResultField = "Product_Code, Product_Name, Specification, unit"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
    End Sub

  
End Class
