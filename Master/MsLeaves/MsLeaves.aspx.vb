Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsLeaves_MsLeaves
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlAbsStatus, "EXEC S_GetAbsenceDt 'Cuti'", True, "AbsenceCode", "AbsenceName", ViewState("DBConnection"))
            FillCombo(ddlLeaveType, "SELECT LeaveTypeCode, LeaveTypeName FROM MsLeaveType", True, "LeaveTypeCode", "LeaveTypeName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            tbDispensasi.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMaxTaken.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDispensasi.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMaxRecuring.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLeadTime.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If

            tbName.Text = ""
            ddlGender.SelectedValue = "All"
            tbDispensasi.Text = "0"
            tbMaxTaken.Text = "0"
            ddlFgRecuring.SelectedValue = "Y"
            tbMaxRecuring.Text = "0"
            tbLeadTime.Text = "0"
            ddlFgHoliday.SelectedValue = "N"
            ddlAbsStatus.SelectedIndex = 0

            tbMaxRecuring.Enabled = (ddlFgRecuring.SelectedValue = "Y")
            tbLeadTime.Enabled = (ddlFgRecuring.SelectedValue = "Y")
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
            SqlString = "SELECT A.*, B.AbsStatusName, C.LeaveTypeName FROM MsLeaves A " + _
                        " INNER JOIN VMsAbsStatus B ON A.AbsStatus = B.AbsStatusCode " + _
                        " LEFT OUTER JOIN MsLeaveType C ON A.LeaveType = C.LeaveTypeCode " + StrFilter + " ORDER BY A.LeaveCode"
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "LeaveCode ASC"
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
            ddlField2.SelectedValue.Replace("LeaveCode", "Leaves_Code")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster5 'VMsLeaves A','LeaveCode','LeaveName','Gender','MaxTaken','AbsStatusName','Leaves & Permission File','Leaves Code','Leaves Name','Gender','Max Taken','Absence Status'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
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

            SQLExecuteNonQuery("DELETE FROM MsLeaves WHERE LeaveCode = '" & GVR.Cells(0).Text & "' ", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
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
            tbCode.Enabled = False

            BindToText(tbCode, obj.Cells(0).Text)
            BindToText(tbName, obj.Cells(1).Text)
            BindToDropList(ddlGender, obj.Cells(2).Text)
            BindToText(tbDispensasi, obj.Cells(3).Text)
            BindToText(tbMaxTaken, obj.Cells(4).Text)
            BindToDropList(ddlFgRecuring, obj.Cells(5).Text)
            BindToText(tbMaxRecuring, obj.Cells(6).Text)
            BindToText(tbLeadTime, obj.Cells(7).Text)
            BindToDropList(ddlFgHoliday, obj.Cells(8).Text)
            BindToDropList(ddlAbsStatus, obj.Cells(9).Text)
            BindToDropList(ddlLeaveType, obj.Cells(10).Text)
            ViewState("State") = "Edit"

            tbMaxRecuring.Enabled = (ddlFgRecuring.SelectedValue = "Y")
            tbLeadTime.Enabled = (ddlFgRecuring.SelectedValue = "Y")
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
            tbCode.Enabled = True
            ClearInput()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Leaves Code must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Leaves Name must be filled.")
                tbName.Focus()
                Return False
            End If
            If ddlGender.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Gender must be filled.")
                ddlGender.Focus()
                Return False
            End If
            If IsNumeric(tbDispensasi.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Dispensasi must be in numeric.")
                tbDispensasi.Focus()
                Exit Function
            End If
            If CFloat(tbDispensasi.Text) < 0 Then
                lstatus.Text = MessageDlg("Dispensasi must be filled.")
                tbDispensasi.Focus()
                Exit Function
            End If
            If IsNumeric(tbMaxTaken.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Max Taken must be in numeric.")
                tbMaxTaken.Focus()
                Exit Function
            End If
            If CFloat(tbMaxTaken.Text) < 0 Then
                lstatus.Text = MessageDlg("Max Taken must be filled.")
                tbMaxTaken.Focus()
                Exit Function
            End If
            If ddlFgRecuring.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Recuring Taken must be filled.")
                ddlFgRecuring.Focus()
                Return False
            End If

            If ddlFgRecuring.SelectedValue = "Y" Then
                If IsNumeric(tbMaxRecuring.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Max Recuring must be in numeric.")
                    tbMaxRecuring.Focus()
                    Exit Function
                End If
                If CFloat(tbMaxRecuring.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Max Recuring must be filled.")
                    tbMaxRecuring.Focus()
                    Exit Function
                End If
                If IsNumeric(tbLeadTime.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Lead Time must be in numeric.")
                    tbLeadTime.Focus()
                    Exit Function
                End If
                If CFloat(tbLeadTime.Text) < 0 Then
                    lstatus.Text = MessageDlg("Lead Time must be filled.")
                    tbLeadTime.Focus()
                    Exit Function
                End If
            End If

            If ddlFgHoliday.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Include Holiday must be filled.")
                ddlFgHoliday.Focus()
                Return False
            End If
            If ddlAbsStatus.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Absence Status must be filled.")
                ddlAbsStatus.Focus()
                Return False
            End If
            If ddlLeaveType.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Leave Type must be filled.")
                ddlLeaveType.Focus()
                Return False
            End If
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
                If SQLExecuteScalar("SELECT LeaveCode FROM VMsLeaves WHERE LeaveCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Leave Code " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsLeaves (LeaveCode, LeaveName, Gender, Dispensasi, MaxTaken, FgRecuring, MaxRecuring, LeadTime, FgHoliday, AbsStatus, LeaveType, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(ddlGender.Text) + ", " + tbDispensasi.Text + ", " & _
                tbMaxTaken.Text + ", " + QuotedStr(ddlFgRecuring.SelectedValue) + ", " + tbMaxRecuring.Text + ", " + tbLeadTime.Text + ", " + QuotedStr(ddlFgHoliday.SelectedValue) + ", " + QuotedStr(ddlAbsStatus.SelectedValue) + ", " + _
                QuotedStr(ddlLeaveType.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "UPDATE MsLeaves SET LeaveName= " + QuotedStr(tbName.Text) & _
                            ", Gender = " + QuotedStr(ddlGender.SelectedValue) & _
                            ", Dispensasi = " + tbDispensasi.Text & _
                            ", MaxTaken = " + tbMaxTaken.Text & _
                            ", FgRecuring = " + QuotedStr(ddlFgRecuring.SelectedValue) & _
                            ", MaxRecuring = " + tbMaxRecuring.Text & _
                            ", LeadTime = " + tbLeadTime.Text & _
                            ", FgHoliday = " + QuotedStr(ddlFgHoliday.SelectedValue) & _
                            ", AbsStatus = " + QuotedStr(ddlAbsStatus.SelectedValue) & _
                            ", LeaveType = " + QuotedStr(ddlLeaveType.SelectedValue) & _
                            " WHERE LeaveCode = " + QuotedStr(tbCode.Text)
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
            tbName.Focus()
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

    Protected Sub ddlFgRecuring_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgRecuring.SelectedIndexChanged
        Try
            If ddlFgRecuring.SelectedValue = "Y" Then
                tbMaxRecuring.Enabled = True
                tbLeadTime.Enabled = True
                tbMaxRecuring.Text = "0"
                tbLeadTime.Text = "0"
            Else
                tbMaxRecuring.Enabled = False
                tbLeadTime.Enabled = False
                tbMaxRecuring.Text = "0"
                tbLeadTime.Text = "0"
            End If
        Catch ex As Exception
            lstatus.Text = "ddlFgRecuring_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
