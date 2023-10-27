Imports System.Data
Partial Class MsNotaris_MsNotaris
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

            If Left(tbCode.Text, 4) <> "N/" Then
                tbCode.Text = "N/" + tbCode.Text
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
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If

            tbName.Text = ""
            ddlTypeID.SelectedIndex = 0
            ddlGender.SelectedIndex = 0
            tbNotarisID.Text = ""
            tbAddress.Text = ""
            tbAddress2.Text = ""
            tbCity.Text = ""
            tbZipCode.Text = ""
            tbEmail.Text = ""
            tbPhone.Text = ""
            tbNpwp.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal NotCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsNotaris  WHERE NotCode = " + QuotedStr(NotCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("NotCode").ToString)
            BindToText(tbName, DT.Rows(0)("NotName").ToString)
            BindToDropList(ddlGender, DT.Rows(0)("Gender").ToString)
            BindToDropList(ddlTypeID, DT.Rows(0)("TypeId").ToString)
            BindToText(tbNotarisID, DT.Rows(0)("NotID").ToString)
            BindToText(TbKk, DT.Rows(0)("NoKK").ToString)
            BindToDropList(ddlGender, DT.Rows(0)("Gender").ToString)
            BindToText(tbAddress, DT.Rows(0)("Address1").ToString)
            BindToText(tbAddress2, DT.Rows(0)("Address2").ToString)
            BindToText(tbDesa, DT.Rows(0)("Desa").ToString)
            BindToText(tbKec, DT.Rows(0)("Kec").ToString)
            BindToText(TbKab, DT.Rows(0)("Kab").ToString)
            BindToText(tbCity, DT.Rows(0)("City").ToString)
            BindToText(tbZipCode, DT.Rows(0)("ZipCode").ToString)
            BindToText(tbEmail, DT.Rows(0)("Email").ToString)
            BindToText(tbPhone, DT.Rows(0)("Phone").ToString)
            BindToText(tbNpwp, DT.Rows(0)("NPWP").ToString)
            BindToDropList(ddlFgPT, DT.Rows(0)("FgPT").ToString)
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
            SqlString = "SELECT * From V_MsNotaris " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "NotCode DESC"
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            ddlField2.SelectedValue.Replace("JobCode", "Job_Code")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster6 'V_MsNotaris','NotCode','NotName','Gender','TypeID','NotID','Address1+'', ''+Address2+'', ''+Desa +'', ''+ Kec+'', ''+ Kab +'', ''+ City +'', Pos: ''+ ZipCode+'', Telp : ''+ Phone','Notaris File','Code','Description','Gender','TypeID','NotarisID','Alamat'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
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
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
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
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
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
                    'ElseIf DDL.SelectedValue = "Non Active" Then
                    '    Try
                    '        If CheckMenuLevel("Delete") = False Then
                    '            Exit Sub
                    '        End If
                    '        If GVR.Cells(8).Text = "N" Then
                    '            lstatus.Text = "<script language='javascript'> {alert('Job Plantation closed already')}</script>"
                    '            Exit Sub
                    '        End If
                    '        SQLExecuteNonQuery("UPDATE MsNotaris SET Fgactive = 'N' WHERE JobCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                    '        bindDataGrid()
                    '    Catch ex As Exception
                    '        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    '    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("DELETE MsNotaris WHERE NotCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                End If
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
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Job Code must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Job Name must be filled.")
                tbName.Focus()
                Return False
            End If

            If tbNotarisID.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("ID No must be filled.")
                tbNotarisID.Focus()
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString, Code, ID As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                Code = SQLExecuteScalar("SELECT NotCode FROM V_MsNotaris WHERE NotCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If Code = tbCode.Text Then
                    lstatus.Text = MessageDlg("Notaris Code " + QuotedStr(tbCode.Text) + " has already been exist")
                    Exit Sub
                End If

                ID = SQLExecuteScalar("SELECT TypeID+'|'+NotID FROM V_MsNotaris WHERE TypeID = " + QuotedStr(ddlTypeID.SelectedValue) + " AND NotID = " + QuotedStr(tbNotarisID.Text), ViewState("DBConnection").ToString)
                If ID = ddlTypeID.SelectedValue + "|" + tbNotarisID.Text Then
                    lstatus.Text = MessageDlg("Notaris With ID " + QuotedStr(tbNotarisID.Text) + " has already been exist")
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsNotaris (NotCode, NotName, Gender, TypeID, NotID, NoKK, Address1,Address2,Desa, Kec, Kab, City, ZipCode, Phone, Email, NPWP, FgPT, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(ddlGender.SelectedValue) + ", " + _
                QuotedStr(ddlTypeID.SelectedValue) + ", " + _
                QuotedStr(tbNotarisID.Text) + ", " + _
                QuotedStr(TbKk.Text) + ", " + _
                QuotedStr(tbAddress.Text) + ", " + _
                QuotedStr(tbAddress2.Text) + ", " + _
                QuotedStr(tbDesa.Text) + ", " + _
                QuotedStr(tbKec.Text) + ", " + _
                QuotedStr(TbKab.Text) + ", " + _
                QuotedStr(tbCity.Text) + ", " + _
                QuotedStr(tbZipCode.Text) + ", " + _
                QuotedStr(tbPhone.Text) + ", " + _
                QuotedStr(tbEmail.Text) + ", " + _
                QuotedStr(tbNpwp.Text) + ", " + _
                QuotedStr(ddlFgPT.SelectedValue) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "UPDATE MsNotaris SET NotName= " + QuotedStr(tbName.Text) & _
                            ", Gender = " + QuotedStr(ddlGender.Text) & _
                            ", TypeID = " + QuotedStr(ddlTypeID.Text) & _
                            ", NotID = " + QuotedStr(tbNotarisID.Text) & _
                            ", NoKK = " + QuotedStr(TbKk.Text) & _
                            ", Address1 = " + QuotedStr(tbAddress.Text) & _
                            ", Address2 = " + QuotedStr(tbAddress2.Text) & _
                            ", Desa = " + QuotedStr(tbDesa.Text) & _
                            ", Kec = " + QuotedStr(tbKec.Text) & _
                            ", Kab = " + QuotedStr(TbKab.Text) & _
                            ", City = " + QuotedStr(tbCity.Text) & _
                            ", ZipCode = " + QuotedStr(tbZipCode.Text) & _
                            ", Phone = " + QuotedStr(tbPhone.Text) & _
                            ", Email = " + QuotedStr(tbEmail.Text) & _
                            ", NPWP = " + QuotedStr(tbNpwp.Text) & _
                            ", FgPT = " + QuotedStr(ddlFgPT.SelectedValue) & _
                            " WHERE NotCode = " + QuotedStr(tbCode.Text)
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
End Class
