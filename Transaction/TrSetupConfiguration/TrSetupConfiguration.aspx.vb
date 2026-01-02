Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrSetupConfiguration_TrSetupConfiguration
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim DS As SqlDataReader
        Dim LI As ListItem

        If Not IsPostBack Then
            InitProperty()
            'DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            'UserLevel
            'MenuParam            
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'FillCombo(ddlGroup, "select * from v_msgroupconfiguration", True, "SetGroup", "SetGroup", ViewState("DBConnection"))
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

            

        End If
        dsAbsStatus.ConnectionString = ViewState("DBConnection")
        lstatus.Text = ""
    End Sub

    'Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '    Try
    '        If Not IsPostBack Then
    '            bindDataGrid()
    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = "error page complete load : " + ex.ToString
    '    End Try
    'End Sub

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

    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        Try
            'DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim SqlString As String
        Dim DV As DataView
        Dim tempDS As New DataSet()
        'Dim ddl As DropDownList

        Try
            SqlString = "select SetCode,setdescription,setvalue,setremark,settable from MsSetup where SetGroup=" + QuotedStr(ddlGroup.SelectedValue)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            'For Each row As GridViewRow In DataGrid.Rows
            '    ddl = row.FindControl("ddlValueEdit")
            '    ddl.Enabled = False
            'Next

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGrid)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpression")
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
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

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim code As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If

            'bindDataGrid()

            code = DataGrid.Rows(e.RowIndex).FindControl("Code")

            'SQLExecuteNonQuery("Delete from MsSetup where SetCode = '" & code.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Update MsSetup set setvalue=''where SetCode = '" & code.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        'Dim obj As GridViewRow
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim dbname, dbName2 As TextBox
        Dim code As Label
        'Dim dbname, dbName2 As String

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            'dbname = DataGrid.Rows(e.NewEditIndex).FindControl("remarkEdit") 'Cells(3).Text
            'dbName2 = DataGrid.Rows(e.NewEditIndex).FindControl("tableEdit") 'Cells(4).Text

            'dbname = DataGrid.Rows(e.NewEditIndex).Cells(3).Text
            'dbName2 = DataGrid.Rows(e.NewEditIndex).Cells(4).Text

            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()

            'obj = DataGrid.Rows(e.NewEditIndex)
            dbname = DataGrid.Rows(e.NewEditIndex).FindControl("remarkEdit")
            dbName2 = DataGrid.Rows(e.NewEditIndex).FindControl("tableEdit")
            code = DataGrid.Rows(e.NewEditIndex).FindControl("CodeEdit")

            ViewState("remark") = dbname.Text
            ViewState("tabel") = dbName2.Text
            ViewState("code") = code.Text

            If dbName2.Text = "ComboBox" Then
                txt = DataGrid.Rows(e.NewEditIndex).FindControl("DescriptionEdit")
                txt.Focus()

                txt = DataGrid.Rows(e.NewEditIndex).FindControl("ValueEdit")
                txt.Visible = False

                'ddl = DataGrid.Rows(e.NewEditIndex).FindControl("ddlValueEdit")
                'FillCombo(ddl, "select AbsStatusCode,AbsStatusName from " + dbname.Text.TrimStart.TrimEnd, True, "AbsStatusCode", "AbsStatusName", ViewState("DBConnection"))
            ElseIf dbName2.Text = "Float" Then
                txt = DataGrid.Rows(e.NewEditIndex).FindControl("DescriptionEdit")
                txt.Focus()

                txt = DataGrid.Rows(e.NewEditIndex).FindControl("ValueEdit")
                txt.Attributes.Add("OnKeyDown", "return PressNumeric();")

                ddl = DataGrid.Rows(e.NewEditIndex).FindControl("ddlValueEdit")
                ddl.Visible = False
            ElseIf dbName2.Text = "String" Then
                txt = DataGrid.Rows(e.NewEditIndex).FindControl("DescriptionEdit")
                txt.Focus()

                txt = DataGrid.Rows(e.NewEditIndex).FindControl("ValueEdit")
                txt.Attributes.Clear()

                ddl = DataGrid.Rows(e.NewEditIndex).FindControl("ddlValueEdit")
                ddl.Visible = False
            End If

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim desc, nilai As TextBox
        Dim code As Label
        'Dim remark, tabel As TextBox

        Try
            'bindDataGrid()

            desc = DataGrid.Rows(e.RowIndex).FindControl("DescriptionEdit")
            nilai = DataGrid.Rows(e.RowIndex).FindControl("ValueEdit")
            ddl = DataGrid.Rows(e.RowIndex).FindControl("ddlValueEdit")
            code = DataGrid.Rows(e.RowIndex).FindControl("CodeEdit")
            'remark = DataGrid.Rows(e.RowIndex).FindControl("remarkEdit")
            'tabel = DataGrid.Rows(e.RowIndex).FindControl("tableEdit")

            If ViewState("tabel") = "ComboBox" Then
                If desc.Text.Trim.Length = 0 Then
                    lstatus.Text = "Description must be filled."
                    desc.Focus()
                    Exit Sub
                End If

                SQLString = "Update MsSetup set setdescription= '" + desc.Text.Replace("'", "''") + "', setvalue= '" + ddl.SelectedValue + "' where SetCode= '" + code.Text.TrimStart.TrimEnd + "'"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            Else
                If desc.Text.Trim.Length = 0 Then
                    lstatus.Text = "Description must be filled"
                    desc.Focus()
                    Exit Sub
                End If

                If nilai.Text.Trim.Length = 0 Then
                    lstatus.Text = "Value must be filled"
                    nilai.Focus()
                    Exit Sub
                End If

                SQLString = "Update MsSetup set setdescription= '" + desc.Text.Replace("'", "''") + "', setvalue= '" + nilai.Text.Replace("'", "''") + "' where SetCode= '" + code.Text.TrimStart.TrimEnd + "'"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowUpdating exception : " + ex.ToString
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
End Class
