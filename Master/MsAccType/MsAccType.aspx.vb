Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Execute_Master_MsAccType_MsAccType
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            Session("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
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
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from MsAccType " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "AccTypeCode ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            Dim ddlFgType, ddlPClass, ddlFgPLSumClass As DropDownList
            Dim GVR As GridViewRow
            Try
                GVR = DataGrid.FooterRow
                ddlFgType = GVR.FindControl("FgTypeAdd")
                ddlPClass = GVR.FindControl("PositionAdd")
                ddlFgPLSumClass = GVR.FindControl("FgPLSummaryAdd")
                If ddlFgType.SelectedIndex = 1 Then
                    ddlPClass.Visible = False
                    ddlPClass.Items.Clear()

                    ddlFgPLSumClass.Visible = True
                Else
                    ddlFgPLSumClass.Visible = False
                    ddlFgPLSumClass.Items.Clear()

                    ddlPClass.Visible = True
                End If
            Catch ex As Exception
                lstatus.Text = "CbxFgTypeAdd Error : " + vbCrLf + ex.ToString
            End Try

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster3 'MsAccType','AccTypeCode','AccTypeName','FgType','Account Type File','Account Type Code','Account Type Name','Type'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
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
        Dim dbCode, dbName As TextBox
        Dim cbxFgType As DropDownList
        Dim cbxposition As DropDownList
        Dim cbxPLSummary As DropDownList
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("AccTypeCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("AccTypeNameAdd")
                cbxFgType = DataGrid.FooterRow.FindControl("FgTypeAdd")
                cbxposition = DataGrid.FooterRow.FindControl("PositionAdd")
                cbxPLSummary = DataGrid.FooterRow.FindControl("FgPLSummaryAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Account Type Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Account Type Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT AccTypeCode From MsAccType WHERE AccTypeCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Account Type " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsAccType (AccTypeCode, AccTypeName, FgType, Position, FgPLSummary, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(cbxFgType.Text) + "," + QuotedStr(cbxposition.Text) + "," + QuotedStr(cbxPLSummary.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("AccTypeCode")

            SQLExecuteNonQuery("Delete from MsAccType where AccTypeCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim txtCode As Label

        Try
            
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)

            txtCode = obj.FindControl("AccTypeCodeEdit")

            Dim PClass As String = DirectCast(SQLExecuteScalar("SELECT Position FROM MsAccType WHERE AccTypeCode = " + QuotedStr(txtCode.Text), ViewState("DBConnection").ToString), String)
            Dim FgPLSumClass As String = DirectCast(SQLExecuteScalar("SELECT FgPLSummary FROM MsAccType WHERE AccTypeCode = " + QuotedStr(txtCode.Text), ViewState("DBConnection").ToString), String)

            txt = obj.FindControl("AccTypeNameEdit")
            txt.Focus()


            Dim ddlFgType, ddlPClass, ddlFgPLSumClass As DropDownList
            Dim GVR As GridViewRow

            GVR = DataGrid.Rows(DataGrid.EditIndex)
            ddlFgType = GVR.FindControl("FgTypeEdit")
            ddlPClass = GVR.FindControl("PositionEdit")
            ddlFgPLSumClass = GVR.FindControl("FgPLSummaryEdit")


            If ddlFgType.SelectedIndex = 1 Then
                ddlPClass.Visible = False
                ddlPClass.Items.Clear()

                ddlFgPLSumClass.Visible = True
                ddlFgPLSumClass.Items.Add("Y")
                ddlFgPLSumClass.Items.Add("N")

                ddlFgPLSumClass.SelectedValue = FgPLSumClass

            Else
                ddlFgPLSumClass.Visible = False
                ddlFgPLSumClass.Items.Clear()

                ddlPClass.Visible = True
                ddlPClass.Items.Add("AKTIVA")
                ddlPClass.Items.Add("PASIVA")

                ddlPClass.SelectedValue = PClass
            End If
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim CbxFgType As DropDownList
        Dim CbxPosition As DropDownList
        Dim CbxFgPLSummary As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("AccTypeCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("AccTypeNameEdit")
            CbxFgType = DataGrid.Rows(e.RowIndex).FindControl("FgTypeEdit")
            CbxPosition = DataGrid.Rows(e.RowIndex).FindControl("PositionEdit")
            CbxFgPLSummary = DataGrid.Rows(e.RowIndex).FindControl("FgPLSummaryEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Account Type Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsAccType set AccTypeName= " + QuotedStr(dbName.Text) + "," & _
            "FgType = " + QuotedStr(CbxFgType.Text) + ", Position = " + QuotedStr(CbxPosition.Text) + ", FgPLSummary = " + QuotedStr(CbxFgPLSummary.Text) + " where AccTypeCode = " & QuotedStr(lbCode.Text)

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

    Protected Sub FgTypeEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlFgType, ddlPClass, ddlFgPLSumClass As DropDownList
        Dim GVR As GridViewRow
        Dim txtCode As Label


        Try
            GVR = DataGrid.Rows(DataGrid.EditIndex)
            ddlFgType = GVR.FindControl("FgTypeEdit")
            ddlPClass = GVR.FindControl("PositionEdit")
            ddlFgPLSumClass = GVR.FindControl("FgPLSummaryEdit")

            txtCode = GVR.FindControl("AccTypeCodeEdit")

            Dim PClass As String = DirectCast(SQLExecuteScalar("SELECT Position FROM MsAccType WHERE AccTypeCode = " + QuotedStr(txtCode.Text), ViewState("DBConnection").ToString), String)
            Dim FgPLSumClass As String = DirectCast(SQLExecuteScalar("SELECT FgPLSummary FROM MsAccType WHERE AccTypeCode = " + QuotedStr(txtCode.Text), ViewState("DBConnection").ToString), String)

            If ddlFgType.SelectedIndex = 1 Then
                ddlPClass.Visible = False
                ddlPClass.Items.Clear()

                ddlFgPLSumClass.Visible = True
                ddlFgPLSumClass.Items.Add("Y")
                ddlFgPLSumClass.Items.Add("N")

                If FgPLSumClass = "" Then
                    ddlFgPLSumClass.SelectedIndex = 0
                Else
                    ddlFgPLSumClass.SelectedValue = FgPLSumClass
                End If


            Else
                ddlFgPLSumClass.Visible = False
                ddlFgPLSumClass.Items.Clear()

                ddlPClass.Visible = True
                ddlPClass.Items.Add("AKTIVA")
                ddlPClass.Items.Add("PASIVA")

                If PClass = "" Then
                    ddlPClass.SelectedIndex = 0
                Else
                    ddlPClass.SelectedValue = PClass
                End If

            End If
        Catch ex As Exception
            lstatus.Text = "CbxFgTypeEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub FgTypeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlFgType, ddlPClass, ddlFgPLSumClass As DropDownList
        Dim GVR As GridViewRow

        Try
            GVR = DataGrid.FooterRow
            ddlFgType = GVR.FindControl("FgTypeAdd")
            ddlPClass = GVR.FindControl("PositionAdd")
            ddlFgPLSumClass = GVR.FindControl("FgPLSummaryAdd")

            If ddlFgType.SelectedIndex = 1 Then
                ddlPClass.Visible = False
                ddlPClass.Items.Clear()

                ddlFgPLSumClass.Visible = True
                ddlFgPLSumClass.Items.Add("Y")
                ddlFgPLSumClass.Items.Add("N")

            Else
                ddlFgPLSumClass.Visible = False
                ddlFgPLSumClass.Items.Clear()

                ddlPClass.Visible = True
                ddlPClass.Items.Add("AKTIVA")
                ddlPClass.Items.Add("PASIVA")

            End If
        Catch ex As Exception
            lstatus.Text = "CbxFgTypeAdd Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Public Sub New()

    End Sub
End Class
