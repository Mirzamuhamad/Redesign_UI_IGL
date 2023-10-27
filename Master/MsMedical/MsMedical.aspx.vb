Imports System.Data

Partial Class Master_MsMedical_MsMedical
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            SetInit()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            FillCombo(ddlMedicalGroup, "EXEC S_GetMsMedicalGroup", False, "MedicalGrpCode", "MedicalGrpName", ViewState("DBConnection"))
        End If

        If Not Session("Result") Is Nothing Then
            Dim Acc As New TextBox
            Dim AccName As New Label
            Dim CurrCode As New DropDownList

            If ViewState("Sender") = "btnAcc" Then
                'tbAccount.Text = Session("Result")(0).ToString
                'tbAccName.Text = Session("Result")(1).ToString
                'ddlCurrCode.SelectedValue = Session("Result")(2).ToString
            End If

            If ViewState("Sender") = "btnJoinMedical" Then
                tbJoinMedical.Text = Session("Result")(0).ToString
                tbJoinMedicalName.Text = Session("Result")(1).ToString
            End If


            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        dsMedicalGroup.ConnectionString = ViewState("DBConnection")
        lstatus.Text = ""
    End Sub
    Private Sub SetInit()
        Try

            tbMaxTaken.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLeadTime.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbCheckPlafondBy1Claim.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbMaxTaken.Attributes.Add("OnBlur", "setformat();")
            'tbLeadTime.Attributes.Add("OnBlur", "setformat();")
            'tbCheckPlafondBy1Claim.Attributes.Add("OnBlur", "setformat();")

            
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
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
            'SqlString = " select A.*, B.MedicalGrpName As MedicalGrpName , C.MedicalName As JoinMedicalName" + _
            ' " from MsMedical A LEFT OUTER JOIN " + _
            ' " MsMedicalGroup B ON A.MedicalGroup = B.MedicalGrpCode LEFT OUTER JOIN " + _
            ' " MsMedical C ON A.JoinMedical = C.MedicalCode " + StrFilter

            SqlString = " select * from V_MsMedical " + StrFilter

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Medical_Code Asc"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

       
    End Sub



    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            obj = DataGrid.Rows(e.NewEditIndex)
            pnlHd.Visible = False
            pnlInput.Visible = True
            tbMedicalCode.Enabled = False

            FillTextBox(obj.Cells(0).Text)
            ViewState("State") = "Edit"

            

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
  
    Private Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim GVR As GridViewRow = Nothing
        Dim DDL As DropDownList
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Insert" Then
                
            ElseIf e.CommandName = "View" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                ViewState("Nmbr") = GVR.Cells(0).Text
                pnlHd.Visible = False
               
            ElseIf e.CommandName = "Assign" Then
                Dim paramgo As String
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                paramgo = GVR.Cells(0).Text + "|" + GVR.Cells(1).Text
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Pay Type', '" + Request.QueryString("KeyId") + "', '" + paramgo + "','AssMsMedicalUser');", True)
                End If
            ElseIf e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                ViewState("Nmbr") = GVR.Cells(1).Text
                If DDL.SelectedValue = "View" Then
                    ViewState("State") = "View"
                    FillTextBox(ViewState("Nmbr"))
                    BtnSave.Visible = False
                    btnReset.Visible = False
                    ModifyInput(False)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    ViewState("State") = "Edit"
                    FillTextBox(ViewState("Nmbr"))
                    ModifyInput(True)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                    BtnSave.Visible = True
                    btnReset.Visible = True
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("Delete from MsMedical where MedicalCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
   

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            SQLExecuteNonQuery("Delete from MsMedical where MedicalCode = '" & GVR.Cells(0).Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
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
    
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Dim StrFilter, SQLString As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            SQLString = "S_PEFormMedical " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptMsMedical.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            ModifyInput(True)
            ClearInput()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ClearInput()
        Try
            If tbMedicalCode.Enabled Then
                tbMedicalCode.Text = ""
            End If

            tbMedicalName.Text = ""
            'ddlMedicalGroup.SelectedIndex = 0
            ddlGender.SelectedIndex = 0
            ddlClaim.SelectedIndex = 1
            tbLeadTime.Text = 0
            ddlMaxTaken.SelectedIndex = 1
            tbMaxTaken.Text = 0
            tbMaxTaken.Enabled = False
            tbJoinMedical.Text = ""
            tbJoinMedicalName.Text = ""
            ddlCheckPlafond.SelectedIndex = 1
            ddlShowSlip.SelectedIndex = 1
            ddlJobLevel.SelectedIndex = 1
            ddlCheckPlafondClaim.SelectedIndex = 1
            tbCheckPlafondBy1Claim.Enabled = False
            tbCheckPlafondBy1Claim.Text = 0

            tbMedicalCode.Focus()


        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
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

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbMedicalName.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            'kecuali mode bank maka akan tersimpan kosong            

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT MedicalCode From MsMedical WHERE MedicalCode = " + QuotedStr(tbMedicalCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Medical " + QuotedStr(tbMedicalCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "Insert into MsMedical (MedicalCode, MedicalName, MedicalGroup, Gender, FgClaimFamily, Leadtime, FgMaxTaken, MaxTaken, JoinMedical, FgCheckPlafond, FgShowSlip, FgAllJoblevel, FgCheck1Claim, Plafond1Claim, UserID, UserDate) " + _
                "SELECT " + QuotedStr(tbMedicalCode.Text) + "," + QuotedStr(tbMedicalName.Text) + "," + QuotedStr(ddlMedicalGroup.SelectedValue) + ", " + QuotedStr(ddlGender.SelectedValue) + "," + _
                QuotedStr(ddlClaim.SelectedValue) + "," + QuotedStr(tbLeadTime.Text.Replace(",", "")) + "," + _
                QuotedStr(ddlMaxTaken.Text) + ", " + QuotedStr(tbMaxTaken.Text.Replace(",", "")) + "," + QuotedStr(tbJoinMedical.Text) + ", " + QuotedStr(ddlCheckPlafond.Text) + "," + _
                QuotedStr(ddlShowSlip.SelectedValue) + "," + QuotedStr(ddlJobLevel.SelectedValue) + "," + QuotedStr(ddlCheckPlafondClaim.Text) + ", " + QuotedStr(tbCheckPlafondBy1Claim.Text.Replace(",", "")) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"


            Else
                SqlString = "Update MsMedical set MedicalName =" + QuotedStr(tbMedicalName.Text) + ", " + _
                            "MedicalGroup =" + QuotedStr(ddlMedicalGroup.SelectedValue) + ", Gender =" + QuotedStr(ddlGender.SelectedValue) + ", " + _
                            "FgClaimFamily =" + QuotedStr(ddlClaim.SelectedValue) + ", Leadtime =" + QuotedStr(tbLeadTime.Text.Replace(",", "")) + ", " + _
                            "FgMaxTaken = " + QuotedStr(ddlMaxTaken.SelectedValue) + ", " + _
                            "MaxTaken =" + QuotedStr(tbMaxTaken.Text.Replace(",", "")) + ", " + _
                            "JoinMedical =" + QuotedStr(tbJoinMedical.Text) + ", FgCheckPlafond =" + QuotedStr(ddlCheckPlafond.SelectedValue) + ", FgShowSlip =" + QuotedStr(ddlShowSlip.SelectedValue) + ", " + _
                            "FgAllJoblevel =" + QuotedStr(ddlJobLevel.SelectedValue) + ", FgCheck1Claim =" + QuotedStr(ddlCheckPlafondClaim.SelectedValue) + ", Plafond1Claim =" + QuotedStr(tbCheckPlafondBy1Claim.Text.Replace(",", "")) + " " + _
                            "where MedicalCode = " & QuotedStr(tbMedicalCode.Text)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Private Function cekInput() As Boolean
        Try
            If tbMedicalCode.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Medical Code must be filled');</script>"
                tbMedicalCode.Focus()
                Exit Function
            End If
            If tbMedicalName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Medical  Name must be filled');</script>"
                tbMedicalName.Focus()
                Exit Function
            End If

            If ddlMedicalGroup.Text.Trim = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Medical Group must be filled');</script>"
                ddlMedicalGroup.Focus()
                Exit Function
            End If

            If Not IsNumeric(tbLeadTime.Text) Then
                lstatus.Text = "<script language='javascript'>alert('Lead Time must be filled');</script>"
                tbLeadTime.Focus()
            End If

            If CFloat(tbLeadTime.Text) = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Lead Time must be filled');</script>"
                tbLeadTime.Focus()
                Exit Function
            End If

            If ddlMaxTaken.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Max Taken Time must be filled');</script>"
                ddlMaxTaken.Focus()
                Exit Function
            End If

            If ddlMaxTaken.SelectedValue = "Y" And CFloat(tbMaxTaken.Text) <= 0 Then
                lstatus.Text = "<script language='javascript'>alert('Max Taken Time must be filled');</script>"
                tbMaxTaken.Focus()
                Exit Function
            End If

            If ddlCheckPlafond.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Max Taken Time must be filled');</script>"
                ddlCheckPlafond.Focus()
                Exit Function
            End If

            If ddlCheckPlafondClaim.SelectedValue = "Y" And CFloat(tbCheckPlafondBy1Claim.Text) <= 0 Then
                lstatus.Text = "<script language='javascript'>alert('Check Plafond By 1 Claim must be filled');</script>"
                ddlCheckPlafond.Focus()
                Exit Function
            End If

            If ddlShowSlip.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Show Slip must be filled');</script>"
                ddlShowSlip.Focus()
                Exit Function
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub ddlMaxTaken_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaxTaken.SelectedIndexChanged
        Try
            If ddlMaxTaken.SelectedValue = "Y" Then
                tbMaxTaken.Enabled = True
            ElseIf ddlMaxTaken.SelectedValue = "N" Then
                tbMaxTaken.Enabled = False
                tbMaxTaken.Text = "0"
            End If

        Catch ex As Exception
            lstatus.Text = "Mode Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCheckPlafondClaim_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCheckPlafondClaim.SelectedIndexChanged
        Try
            If ddlCheckPlafondClaim.SelectedValue = "Y" Then
                tbCheckPlafondBy1Claim.Enabled = True
            ElseIf ddlCheckPlafondClaim.SelectedValue = "N" Then
                tbCheckPlafondBy1Claim.Enabled = False
                tbCheckPlafondBy1Claim.Text = "0"
            End If
        Catch ex As Exception
            lstatus.Text = "Mode Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal MedicalCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = " select A.*, B.MedicalGrpName As MedicalGrpName , C.MedicalName As JoinMedicalName" + _
            " from MsMedical A LEFT OUTER JOIN " + _
            " MsMedicalGroup B ON A.MedicalGroup = B.MedicalGrpCode LEFT OUTER JOIN " + _
            " MsMedical C ON A.JoinMedical = C.MedicalCode "

            DT = BindDataTransaction(SqlString, " A.MedicalCode = " + QuotedStr(MedicalCode), ViewState("DBConnection").ToString)

            tbMaxTaken.Enabled = False
            tbCheckPlafondBy1Claim.Enabled = False

            BindToText(tbMedicalCode, DT.Rows(0)("MedicalCode").ToString)
            BindToText(tbMedicalName, DT.Rows(0)("MedicalName").ToString)
            BindToDropList(ddlMedicalGroup, DT.Rows(0)("MedicalGroup").ToString)
            BindToDropList(ddlGender, DT.Rows(0)("Gender").ToString)
            BindToDropList(ddlClaim, DT.Rows(0)("FgClaimFamily").ToString)
            BindToText(tbLeadTime, DT.Rows(0)("LeadTime").ToString)
            BindToDropList(ddlMaxTaken, DT.Rows(0)("FgMaxTaken").ToString)
            BindToText(tbMaxTaken, DT.Rows(0)("MaxTaken").ToString)


            BindToText(tbJoinMedical, DT.Rows(0)("JoinMedical").ToString)
            BindToText(tbJoinMedicalName, DT.Rows(0)("JoinMedicalName").ToString)
            BindToDropList(ddlCheckPlafond, DT.Rows(0)("FgCheckPlafond").ToString)
            BindToDropList(ddlShowSlip, DT.Rows(0)("FgShowSlip").ToString)
            BindToDropList(ddlJobLevel, DT.Rows(0)("FgAllJobLevel").ToString)
            BindToDropList(ddlCheckPlafondClaim, DT.Rows(0)("FgCheck1Claim").ToString)
            BindToText(tbCheckPlafondBy1Claim, DT.Rows(0)("Plafond1Claim").ToString)

            'tbMaxTaken.Enabled = False
            'tbCheckPlafondBy1Claim.Enabled = False

            tbMaxTaken.Enabled = ddlMaxTaken.SelectedValue = "Y"

            tbCheckPlafondBy1Claim.Enabled = ddlCheckPlafondClaim.SelectedValue = "Y"

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        tbMedicalCode.Enabled = State
        tbMedicalName.Enabled = State
        ddlMedicalGroup.Enabled = State
        ddlGender.Enabled = State
        ddlClaim.Enabled = State
        tbLeadTime.Enabled = State
        ddlMaxTaken.Enabled = State
        tbMaxTaken.Enabled = State
        tbJoinMedical.Enabled = State
        tbJoinMedicalName.Enabled = State
        ddlCheckPlafond.Enabled = State
        ddlShowSlip.Enabled = State
        ddlJobLevel.Enabled = State
        ddlCheckPlafondClaim.Enabled = State
        tbCheckPlafondBy1Claim.Enabled = State
        btnJoinMedical.Enabled = State
        BtnSave.Visible = State
        btnReset.Visible = State
    End Sub

    Protected Sub btnJoinMedical_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJoinMedical.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM MsMedical"
            ResultField = "MedicalCode, MedicalName"
            ViewState("Sender") = "btnJoinMedical"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Join Medical Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbJoinMedical_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbJoinMedical.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Dim SQLString As String
        Try
            SQLString = "SELECT * FROM MsMedical WHERE MedicalCode = " + QuotedStr(tbJoinMedical.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                BindToText(tbJoinMedical, Dr("MedicalCode").ToString)
                BindToText(tbJoinMedicalName, Dr("MedicalName").ToString)
            Else
                tbJoinMedical.Text = ""
                tbJoinMedicalName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb Join Medical change Error : " + ex.ToString)
        End Try
    End Sub

End Class
