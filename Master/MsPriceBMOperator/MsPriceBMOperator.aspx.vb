Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsPriceBMOperator
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddldivision, "Select Division_Code, Division_Name From V_MsDivisionTPH", True, "Division_Code", "Division_Name", ViewState("DBConnection"))
            ' FillCombo(ddlAbsStatus, "EXEC S_GetAbsenceDt 'Cuti'", True, "AbsenceCode", "AbsenceName", ViewState("DBConnection"))
            ' FillCombo(ddlLeaveType, "SELECT LeaveTypeCode, LeaveTypeName FROM MsLeaveType", True, "LeaveTypeCode", "LeaveTypeName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            tbTphToLTJ.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTphToPksJ.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLtToPksDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTphToPksDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbOPLTtoPksDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbOPTphToLT.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbOPTphToPks.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbOPTphToPKSDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
        End If

        lstatus.Text = ""
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            'If CommandName = "Insert" Then
            '    If ViewState("FgInsert") = "N" Then
            '        lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function


    Private Sub ClearInput()
        Try
            If ddldivision.Enabled Then
                ddldivision.SelectedValue = ""
            End If

            ddldivision.SelectedValue = ""
            tbDate.SelectedDate = ViewState("ServerDate")
            tbTphToLTJ.Text = "0"
            tbTphToPksJ.Text = "0"
            tbLtToPksDt.Text = "0"
            tbTphToPksDt.Text = "0"
            tbOPLTtoPksDt.Text = "0"
            tbOPTphToLT.Text = "0"
            tbOPTphToPks.Text = "0"
            tbOPTphToPKSDt.Text = "0"
            

            'tbMaxRecuring.Enabled = (ddlFgRecuring.SelectedValue = "Y")
            'tbLeadTime.Enabled = (ddlFgRecuring.SelectedValue = "Y")
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
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
            lstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM V_MsPriceBMOperator " + StrFilter
            '" INNER JOIN VMsAbsStatus B ON A.AbsStatus = B.AbsStatusCode " + _
            '" LEFT OUTER JOIN MsLeaveType C ON A.LeaveType = C.LeaveTypeCode " + StrFilter + " ORDER BY A.LeaveCode"
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Divisi ASC"
                ViewState("SortOrder") = "ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim dt As DataTable
            dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)

            If dt.Rows.Count = 0 Then
                lstatus.Text = "No Data"
                DataGrid.Visible = False
                btnAdd2.Visible = False
            Else
                DataGrid.Visible = True
                btnAdd2.Visible = True
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            ddlField2.SelectedValue.Replace("PlantPeriodCode", "PlantPeriod_Code")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster10 'V_MsPriceBMOperator','Divisi','EffectiveDate','BmJonder_TPHtoLT','BmJonder_TPHtoPKS','BmDt_LTtoPKS','BmDt_TPHtoPKS','OprJonder_TPHtoLT','OprJonder_TPHtoPKS','OprDt_LTtoPKS','OprDt_TPHtoPKS','PRICE BM & OPERATOR','Division','Effective Date','BM Jonder TPH to LT','BM Jonder TPH to PKS','BM DT LT to PKS','BM DT TPH to PKS','Op Jonder to LT','Op Jonder LT to PKS','Op Dt LT to PKS','Op Dt TPH to PKS'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster10.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        'Dim SQLString As String
        'Dim dbCode, dbName As TextBox
        'Dim cbxWrhsGroup As DropDownList
        'Dim cbxWrhsArea As DropDownList
        'Dim cbxWrhsType As DropDownList
        'Dim cbxWrhsCondition As DropDownList
        'Dim cbxFgActive As DropDownList

        Try
            If e.CommandName = "Insert" Then
                'dbCode = DataGrid.FooterRow.FindControl("WrhsCodeAdd")
                'dbName = DataGrid.FooterRow.FindControl("WrhsNameAdd")
                'cbxWrhsGroup = DataGrid.FooterRow.FindControl("WrhsGroupAdd")
                'cbxWrhsArea = DataGrid.FooterRow.FindControl("WrhsAreaAdd")
                'cbxWrhsType = DataGrid.FooterRow.FindControl("WrhsTypeAdd")
                'cbxWrhsCondition = DataGrid.FooterRow.FindControl("WrhsConditionAdd")
                'cbxFgActive = DataGrid.FooterRow.FindControl("FgActiveAdd")

                'If dbCode.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Code must be filled."
                '    dbCode.Focus()
                '    Exit Sub
                'End If
                'If tbName.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Name must be filled."
                '    tbName.Focus()
                '    Exit Sub
                'End If

                'If SQLExecuteScalar("SELECT Wrhs_Code From VMsLeaves WHERE Wrhs_Code = " + QuotedStr(dbCode.Text), Session("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "Warehouse " + QuotedStr(dbCode.Text) + " has already been exist"
                '    Exit Sub
                'End If

                ''insert the new entry
                'SQLString = "Insert into MsLeaves (WrhsCode, WrhsName, WrhsGroup, WrhsArea, WrhsType, WrhsCondition, FgActive, UserId, UserDate ) " + _
                '"SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " & _
                'QuotedStr(cbxWrhsGroup.SelectedValue) + ", " + QuotedStr(cbxWrhsArea.SelectedValue) + ", " & _
                'QuotedStr(cbxWrhsType.SelectedItem.ToString) + ", " + QuotedStr(cbxWrhsCondition.SelectedValue) + ", " & _
                'QuotedStr(cbxFgActive.SelectedValue) + ", " & _
                'QuotedStr(Session("userId").ToString) + ", getDate()"
                'SQLExecuteNonQuery(SQLString)
                'bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        'Dim txtID As Label
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'txtID = DataGrid.Rows(e.RowIndex).FindControl("ItemNo")

            SQLExecuteNonQuery("DELETE FROM MsPriceBMOPR WHERE Divisi = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub FillTextBox(ByVal Divisi As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsPriceBMOperator A WHERE Divisi = " + QuotedStr(Divisi)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToDropList(ddldivision, DT.Rows(0)("Divisi").ToString)
            BindToDate(tbDate, DT.Rows(0)("EffectiveDate").ToString)
            BindToText(tbTphToLTJ, DT.Rows(0)("BmJonder_TPHtoLT").ToString)
            BindToText(tbTphToPksJ, DT.Rows(0)("BmJonder_TPHtoPKS").ToString)
            BindToText(tbLtToPksDt, DT.Rows(0)("BmDt_LTtoPKS").ToString)
            BindToText(tbTphToPksDt, DT.Rows(0)("BmDt_TPHtoPKS").ToString)
            BindToText(tbOPLTtoPksDt, DT.Rows(0)("OprDt_LTtoPKS").ToString)
            BindToText(tbOPTphToLT, DT.Rows(0)("OprJonder_TPHtoLT").ToString)
            BindToText(tbOPTphToPks, DT.Rows(0)("OprJonder_TPHtoPKS").ToString)
            BindToText(tbOPTphToPKSDt, DT.Rows(0)("OprDt_TPHtoPKS").ToString)


        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            obj = DataGrid.Rows(e.NewEditIndex)

            pnlHd.Visible = False
            pnlInput.Visible = True
            ddldivision.Enabled = False

            FillTextBox(obj.Cells(1).Text)
            ViewState("State") = "Edit"
            tbTphToPksJ.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        'Dim SQLString As String
        'Dim dbName As TextBox
        'Dim lbCode As Label
        'Dim CbxWrhsGroup As DropDownList
        'Dim CbxWrhsArea As DropDownList
        'Dim CbxWrhsType As DropDownList
        'Dim CbxFgActive As DropDownList

        'Try
        '    lbCode = DataGrid.Rows(e.RowIndex).FindControl("WrhsCodeEdit")
        '    dbName = DataGrid.Rows(e.RowIndex).FindControl("WrhsNameEdit")
        '    CbxWrhsGroup = DataGrid.Rows(e.RowIndex).FindControl("WrhsGroupEdit")
        '    CbxWrhsArea = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaEdit")
        '    CbxWrhsType = DataGrid.Rows(e.RowIndex).FindControl("WrhsTypeEdit")
        '    CbxFgActive = DataGrid.Rows(e.RowIndex).FindControl("FgActiveEdit")

        '    If dbName.Text.Trim.Length = 0 Then
        '        lstatus.Text = " Wrhs Name must be filled."
        '        dbName.Focus()
        '        Exit Sub
        '    End If

        '    SQLString = "Update MsLeaves set WrhsName= " + QuotedStr(dbName.Text) + "," & _
        '    "WrhsGroup = " + QuotedStr(CbxWrhsGroup.SelectedValue) & _
        '    "WrhsArea = " + QuotedStr(CbxWrhsArea.SelectedValue) & _
        '    "WrhsType = " + QuotedStr(CbxWrhsType.SelectedItem.ToString) & _
        '    "FgActive = " + QuotedStr(CbxFgActive.SelectedValue) & _
        '    " where WrhsCode = " & QuotedStr(lbCode.Text)

        '    SQLExecuteNonQuery(SQLString)

        '    DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '    DataGrid.EditIndex = -1
        '    bindDataGrid()

        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            ddldivision.Enabled = True
            ClearInput()
            ddldivision.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If ddldivision.SelectedValue.Trim.Length = 0 Then
                lstatus.Text = MessageDlg(" Divisi must be filled.")
                ddldivision.Focus()
                Return False
            End If
            'If tbDate.SelectedDate = "" Then
            '    lstatus.Text = MessageDlg(" Date must be filled.")
            '    tbDate.Focus()
            '    Return False
            'End If
            If tbTphToPksJ.Text.Trim = "" Then
                lstatus.Text = MessageDlg("BM Jonder TPH To PKS must be filled.")
                tbTphToPksJ.Focus()
                Return False
            End If

            If tbTphToLTJ.Text.Trim = "" Then
                lstatus.Text = MessageDlg("BM Jonder TPH To LT must be filled.")
                tbTphToLTJ.Focus()
                Return False
            End If

            If tbTphToPksDt.Text.Trim = "" Then
                lstatus.Text = MessageDlg("BM DT TPH To PKS must be filled.")
                tbTphToPksDt.Focus()
                Return False
            End If

            If tbLtToPksDt.Text.Trim = "" Then
                lstatus.Text = MessageDlg("BM DT LT To PKS must be filled.")
                tbLtToPksDt.Focus()
                Return False
            End If



            If tbOPTphToPks.Text.Trim = "" Then
                lstatus.Text = MessageDlg("OP DT TPH To PKS must be filled.")
                tbOPTphToPks.Focus()
                Return False
            End If

            If tbOPTphToLT.Text.Trim = "" Then
                lstatus.Text = MessageDlg("OP DT TPH To LT must be filled.")
                tbOPTphToLT.Focus()
                Return False
            End If



            If tbOPLTtoPksDt.Text.Trim = "" Then
                lstatus.Text = MessageDlg("OP DT LT To LT must be filled.")
                tbOPTphToLT.Focus()
                Return False
            End If

            If tbOPTphToPKSDt.Text.Trim = "" Then
                lstatus.Text = MessageDlg("OP DT TPH To LT must be filled.")
                tbOPTphToLT.Focus()
                Return False
            End If
            'If IsNumeric(tbStartRange.Text.Replace(",", "")) = 0 Then
            '    lstatus.Text = MessageDlg("Start Range must be in numeric.")
            '    tbStartRange.Focus()
            '    Exit Function
            'End If
            


            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT Divisi FROM V_MsPriceBMOperator WHERE Divisi = " + QuotedStr(ddldivision.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Plantation Period Code " + QuotedStr(ddldivision.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsPriceBMOPR (Divisi, EffectiveDate, BmJonder_TPHtoLT, BmJonder_TPHtoPKS,BmDt_LTtoPKS, BmDt_TPHtoPKS, OprJonder_TPHtoLT, OprJonder_TPHtoPKS, OprDt_LTtoPKS, OprDt_TPHtoPKS, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(ddldivision.SelectedValue) + ", " + QuotedStr(tbDate.SelectedDate) + ", " & _
                QuotedStr(tbTphToLTJ.Text) + ", " + QuotedStr(tbTphToPksJ.Text) + ", " + tbLtToPksDt.Text + ", " & _
                tbTphToPksDt.Text + ", " + tbOPTphToLT.Text + ", " + tbOPTphToPks.Text + ", " + _
                QuotedStr(tbOPLTtoPksDt.Text) + "," + QuotedStr(tbOPTphToPKSDt.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "UPDATE MsPriceBMOPR SET EffectiveDate= " + QuotedStr(tbDate.SelectedDate) & _
                            ", BmJonder_TPHtoLT = " + QuotedStr(tbTphToLTJ.Text) & _
                            ", BmJonder_TPHtoPKS = " + QuotedStr(tbTphToPksJ.Text) & _
                            ", BmDt_LTtoPKS = " + tbLtToPksDt.Text & _
                            ", BmDt_TPHtoPKS = " + tbTphToPksDt.Text & _
                            ", OprJonder_TPHtoLT = " + tbOPTphToLT.Text & _
                            ", OprJonder_TPHtoPKS = " + tbOPTphToPks.Text & _
                            ", OprDt_LTtoPKS = " + QuotedStr(tbOPLTtoPksDt.Text) & _
                            ", OprDt_TPHtoPKS = " + QuotedStr(tbOPTphToPKSDt.Text) & _
                            " WHERE Divisi = " + QuotedStr(ddldivision.SelectedValue)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbTphToLTJ.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    ' Protected Sub ddlFgRecuring_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgRecuring.SelectedIndexChanged
    '    Try
    '  If ddlFgRecuring.SelectedValue = "Y" Then
    'tbMaxRecuring.Enabled = True
    'tbLeadTime.Enabled = True
    'tbMaxRecuring.Text = "0"
    'tbLeadTime.Text = "0"
    'Else
    'tbMaxRecuring.Enabled = False
    'tbLeadTime.Enabled = False
    'tbMaxRecuring.Text = "0"
    'tbLeadTime.Text = "0"
    'End If
    '   Catch ex As Exception
    '      lstatus.Text = "ddlFgRecuring_SelectedIndexChanged Error : " + ex.ToString
    ' End Try
    'End Sub
End Class
