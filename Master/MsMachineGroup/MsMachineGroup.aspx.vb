Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsMachineGroup_MsMachineGroup
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnWorkCtrAdd" Or ViewState("Sender") = "btnWorkCtrEdit" Then
                Dim WorkCtr As TextBox
                Dim WorkCtrName As Label
                If ViewState("Sender") = "btnWorkCtrAdd" Then
                    WorkCtr = DataGrid.FooterRow.FindControl("WorkCtrAdd")
                    WorkCtrName = DataGrid.FooterRow.FindControl("WorkCtrNameAdd")
                Else
                    WorkCtr = DataGrid.Rows(DataGrid.EditIndex).FindControl("WorkCtrEdit")
                    WorkCtrName = DataGrid.Rows(DataGrid.EditIndex).FindControl("WorkCtrNameEdit")
                End If
                WorkCtr.Text = Session("Result")(0).ToString
                WorkCtrName.Text = Session("Result")(1).ToString
                WorkCtr.Focus()
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
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
            bindDataGrid()
            'tbFilter.Text = ""
            'tbfilter2.Text = ""
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
        Dim QtyMachine As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from VMsMachineGroup " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MachineGrpCode Asc "
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            QtyMachine = GVR.FindControl("QtyMachineAdd")
            QtyMachine.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "S_FormPrintMaster5 'VMsMachineGroup','MachineGrpCode','MachineGrpName','WorkCtr_Code','WorkCtr_Name','QtyMachine','Machine Group File','Machine Group Code','Machine Group Name','Work Ctr Code','Work Ctr Name','Qty Machine'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster4.frx"
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
        Dim dbCode, dbName, dbWorkCtr, dbMachine As TextBox


        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("MachineGrpCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("MachineGrpNameAdd")
                dbWorkCtr = DataGrid.FooterRow.FindControl("WorkCtrAdd")
                dbMachine = DataGrid.FooterRow.FindControl("QtyMachineAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Machine Group Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Machine Group Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If dbWorkCtr.Text.Trim.Length = 0 Then
                    lstatus.Text = "Work Center must be filled."
                    dbWorkCtr.Focus()
                    Exit Sub
                End If

                If CFloat(dbMachine.Text.Trim) = 0 Then
                    lstatus.Text = "Qty Machine must be filled."
                    dbMachine.Focus()
                    Exit Sub
                End If


                If SQLExecuteScalar("Select MachineGrpCode From VMsMachineGroup WHERE MachineGrpCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Machine Group" + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsMachineGroup SELECT " + QuotedStr(dbCode.Text) + _
                "," + QuotedStr(dbName.Text) + ", " + QuotedStr(dbWorkCtr.Text.Trim) + _
                "," + dbMachine.Text.Replace(",", "") + _
                "," + QuotedStr(ViewState("UserId").ToString) + " , getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()
            ElseIf e.CommandName = "btnWorkCtrAdd" Or e.CommandName = "btnWorkCtrEdit" Then
                Dim FieldResult As String

                If e.CommandName = "btnWorkCtrAdd" Then
                    Session("filter") = "Select * FROM VMsWorkCtr "
                    ViewState("Sender") = "btnWorkCtrAdd"
                Else
                    Session("filter") = "Select * FROM VMsWorkCtr "
                    ViewState("Sender") = "btnWorkCtrEdit"
                End If
                FieldResult = "WorkCtr_Code, WorkCtr_Name"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("MachineGrpCode")

            SQLExecuteNonQuery("Delete from MsMachineGroup where MachineGrpCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, QtyMachine As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("MachineGrpNameEdit")
            txt.Focus()

            QtyMachine = DataGrid.Rows(e.NewEditIndex).FindControl("QtyMachineEdit")
            QtyMachine.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbWorkCtr, dbQtyMachine As TextBox
        Dim lbCode As Label


        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("MachineGrpCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("MachineGrpNameEdit")
            dbWorkCtr = DataGrid.Rows(e.RowIndex).FindControl("WorkCtrEdit")
            dbQtyMachine = DataGrid.Rows(e.RowIndex).FindControl("QtyMachineEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Machine Group Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If dbWorkCtr.Text.Trim.Length = 0 Then
                lstatus.Text = "Work Center must be filled."
                dbWorkCtr.Focus()
                Exit Sub
            End If

            If CFloat(dbQtyMachine.Text.Trim) = 0 Then
                lstatus.Text = "Qty Machine must be filled."
                dbQtyMachine.Focus()
                Exit Sub
            End If

            SQLString = "Update MsMachineGroup set MachineGrpName = " + QuotedStr(dbName.Text) + _
            ",WorkCtr = " + QuotedStr(dbWorkCtr.Text) + _
            ",QtyMachine = " + dbQtyMachine.Text.Replace(",", "") + _
            " where MachineGrpCode = '" & lbCode.Text + "'"

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

    Protected Sub WorkCtrEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim WorkCtr As TextBox
        Dim WorkCtrName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            WorkCtr = dgi.FindControl("WorkCtrEdit")
            WorkCtrName = dgi.FindControl("WorkCtrNameEdit")


            ds = SQLExecuteQuery("Select WorkCtr_Code, WorkCtr_Name From VMsWorkCtr WHERE WorkCtr_Code = " + QuotedStr(WorkCtr.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                WorkCtr.Text = ""
                WorkCtrName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                WorkCtr.Text = dr("WorkCtr_Code").ToString
                WorkCtrName.Text = dr("WorkCtr_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Work Center Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub WorkCtrAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim WorkCtr, tb As TextBox
        Dim WorkCtrName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try

            tb = sender
            If tb.ID = "WorkCtrAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                WorkCtr = dgi.FindControl("WorkCtrAdd")
                WorkCtrName = dgi.FindControl("WorkCtrNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                WorkCtr = dgi.FindControl("WorkCtrEdit")
                WorkCtrName = dgi.FindControl("WorkCtrNameEdit")
            End If

            ds = SQLExecuteQuery("Select WorkCtr_Code, WorkCtr_Name From VMsWorkCtr WHERE WorkCtr_Code = " + QuotedStr(WorkCtr.Text), ViewState("DBConnection").ToString)
            'ds = SQLExecuteQuery("Select Account, Description AS AccountName From VMsAccount WHERE Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                WorkCtr.Text = ""
                WorkCtrName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                WorkCtr.Text = dr("WorkCtr_Code").ToString
                WorkCtrName.Text = dr("WorkCtr_Name").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tb Acc Changed Error : " + ex.ToString
        End Try
    End Sub


End Class
