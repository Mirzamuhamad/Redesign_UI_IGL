
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Partial Class MsPortalRegistrasi
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
           
            'FillCombo(ddlUnit, "SELECT UnitCode, UnitName FROM MsUnit", True, "UnitCode", "UnitName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            'tbKepadatan.Attributes.Add("OnKeyDown", "return PressNumeric();")

            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"

        End If
        If Not Session("Result") Is Nothing Then

            'If ViewState("Sender") = "btnAccExpense" Then
            '    tbAccExpense.Text = Session("Result")(0).ToString
            '    tbAccExpenseName.Text = Session("Result")(1).ToString
            'End If
           
            Session("Result") = Nothing
            ViewState("Sender") = Nothing

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


    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged

        Try

            If Left(tbCode.Text, 4) <> "S/" Then
                tbCode.Text = "S/" + tbCode.Text
            Else
                tbCode.Text = tbCode.Text
            End If

        Catch ex As Exception
            lstatus.Text = "ddl Term Error : " + ex.ToString
        End Try
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
            ' If tbCode.Enabled Then
            '     tbCode.Text = ""
            ' End If

            ' tbName.Text = ""
            ' ddlTypeID.SelectedIndex = 0
            ' ddlGender.SelectedIndex = 0
            ' tbSellerID.Text = ""
            ' tbAddress.Text = ""
            ' tbAddress2.Text = ""
            ' tbCity.Text = ""
            ' tbZipCode.Text = ""
            ' tbEmail.Text = ""
            ' tbPhone.Text = ""
            ' tbNpwp.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal SellCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_RegistrationRequests  WHERE RequestNumber = " + QuotedStr(SellCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("RequestNumber").ToString)
            BindToText(tbName, DT.Rows(0)("FullName").ToString)
            BindToDropList(ddlRoleType, DT.Rows(0)("RoleType").ToString)
            BindToText(tbEmail, DT.Rows(0)("Email").ToString)
            BindToText(tbKavlingDesc, DT.Rows(0)("KavlingDesc").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
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
            SqlString = "SELECT * From V_RegistrationRequests " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "RequestId DESC"
                ViewState("SortOrder") = "DESC"
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

    ' Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '     Dim StrFilter As String
    '     Try
    '         ddlField2.SelectedValue.Replace("JobCode", "Job_Code")
    '         StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '         Session("DBConnection") = ViewState("DBConnection")
    '         Session("PrintType") = "Print"
    '         Session("SelectCommand") = "S_FormPrintMaster6 'V_RegistrationRequests','SellCode','SellName','Gender','TypeID','SellID','Address1+'', ''+Address2+'', ''+ Desa+'', ''+ Kec+'', ''+ Kab +'', ''+ City +'', Pos: ''+ ZipCode+'', Telp : ''+ Phone','Seller File','Code','Description','Gender','TypeID','SellerID','Alamat'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
    '         Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
    '         AttachScript("openprintdlg();", Page, Me.GetType)
    '     Catch ex As Exception
    '         lstatus.Text = "btn print Error = " + ex.ToString
    '     End Try
    ' End Sub

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
     
        Try
            
        
        ' ================== ACTION MENU ==================
        If e.CommandName = "Go" Then

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim GVR As GridViewRow = DataGrid.Rows(index)
            Dim DDL As DropDownList = CType(GVR.FindControl("ddl"), DropDownList)

            If DDL.SelectedValue = "View" Then
                MovePanel(pnlHd, pnlInput)
                FillTextBox(GVR.Cells(1).Text)
                ViewState("State") = "View"
                ModifyInput(False, pnlInput)
                tbCode.Enabled = False
                btnHome.Visible = True
                BtnSave.Visible = False
                btnReset.Visible = False
                btnCancel.Visible = False

            ElseIf DDL.SelectedValue = "Edit" Then
                If CheckMenuLevel("Edit") = False Then Exit Sub

                MovePanel(pnlHd, pnlInput)
                FillTextBox(GVR.Cells(1).Text)
                ViewState("State") = "Edit"
                ModifyInput(True, pnlInput)
                tbCode.Enabled = False
                btnHome.Visible = False
                BtnSave.Visible = True
                btnReset.Visible = True
                btnCancel.Visible = True
                tbName.Focus()

            ElseIf DDL.SelectedValue = "Delete" Then
                If CheckMenuLevel("Delete") = False Then Exit Sub

                SQLExecuteNonQuery("DELETE RegistrationRequests WHERE RequestId = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString())
                bindDataGrid()
            End If

        End If


        ' ================== LIGHTBOX PREVIEW ==================
        If e.CommandName = "PreviewLightbox" Then

            Dim requestId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim sb As New StringBuilder()
            sb.Append("[")

        Using conn As New System.Data.SqlClient.SqlConnection(ViewState("DBConnection").ToString())
    conn.Open()

                Dim sql As String = "SELECT FileUrl,FileType FROM V_RegistrationDocuments WHERE RequestId = @RequestId ORDER BY UploadedAt"

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@RequestId", requestId)

                    Using dr As SqlDataReader = cmd.ExecuteReader()
                        While dr.Read()
                           sb.Append("{path:'" & dr("FileUrl").ToString() & "',type:'" & dr("FileType").ToString() & "'},")

                        End While
                    End Using
                End Using
            End Using

            If sb.Length > 1 Then sb.Length -= 1
            sb.Append("]")

            ClientScript.RegisterStartupScript( Me.GetType(),"Lightbox","openLightbox(" & sb.ToString() & ");",True)

        End If

        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCommand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbCode.Enabled = True
            'FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            ClearInput()
            ModifyInput(True, pnlInput)
            BtnSave.Visible = True
            btnReset.Visible = True
            btnCancel.Visible = True
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            ' If tbCode.Text.Trim.Length = 0 Then
            '     lstatus.Text = MessageDlg("ID Code must be filled.")
            '     tbCode.Focus()
            '     Return False
            ' End If

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
                If SQLExecuteScalar("SELECT RequestId FROM V_RegistrationRequests WHERE Email = " + QuotedStr(tbEmail.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Email " + QuotedStr(tbEmail.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO RegistrationRequests (FullName,RoleType, Email, KavlingDesc) " + _
                "SELECT " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(ddlRoleType.SelectedValue) + ", " + _
                QuotedStr(tbEmail.Text) + ", " + _
                QuotedStr(tbKavlingDesc.Text)
            Else
                SqlString = "UPDATE RegistrationRequests SET FullName = " + QuotedStr(tbName.Text) & _
                            ", RoleType = " + QuotedStr(ddlRoleType.SelectedValue) & _
                            ", KavlingDesc = " + QuotedStr(tbKavlingDesc.Text) & _
                            " WHERE Email = " + QuotedStr(tbEmail.Text)
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


    'Protected Sub btnAccAsset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAsset.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "Select Account, Description from V_MsAccount WHERE FgActive = 'Y' "
    '        ResultField = "Account, Description"
    '        ViewState("Sender") = "btnAccAsset"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lstatus.Text = "btn Acc Asset Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Home Error : " + ex.ToString
        End Try
    End Sub

    Public Function GetStatusCss(ByVal status As Object) As String
    If status Is Nothing Then
        Return ""
    End If

    Select Case UCase(status.ToString())
        Case "PENDING"
            Return "status-badge status-pending"
        Case "APPROVED"
            Return "status-badge status-approved"
        Case "REJECTED"
            Return "status-badge status-rejected"
        Case Else
            Return "status-badge"
    End Select
End Function

    
End Class
