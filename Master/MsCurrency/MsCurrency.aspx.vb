Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsCurrency_MsCurrency
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
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
        Dim ToleransiValue As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT A.CurrCode, A.CurrName, A.FgHome, A.FgBook, A.DigitDecimal, dbo.FORMATFloat(A.valuetoleransi, 0) AS valuetoleransi, A.UserId, A.UserDate FROM MsCurrency A " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CurrCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            ToleransiValue = GVR.FindControl("ValueToleransiAdd")
            ToleransiValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim ToleransiValue As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("CurrNameEdit")
            txt.Focus()

            ToleransiValue = DataGrid.Rows(e.NewEditIndex).FindControl("ValueToleransiEdit")
            ToleransiValue.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString, HomeCurr As String
        Dim dbName, dbToleransi As TextBox
        Dim ddlDigit, ddlHome, ddlBook As DropDownList
        Dim lbCode As Label
        Dim GVR As GridViewRow
        Try

            GVR = DataGrid.Rows(e.RowIndex)

            lbCode = GVR.FindControl("CurrCodeEdit")
            dbName = GVR.FindControl("CurrNameEdit")
            dbToleransi = GVR.FindControl("ValueToleransiEdit")
            ddlDigit = GVR.FindControl("DigitDecimalEdit")
            ddlHome = GVR.FindControl("FgHomeEdit")
            ddlBook = GVR.FindControl("FgBookEdit")


            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Currency Name must be filled."
                dbName.Focus()
                Exit Sub
            End If
            If dbToleransi.Text.Trim.Length = 0 Then
                lstatus.Text = "Toleransi Value must be filled."
                dbToleransi.Focus()
                Exit Sub
            End If
            If Not IsNumeric(dbToleransi.Text) Then
                lstatus.Text = "Toleransi Value must in correct format."
                dbToleransi.Focus()
                Exit Sub
            End If

            HomeCurr = SQLExecuteScalar("Select COALESCE(CurrCode,'') From MsCurrency WHERE FgHome ='Y'", ViewState("DBConnection").ToString)

            If HomeCurr <> lbCode.Text And HomeCurr.Length > 0 Then
                If ddlHome.SelectedValue = "Y" Then
                    lstatus.Text = "Only 1 home currencye allowed."
                    ddlHome.Focus()
                    Exit Sub
                End If
            End If


            SQLString = "Update MsCurrency set CurrName= '" + dbName.Text.Replace("'", "''") + _
            "', ValueToleransi=" + dbToleransi.Text.Replace(",", "") + ", DigitDecimal=" + ddlDigit.SelectedValue + _
            ", FgHome =" + QuotedStr(ddlHome.SelectedValue) + ", FgBook =" + QuotedStr(ddlBook.SelectedValue) + _
            " where CurrCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString, HomeCurr As String
        Dim dbCode, dbName, dbToleransi As TextBox
        Dim ddlDigit, ddlHome, ddlBook As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("CurrCodeAdd")
                dbName = GVR.FindControl("CurrNameAdd")
                dbToleransi = GVR.FindControl("ValueToleransiAdd")
                ddlDigit = GVR.FindControl("DigitDecimalAdd")
                ddlHome = GVR.FindControl("FgHomeAdd")
                ddlBook = GVR.FindControl("FgBookAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Currency Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Currency Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If
                If dbToleransi.Text.Trim.Length = 0 Then
                    lstatus.Text = "Toleransi Value Name must be filled."
                    dbToleransi.Focus()
                    Exit Sub
                End If
                If SQLExecuteScalar("SELECT Currency From VMsCurrency  WHERE Currency = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Currency " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                HomeCurr = SQLExecuteScalar("Select COALESCE(CurrCode,'') From MsCurrency WHERE FgHome ='Y'", ViewState("DBConnection").ToString)

                If ddlHome.SelectedValue = "Y" Then
                    If HomeCurr.Trim.Length > 0 Then
                        lstatus.Text = "Only 1 home currency allowed."
                        ddlHome.Focus()
                        Exit Sub
                    End If
                End If

                'insert the new entry
                SQLString = "Insert into MsCurrency(Currcode, CurrName, FgHome, DigitDecimal, ValueToleransi, FgBook, UserId, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + _
                QuotedStr(ddlHome.SelectedValue) + ", " + ddlDigit.SelectedValue + ", " + _
                dbToleransi.Text.Replace(",", "") + ", " + QuotedStr(ddlBook.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("CurrCode")

            SQLExecuteNonQuery("Delete from MsCurrency where CurrCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            'Session("SelectCommand") = "EXEC S_FormMsCountry " + StrFilter
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'VMsCurrency A','Currency','Currency_Name','Fg_Home','Digit','ValueToleransi','Currency File','Currency Code','Currency Name','Home','Digit','Toleransi'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMasterCurr.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub


End Class
