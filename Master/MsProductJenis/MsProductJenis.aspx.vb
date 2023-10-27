Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsProductJenis_MsProductJenis
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If
        dsProductMateri.ConnectionString = ViewState("DBConnection")
        lstatus.Text = ""
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
            SqlString = "select A.JenisCode, A.JenisName,B.MateriName, A.JenisNo, A.Materi from MsProductJenis A INNER JOIN MsProductMateri B ON A.Materi = B.MateriCode " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "JenisCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
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
            'Session("SelectCommand") = "EXEC S_FormMsProductSubGroup"
            Session("SelectCommand") = "EXEC S_FormPrintMaster4 'VMsProductJenis','Type_Code','Type_Name','MateriName','JenisNo'," + QuotedStr(lblTitle.Text) + ",'Product Jenis Code','Product Jenis Name','Product Materi', 'Product Jenis No'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

            'Dim ReportGw As New ReportDocument
            'Dim Ds As DataSet
            'Dim crParameterFieldDefinitnions As ParameterFieldDefinitions
            'Dim crparameter1, crprmColumn As ParameterFieldDefinition
            'Dim crparameter1values, crprmColumnvalues As ParameterValues
            'Dim crDiscrete1Value As New ParameterDiscreteValue
            'Try
            'Ds = SQLExecuteQuery("Select A.ProductSubGrpCode AS Code, A.ProductSubGrpName AS Name, B.ProductGrpName AS Col3 from MsProductGroupSub A INNER JOIN MsProductGroup B ON A.ProductGroup = B.ProductCatCode ")
            'ReportGw.Load(Server.MapPath("~\Rpt\PrintMaster3.Rpt"))

            'ReportGw.SetDataSource(Ds.Tables(0))
            'crParameterFieldDefinitnions = ReportGw.DataDefinition.ParameterFields
            'crparameter1 = crParameterFieldDefinitnions.Item("Title")
            'crparameter1values = crparameter1.CurrentValues
            'crDiscrete1Value.Value = "Product Sub Group File"
            'crparameter1values.Add(crDiscrete1Value)
            'crparameter1.ApplyCurrentValues(crparameter1values)

            'crprmColumn = crParameterFieldDefinitnions.Item("Col3Title")
            'crprmColumnvalues = crparameter1.CurrentValues
            'crDiscrete1Value.Value = "Product Group"
            'crprmColumnvalues.Add(crDiscrete1Value)
            'crprmColumn.ApplyCurrentValues(crprmColumnvalues)

            'Session("Report") = ReportGw
            'Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
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
        Dim dbCode, dbName, dbjenisNo As TextBox
        Dim ddlMateri As DropDownList

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("JenisCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("JenisNameAdd")
                dbjenisNo = DataGrid.FooterRow.FindControl("JenisNoAdd")
                ddlMateri = DataGrid.FooterRow.FindControl("MateriAdd")

                If dbjenisNo.Text.Trim.Length = 0 Then
                    lstatus.Text = "Jenis  Code must be filled."
                    dbjenisNo.Focus()
                    Exit Sub
                End If

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Product Jenis Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Product Jenis Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Type_Code From VMsProductJenis WHERE Type_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection")).Length > 0 Then
                    lstatus.Text = "Product Jenis " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsProductJenis (JenisCode, JenisName, Materi, JenisNo, UserId, UserDate ) " + _
                " SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + _
                QuotedStr(ddlMateri.SelectedValue) + ", " + QuotedStr(dbjenisNo.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("JenisCode")

            SQLExecuteNonQuery("Delete from MsProductJenis where JenisCode = '" & txtID.Text & "'", ViewState("DBConnection"))
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim lbJenisCode As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("JenisNameEdit")
            lbJenisCode = obj.FindControl("JenisCodeEdit")
            ViewState("JenisCode") = lbJenisCode.Text
            
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbJenisNo As TextBox
        Dim lbCode As Label
        Dim ddlMateri As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("JenisCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("JenisNameEdit")
            ddlMateri = DataGrid.Rows(e.RowIndex).FindControl("MateriEdit")
            dbJenisNo = DataGrid.Rows(e.RowIndex).FindControl("JenisNoEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Product Sub Group Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsProductJenis set JenisName= " + QuotedStr(dbName.Text) + " where JenisCode = " + QuotedStr(lbCode.Text)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'If SQLExecuteScalar("SELECT Type_Code From VMsProductJenis WHERE Type_Code = " + QuotedStr(lbCode.Text), ViewState("DBConnection")).Length > 0 Then
            '    lstatus.Text = "Product Jenis " + QuotedStr(lbCode.Text) + " has already been exist"
            '    SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
            'End If
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

    Protected Sub MateriAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim dgi As GridViewRow
        Dim Count As Integer
        Dim ddlmateri As DropDownList
        Dim dbJenisCode, dbJenisNo As TextBox
        Try
            Count = DataGrid.Controls(0).Controls.Count
            dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            ddlmateri = dgi.FindControl("MateriAdd")
            dbJenisCode = dgi.FindControl("JenisCOdeAdd")
            dbJenisNo = dgi.FindControl("JenisNoAdd")
            dbJenisCode.Text = ddlmateri.Text + dbJenisNo.Text

        Catch ex As Exception
            lstatus.Text = "Product JEnis Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub MateriEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim dgi As GridViewRow
        Dim Count As Integer
        Dim ddlmateri As DropDownList
        Dim dbJenisCode As Label
        Dim dbJenisNo As TextBox
        Try
            
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            ddlmateri = dgi.FindControl("MateriEdit")
            dbJenisCode = dgi.FindControl("JenisCodeEdit")
            dbJenisNo = dgi.FindControl("JenisNoEdit")
            dbJenisCode.Text = ddlmateri.Text + dbJenisNo.Text

        Catch ex As Exception
            lstatus.Text = "Product Jenis Add Index Changed Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub JenisNoAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim Count As Integer
        Dim ddlmateri As DropDownList
        Dim dbJenisCode, dbJenisNo As TextBox
        Try

            'lstatus.Text = "1"
            'Exit Sub
            Count = DataGrid.Controls(0).Controls.Count
            dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            ddlmateri = dgi.FindControl("MateriAdd")
            dbJenisCode = dgi.FindControl("JenisCodeAdd")
            dbJenisNo = dgi.FindControl("JenisNoAdd")


            dbJenisCode.Text = ddlmateri.Text + dbJenisNo.Text



        Catch ex As Exception
            lstatus.Text = "tb materi Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub JenisNoEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim Count As Integer
        Dim ddlmateri As DropDownList
        Dim dbJenisCode, dbJenisNo As TextBox
        Try

            'lstatus.Text = "1"
            'Exit Sub
            Count = DataGrid.Controls(0).Controls.Count
            dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            ddlmateri = dgi.FindControl("MateriAdd")
            dbJenisCode = dgi.FindControl("JenisCodeAdd")
            dbJenisNo = dgi.FindControl("JenisNoAdd")


            dbJenisCode.Text = ddlmateri.Text + dbJenisNo.Text



        Catch ex As Exception
            lstatus.Text = "tb materi Changed Error : " + ex.ToString
        End Try
    End Sub
    
    Protected Sub DataGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid.SelectedIndexChanged

    End Sub
End Class
