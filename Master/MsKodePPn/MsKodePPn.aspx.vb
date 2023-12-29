Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsKodePPn_MsKodePPn
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            'UserLevel
            'MenuParam            
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
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
        Dim GVR As GridViewRow
        Dim StartNominal1, EndNominal1, StartNominal2, EndNominal2 As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from MsKodePPn " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "PPnCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            GVR = DataGrid.FooterRow
            StartNominal1 = GVR.FindControl("StartNominal1Add")
            StartNominal1.Attributes.Add("OnKeyDown", "return PressNumeric();")

            EndNominal1 = GVR.FindControl("EndNominal1Add")
            EndNominal1.Attributes.Add("OnKeyDown", "return PressNumeric();")

            StartNominal2 = GVR.FindControl("StartNominal2Add")
            StartNominal2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            EndNominal2 = GVR.FindControl("EndNominal2Add")
            EndNominal2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormMsKodePPn " + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMsKodePPn.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
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
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim PPnCode, PPnName, PPnCode1, StartNominal1, EndNominal1, PPnCode2, StartNominal2, EndNominal2 As TextBox
        Dim PPnPaid As DropDownList
        Dim StartNominal2Temp, EndNominal2Temp As String

        Try
            If e.CommandName = "Insert" Then
                PPnCode = DataGrid.FooterRow.FindControl("PPnCodeAdd")
                PPnName = DataGrid.FooterRow.FindControl("PPnNameAdd")
                PPnPaid = DataGrid.FooterRow.FindControl("PPnPaidAdd")

                PPnCode1 = DataGrid.FooterRow.FindControl("PPnCode1Add")
                StartNominal1 = DataGrid.FooterRow.FindControl("StartNominal1Add")
                EndNominal1 = DataGrid.FooterRow.FindControl("EndNominal1Add")

                PPnCode2 = DataGrid.FooterRow.FindControl("PPnCode2Add")
                StartNominal2 = DataGrid.FooterRow.FindControl("StartNominal2Add")
                EndNominal2 = DataGrid.FooterRow.FindControl("EndNominal2Add")

                If PPnCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("PPn Code must be filled")
                    PPnCode.Focus()
                    Exit Sub
                End If

                If PPnName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("PPn Name must be filled")
                    PPnName.Focus()
                    Exit Sub
                End If

                If PPnCode1.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("PPn Code 1 must be filled")
                    PPnCode1.Focus()
                    Exit Sub
                End If

                If PPnCode1.Text.Trim.Length <> 0 Then
                    If StartNominal1.Text.Trim.Length = 0 Then
                        lstatus.Text = MessageDlg("Start Nominal 1 must be filled")
                        StartNominal1.Focus()
                        Exit Sub
                    End If

                    If EndNominal1.Text.Trim.Length = 0 Then
                        lstatus.Text = MessageDlg("End Nominal 1 must be filled")
                        EndNominal1.Focus()
                        Exit Sub
                    End If

                    If CFloat(StartNominal1.Text) > CFloat(EndNominal1.Text) Then
                        lstatus.Text = MessageDlg("Start Nominal 1 cannot greater then End Nominal 1")
                        StartNominal1.Focus()
                        Exit Sub
                    End If
                End If

                If PPnCode2.Text.Trim.Length <> 0 Then
                    If StartNominal2.Text.Trim.Length = 0 Then
                        lstatus.Text = MessageDlg("Start Nominal 2 must be filled")
                        StartNominal2.Focus()
                        Exit Sub
                    End If

                    If EndNominal2.Text.Trim.Length = 0 Then
                        lstatus.Text = MessageDlg("End Nominal 2 must be filled")
                        EndNominal2.Focus()
                        Exit Sub
                    End If

                    If CFloat(StartNominal2.Text) < CFloat(EndNominal1.Text) Then
                        lstatus.Text = MessageDlg("Start Nominal 2 must be greater then End Nominal 1")
                        StartNominal2.Focus()
                        Exit Sub
                    End If

                    If CFloat(StartNominal2.Text) > CFloat(EndNominal2.Text) Then
                        lstatus.Text = MessageDlg("Start Nominal 2 cannot greater then End Nominal 2")
                        StartNominal2.Focus()
                        Exit Sub
                    End If
                End If

                If StartNominal2.Text.Trim.Length = 0 Then
                    StartNominal2Temp = 0
                Else
                    StartNominal2Temp = StartNominal2.Text
                End If

                If EndNominal2.Text.Trim.Length = 0 Then
                    EndNominal2Temp = 0
                Else
                    EndNominal2Temp = EndNominal2.Text
                End If

                If SQLExecuteScalar("SELECT PPnCode From MsKodePPn WHERE PPnCode  = " + QuotedStr(PPnCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("PPn Code " + QuotedStr(PPnCode.Text) + " has already been exist")
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsKodePPn (PPnCode, PPnName, PPnPaid, PPnCode1, StartNominal1, EndNominal1, " + _
                            "PPnCode2, StartNominal2, EndNominal2, UserId, UserDate) " + _
                            "SELECT " + QuotedStr(PPnCode.Text) + _
                            "," + QuotedStr(PPnName.Text) + _
                            "," + QuotedStr(PPnPaid.SelectedValue) + _
                            "," + QuotedStr(PPnCode1.Text) + _
                            "," + QuotedStr(StartNominal1.Text) + _
                            "," + QuotedStr(EndNominal1.Text) + _
                            "," + QuotedStr(PPnCode2.Text) + _
                            "," + QuotedStr(StartNominal2Temp) + _
                            "," + QuotedStr(EndNominal2Temp) + _
                            "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("PPnCode")

            SQLExecuteNonQuery("Delete From MsKodePPn Where PPnCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim StartNominal1, EndNominal1, StartNominal2, EndNominal2 As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("PPnNameEdit")
            txt.Focus()

            StartNominal1 = DataGrid.Rows(e.NewEditIndex).FindControl("StartNominal1Edit")
            StartNominal1.Attributes.Add("OnKeyDown", "return PressNumeric();")

            EndNominal1 = DataGrid.Rows(e.NewEditIndex).FindControl("EndNominal1Edit")
            EndNominal1.Attributes.Add("OnKeyDown", "return PressNumeric();")

            StartNominal2 = DataGrid.Rows(e.NewEditIndex).FindControl("StartNominal2Edit")
            StartNominal2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            EndNominal2 = DataGrid.Rows(e.NewEditIndex).FindControl("EndNominal2Edit")
            EndNominal2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim lbPPnCode As Label
        Dim PPnName, PPnCode1, StartNominal1, EndNominal1, PPnCode2, StartNominal2, EndNominal2 As TextBox
        Dim PPnPaid As DropDownList
        Dim StartNominal2Temp, EndNominal2Temp As String

        Try
            lbPPnCode = DataGrid.Rows(e.RowIndex).FindControl("PPnCodeEdit")
            PPnName = DataGrid.Rows(e.RowIndex).FindControl("PPnNameEdit")

            PPnPaid = DataGrid.Rows(e.RowIndex).FindControl("PPnPaidEdit")

            PPnCode1 = DataGrid.Rows(e.RowIndex).FindControl("PPnCode1Edit")
            StartNominal1 = DataGrid.Rows(e.RowIndex).FindControl("StartNominal1Edit")
            EndNominal1 = DataGrid.Rows(e.RowIndex).FindControl("EndNominal1Edit")

            PPnCode2 = DataGrid.Rows(e.RowIndex).FindControl("PPnCode2Edit")
            StartNominal2 = DataGrid.Rows(e.RowIndex).FindControl("StartNominal2Edit")
            EndNominal2 = DataGrid.Rows(e.RowIndex).FindControl("EndNominal2Edit")

            If PPnName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("PPn Name must be filled")
                PPnName.Focus()
                Exit Sub
            End If

            If PPnCode1.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("PPn Code 1 must be filled")
                PPnCode1.Focus()
                Exit Sub
            End If

            If PPnCode1.Text.Trim.Length <> 0 Then
                If StartNominal1.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Start Nominal 1 must be filled")
                    StartNominal1.Focus()
                    Exit Sub
                End If

                If EndNominal1.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("End Nominal 1 must be filled")
                    EndNominal1.Focus()
                    Exit Sub
                End If

                If CFloat(StartNominal1.Text) > CFloat(EndNominal1.Text) Then
                    lstatus.Text = MessageDlg("Start Nominal 1 cannot greater then End Nominal 1")
                    StartNominal1.Focus()
                    Exit Sub
                End If
            End If

            If PPnCode2.Text.Trim.Length <> 0 Then
                If StartNominal2.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Start Nominal 2 must be filled")
                    StartNominal2.Focus()
                    Exit Sub
                End If

                If EndNominal2.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("End Nominal 2 must be filled")
                    EndNominal2.Focus()
                    Exit Sub
                End If

                If CFloat(StartNominal2.Text) < CFloat(EndNominal1.Text) Then
                    lstatus.Text = MessageDlg("Start Nominal 2 must be greater then End Nominal 1")
                    StartNominal2.Focus()
                    Exit Sub
                End If

                If CFloat(StartNominal2.Text) > CFloat(EndNominal2.Text) Then
                    lstatus.Text = MessageDlg("Start Nominal 2 cannot greater then End Nominal 2")
                    StartNominal2.Focus()
                    Exit Sub
                End If
            End If

            If StartNominal2.Text.Trim.Length = 0 Then
                StartNominal2Temp = 0
            Else
                StartNominal2Temp = StartNominal2.Text
            End If

            If EndNominal2.Text.Trim.Length = 0 Then
                EndNominal2Temp = 0
            Else
                EndNominal2Temp = EndNominal2.Text
            End If

            SQLString = "Update MsKodePPn set PPnName=" + QuotedStr(PPnName.Text) + _
                        ", PPnPaid =" + QuotedStr(PPnPaid.SelectedValue) + _
                        ", PPnCode1 =" + QuotedStr(PPnCode1.Text) + _
                        ", StartNominal1 =" + QuotedStr(StartNominal1.Text) + _
                        ", EndNominal1 =" + QuotedStr(EndNominal1.Text) + _
                        ", PPnCode2 =" + QuotedStr(PPnCode2.Text) + _
                        ", StartNominal2 =" + QuotedStr(StartNominal2.Text) + _
                        ", EndNominal2 =" + QuotedStr(EndNominal2.Text) + _
                        " where PPnCode = " + QuotedStr(lbPPnCode.Text) + ""

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
