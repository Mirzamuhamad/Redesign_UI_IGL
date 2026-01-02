Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class TrSPTBSComplete
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim url As String
        Dim containerId As String
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim ds As New DataSet()

        Try
            If Not IsPostBack Then
                MV1.Visible = True
                MV1.ActiveViewIndex = 0
                'Menu1.Items.Item(0).Selected = True
                InitProperty()
                SetInit()
                pnlUnProcess.Visible = False
                pnlProcess.Visible = False

            End If
            lbStatus.Text = ""
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If
            lbStatus.Text = ""

            ViewState("KeyId") = Request.QueryString("KeyId")
            DT = SQLExecuteQuery("select * from MsMenu where MenuId = 'MsIssueRequestID'", ViewState("DBConnection").ToString).Tables(0)
            'If DT.Rows.Count > 0 Then
            '    Dr = DT.Rows(0)
            'Else
            '    Dr = Nothing
            'End If

            If DT.Rows.Count > 0 Then
                url = DT.Rows(0)("MenuUrl")
                containerId = DT.Rows(0)("MenuId")
            Else
                url = ""
                containerId = ""
            End If
            'ds = SQLExecuteQuery("EXEC S_PLSPTBSCompleteView " + QuotedStr(ddlField.Text), ViewState("DBConnection").ToString)

            btnSPTBS.PostBackUrl = "/IAL/" + url + "?KeyId=" + ViewState("KeyId").ToString + "&ContainerId=" + containerId
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
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

    Private Sub SetInit()
        'FillRange(ddlRangeP)
        'FillRange(ddlRangeUP)
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        'FillRange(ddlRange)
        'tbStartDate.SelectedDate = ViewState("ServerDate")
        Dim addDate As Date
        'addDate = DateAdd(DateInterval.Day, 13, tbStartDate.SelectedDate)
        tbSDate.SelectedDate = ViewState("ServerDate")
        tbEDate.SelectedDate = ViewState("ServerDate")
        FillCombo(ddlField, "EXEC S_GetPlantDivision", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
        'tbDateTeam.SelectedDate = tbStartDate.SelectedDate
        'tbExtendedDay.Text = "1"
        'FillCombo2(ddlArea, "EXEC S_GetMsAreaService", True, "AreaCode", "AreaName", ViewState("DBConnection"))
        'FillCombo(ddlTeam, "EXEC S_GetMsTeam", True, "TeamCode", "TeamName", ViewState("DBConnection"))
        'BindDataDt(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)
        'BindDataGrid(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), Format(tbEndDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)

        gvProcess.PageSize = ddlShowRecord.SelectedValue
        'BindDataGridProcess()


        'BindDataGridUnProcess()
    End Sub

    'Private Sub FillCombo2(ByRef DDL As DropDownList, ByVal SqlString As String, ByVal Nullable As Boolean, ByVal SelectValue As String, ByVal SelectText As String, ByVal DBConnection As String)

    'End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "EXEC S_PLSPTBSCompleteView " + QuotedStr(Format(tbSDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(Format(tbEDate.SelectedValue, "yyyy-MM-dd")) + ", 0, " + QuotedStr(ddlField.Text)
    End Function

    'Private Sub BindDataDt(ByVal StartDate As String, ByVal Area As String)
    '    Try
    '        Dim dt As New DataTable
    '        Dim tgl As Date
    '        Dim day As Integer
    '        Dim StrFilter As String
    '        StrFilter = GenerateFilterDlg2(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

    '        ViewState("Dt") = Nothing
    '        dt = SQLExecuteQuery("EXEC S_OPJobScheduleView " + QuotedStr(StartDate) + ", " + QuotedStr(Area) + "," + QuotedStr(StrFilter), ViewState("DBConnection").ToString).Tables(0)
    '        ViewState("Dt") = dt
    '        BindGridDt(dt, GridDt)

    '        GridDt.HeaderRow.Cells(14).Text = "Due"
    '        tgl = tbStartDate.SelectedValue
    '        day = 1
    '        While tgl <= tbEndDate.SelectedValue
    '            GridDt.HeaderRow.Cells(14 + day).Text = tgl.Day.ToString & " -" & Chr(10) & tgl.DayOfWeek.ToString.Substring(0, 3)
    '            tgl = tgl.AddDays(1)
    '            day = day + 1
    '        End While
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Private Sub BindDataRoomGrid()
    '    Dim tempDS As New DataSet()
    '    Dim DV As DataView
    '    Dim StrFilter As String

    '    StrFilter = GenerateFilterDlg2(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

    '    Try
    '        tempDS = SQLExecuteQuery("Select RoomCode, RoomName From MsRoom", ViewState("DBConnection").ToString)
    '        DV = tempDS.Tables(0).DefaultView

    '        DV.Sort = ViewState("SortExpression")
    '        RoomdGrid.DataSource = DV
    '        RoomdGrid.DataBind()
    '    Catch ex As Exception
    '        lbStatus.Text = lbStatus.Text + "BindDataGrid Error: " & ex.ToString
    '    Finally
    '    End Try
    'End Sub

    Private Sub BindDataGridProcess()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        'Dim DT As DataTable
        Dim StrFilter As String
        Dim sqlString As String

        Try
            'Return "EXEC S_PLSPTBSCompleteView " + QuotedStr(Format(tbSDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(Format(tbEDate.SelectedValue, "yyyy-MM-dd")) + ", 0, " + QuotedStr(ddlField.Text)
            StrFilter = GenerateFilterDlg2(ddlFieldP.SelectedValue, ddlField2P.SelectedValue, tbFilterP.Text, tbfilter2P.Text, ddlNotasiP.SelectedValue) ', ddlRangeP.SelectedValue, "AssignDate")
            StrFilter = StrFilter.Replace("AND", "Where")
            sqlString = "EXEC S_PLSPTBSCompleteView " + QuotedStr(Format(tbSDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(Format(tbEDate.SelectedValue, "yyyy-MM-dd")) + ", 0, " + QuotedStr(ddlField.Text) '+ QuotedStr(StrFilter)
            tempDS = SQLExecuteQuery(sqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                lbStatusProcess.Text = "No Data"
            Else
                lbStatusProcess.Text = ""
            End If

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "DateAngkut DESC"
            End If

            DV.Sort = ViewState("SortExpression")
            gvProcess.DataSource = DV
            gvProcess.DataBind()
        Catch ex As Exception
            lbStatus.Text = "BindDataGridProcess Error: " & ex.ToString
        End Try
    End Sub

    Private Sub BindDataGridUnProcess()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        'Dim DT As DataTable
        Dim StrFilter As String
        Dim sqlString As String

        Try
            'tempDS = SQLExecuteQuery("EXEC S_PLSPTBSCompleteViewUnProcess " + QuotedStr(ddlFieldUnProcess.Text), ViewState("DBConnection").ToString)
            'DV = tempDS.Tables(0).DefaultView
            'DataGridUnProcess.DataSource = DV
            'DataGridUnProcess.DataBind()

            'If ViewState("pkUnProcess") Is Nothing Then
            '    Dim a As Integer
            '    Dim pk As String
            '    Dim Pertamax As Boolean
            '    pk = ""
            '    Pertamax = True
            '    For a = 0 To DV.Table.Columns.Count - 1
            '        If Pertamax Then
            '            pk = DV.Table.Columns(a).ColumnName
            '            Pertamax = False
            '        Else
            '            pk = pk + ";" + DV.Table.Columns(a).ColumnName.Trim
            '        End If
            '    Next
            '    DataGridUnProcess.KeyFieldName = pk
            '    ViewState("pkUnProcess") = pk
            'End If

            'StrFilter = GenerateFilterDlg2(ddlFieldUP.SelectedValue, ddlField2UP.SelectedValue, tbFilterUP.Text, tbfilter2UP.Text, ddlNotasiUP.SelectedValue) ', ddlRangeUP.SelectedValue, "AssignDate")
            StrFilter = StrFilter.Replace("AND", "Where")
            ' sqlString = "EXEC S_PLSPTBSCompleteViewUnProcess " + QuotedStr(ddlFieldUnProcess.Text) + "," + QuotedStr(StrFilter)
            tempDS = SQLExecuteQuery(sqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                lbStatusUnProcess.Text = "No Data"
            Else
                lbStatusUnProcess.Text = ""
            End If

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "DateAngkut DESC"
            End If

            DV.Sort = ViewState("SortExpression")
            gvUnProcess.DataSource = DV
            gvUnProcess.DataBind()
        Catch ex As Exception
            lbStatus.Text = "BindDataGridUnProcess Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DrawingCell(ByVal cell As Integer, ByVal Type As String, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'Select Case Type
        '    Case "I"
        '        e.Row.Cells(cell).BackColor = System.Drawing.Color.Yellow
        '    Case "T"
        '        e.Row.Cells(cell).BackColor = System.Drawing.Color.Gray
        '    Case "S1"
        '        e.Row.Cells(cell).BackColor = System.Drawing.Color.Red
        'End Select
    End Sub

    Public Function GenerateFilterDlg2(ByVal Field1 As String, ByVal Field2 As String, ByVal Filter1 As String, ByVal Filter2 As String, ByVal Notasi As String) As String
        Dim StrFilter As String
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                If Filter2.Trim.Length > 0 Then
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%' " + _
                    Notasi + " " + Field2.Replace(" ", "_") + " like '%" + Filter2 + "%'"
                Else
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%'"
                End If
            Else
                StrFilter = ""
            End If
            If StrFilter <> "" Then
                StrFilter = "AND " + StrFilter
            End If

            'StrFilter = "( " + StrFilter + " )"
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterDlg Error : " + ex.ToString)
        End Try
    End Function


    Protected Sub ddlField_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlField.SelectedIndexChanged
        gvProcess.PageSize = ddlShowRecord.SelectedValue
        BindDataGridProcess()
        'Dim tempDS As New DataSet()

        'tempDS = SQLExecuteQuery("EXEC S_PLSPTBSCompleteView " + QuotedStr(ddlField.Text), ViewState("DBConnection").ToString)

        'DataGrid.DataSource = tempDS.Tables(0)
        'DataGrid.DataBind()
    End Sub

    'Protected Sub ddlFieldUnProcess_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFieldUnProcess.SelectedIndexChanged
    '    gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
    '    BindDataGridUnProcess()
    'End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.ServerClick
        Dim Result As String
        'Dim Fields() As String
        Dim GVR As GridViewRow
        Dim HaveCheck As Boolean
        Dim CB As CheckBox
        Dim Nmbr, ReceiveRemark As String
        Dim ReceiveDate As DateTime
        Try
            HaveCheck = False
            For Each GVR In gvProcess.Rows
                CB = GVR.FindControl("cbSelect")
                If CB.Checked = True Then
                    HaveCheck = True
                End If
            Next

            If HaveCheck = True Then
                For Each GVR In gvProcess.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked = True Then
                        Nmbr = GVR.Cells(1).Text
                        ReceiveDate = GVR.Cells(2).Text
                        ReceiveRemark = GVR.Cells(13).Text

                        Result = ExecSPProcess(Nmbr, ReceiveDate, ReceiveRemark)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If

                    End If
                Next
            Else
                lbStatus.Text = MessageDlg("Data must have at least 1 selected")
                Exit Sub
            End If

            gvProcess.PageSize = ddlShowRecord.SelectedValue
            BindDataGridProcess()

            ' gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
            ' BindDataGridUnProcess()
        Catch ex As Exception
            lbStatus.Text = "btnProcess_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnUnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnProcess.ServerClick
    '    Dim Result As String
    '    'Dim Fields() As String
    '    Dim GVR As GridViewRow
    '    Dim HaveCheck As Boolean
    '    Dim CB As CheckBox
    '    Dim RequestNo, Product As String

    '    Try
    '        'Dim list As Collections.Generic.List(Of Object) = DataGridUnProcess.GetSelectedFieldValues(ViewState("pkUnProcess"))

    '        'If list.Count = 0 Then
    '        '    lbStatus.Text = MessageDlg("Data must have at least 1 selected")
    '        '    DataGridUnProcess.Focus()
    '        '    Exit Sub
    '        'End If

    '        'For i As Integer = 0 To list.Count - 1
    '        '    'currID = list.Item(0) 'Index 0 means first fields passed to the function GetSelectedFieldValues
    '        '    'Now you can do what you want to do to the current returned ID

    '        '    Fields = list.Item(i).ToString.Split("|")
    '        '    RequestNo = Fields(13)
    '        '    Product = Fields(3)

    '        '    Result = ExecSPUnProcess(ddlFieldUnProcess.SelectedItem.Text, RequestNo, Product)
    '        '    If Trim(Result) <> "" Then
    '        '        lbStatus.Text = lbStatus.Text + Result + " <br/>"
    '        '    End If
    '        'Next
    '        'DataGridUnProcess.Selection.UnselectAll()
    '        'BindDataGridUnProcess()

    '        HaveCheck = False
    '        For Each GVR In gvUnProcess.Rows
    '            CB = GVR.FindControl("cbSelect")
    '            If CB.Checked = True Then
    '                HaveCheck = True
    '            End If
    '        Next

    '        If HaveCheck = True Then
    '            For Each GVR In gvUnProcess.Rows
    '                CB = GVR.FindControl("cbSelect")
    '                If CB.Checked = True Then
    '                    RequestNo = GVR.Cells(1).Text
    '                    Product = GVR.Cells(5).Text

    '                    Result = ExecSPUnProcess(ddlFieldUnProcess.SelectedItem.Text, RequestNo, Product)
    '                    If Trim(Result) <> "" Then
    '                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
    '                    End If

    '                    'USERLOG
    '                    SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ", '', " + QuotedStr(ddlFieldUnProcess.SelectedItem.Text + "," + RequestNo + "," + Product) + ", " + _
    '                                        QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr("UnProcess") + ", ''", ViewState("DBConnection").ToString)
    '                End If
    '            Next
    '        Else
    '            lbStatus.Text = MessageDlg("Data must have at least 1 selected")
    '            Exit Sub
    '        End If

    '        gvProcess.PageSize = ddlShowRecord.SelectedValue
    '        BindDataGridProcess()

    '        ' gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
    '        BindDataGridUnProcess()
    '    Catch ex As Exception
    '        lbStatus.Text = "btnUnProcess_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Public Function ExecSPProcess(ByVal Nmbr As String, ByVal ReceiveDate As DateTime, ByVal ReceiveRemark As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_PLSPTBSComplete " + QuotedStr(Nmbr) + ", " + QuotedStr(ReceiveDate) + ", " + QuotedStr(ReceiveRemark) + ", " + QuotedStr(ViewState("GLYear")) + ", " + QuotedStr(ViewState("GLPeriod")) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                            "SELECT @A;"
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecSPProcess Error : " + ex.ToString)
        End Try
    End Function

    Public Function ExecSPUnProcess(ByVal Type As String, ByVal RequestNo As String, ByVal Product As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_STRequestVerifikasiUnProcess " + QuotedStr(Type) + ", " + QuotedStr(RequestNo) + ", " + QuotedStr(Product) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                            "SELECT @A;"
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecSPUnProcess Error : " + ex.ToString)
        End Try
    End Function

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Insert" Then
                If ViewState("FgInsert") = "N" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MV1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Menu1.Items(0).Selected = True Then
            btnProcess.Visible = True
        Else
            btnProcess.Visible = False
        End If
        btnUnProcess.Visible = btnProcess.Visible
        'Dim i As Integer
        'For i = 0 To Menu1.Items.Count - 1
        '    If i = e.Item.Value Then

        '    ElseIf Menu1.Items(0).Selected = True Then
        '        'Dim GVR As GridViewRow = Nothing
        '        'Try
        '        '    If CheckMenuLevel("Insert") = False Then
        '        '        Exit Sub
        '        '    End If
        '        'Catch ex As Exception
        '        '    lbStatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
        '        'End Try
        '        BindDataGridProcess()
        '    ElseIf Menu1.Items(1).Selected = True Then
        '        BindDataGridUnProcess()
        '    End If
        'Next
    End Sub

    'Protected Sub DataGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles DataGrid.ProcessColumnAutoFilter
    '    BindDataGridProcess()
    'End Sub

    'Protected Sub DataGridUnProcess_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles DataGridUnProcess.ProcessColumnAutoFilter
    '    BindDataGridUnProcess()
    'End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(gvProcess, sender)
    End Sub

    Protected Sub cbSelectHdUnProcess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(gvUnProcess, sender)
    End Sub

    Protected Sub btnExpandP_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpandP.ServerClick
        Try
            tbfilter2P.Text = ""
            If pnlSearchP.Visible Then
                pnlSearchP.Visible = False
            Else
                pnlSearchP.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btnExpandP_ServerClick Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnExpandUP_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpandUP.ServerClick
    '    Try
    '        tbfilter2UP.Text = ""
    '        If pnlSearchUP.Visible Then
    '            pnlSearchUP.Visible = False
    '        Else
    '            pnlSearchUP.Visible = True
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "btnExpandUP_ServerClick Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnSearchP_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchP.ServerClick
        Try
            gvProcess.PageIndex = 0
            'Session("AdvanceFilter") = ""
            gvProcess.PageSize = ddlShowRecord.SelectedValue
            BindDataGridProcess()
            pnlUnProcess.Visible = True
            pnlProcess.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btnSearchP_ServerClick Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnSearchUP_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchUP.ServerClick
    '    Try
    '        gvUnProcess.PageIndex = 0
    '        'Session("AdvanceFilter") = ""
    '        ' gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
    '        BindDataGridUnProcess()
    '    Catch ex As Exception
    '        lbStatus.Text = "btnSearchUP_ServerClick Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub gvProcess_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProcess.PageIndexChanging
        gvProcess.PageIndex = e.NewPageIndex
        If gvProcess.EditIndex <> -1 Then
            gvProcess_RowCancelingEdit(Nothing, Nothing)
        End If
        gvProcess.PageSize = ddlShowRecord.SelectedValue
        BindDataGridProcess()
    End Sub

    Protected Sub gvProcess_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvProcess.RowCancelingEdit
        Try
            gvProcess.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            gvProcess.EditIndex = -1
            gvProcess.PageSize = ddlShowRecord.SelectedValue
            BindDataGridProcess()
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub gvUnProcess_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUnProcess.PageIndexChanging
        gvUnProcess.PageIndex = e.NewPageIndex
        If gvUnProcess.EditIndex <> -1 Then
            gvUnProcess_RowCancelingEdit(Nothing, Nothing)
        End If
        'gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
        BindDataGridUnProcess()
    End Sub

    Protected Sub gvUnProcess_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvUnProcess.RowCancelingEdit
        Try
            gvUnProcess.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            gvUnProcess.EditIndex = -1
            '   gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
            BindDataGridUnProcess()
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub gvProcess_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvProcess.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            gvProcess.PageSize = ddlShowRecord.SelectedValue
            BindDataGridProcess()
        Catch ex As Exception
            lbStatus.Text = "Grid Process 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub gvUnProcess_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvUnProcess.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            '   gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
            BindDataGridUnProcess()
        Catch ex As Exception
            lbStatus.Text = "Grid UnProcess 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        Try
            gvProcess.PageSize = ddlShowRecord.SelectedValue
            BindDataGridProcess()
        Catch ex As Exception
            lbStatus.Text = "ddlShowRecord_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlShowRecord2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord2.SelectedIndexChanged
    '    Try
    '        '  gvUnProcess.PageSize = ddlShowRecord2.SelectedValue
    '        BindDataGridUnProcess()
    '    Catch ex As Exception
    '        lbStatus.Text = "ddlShowRecord2_SelectedIndexChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnTotalComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTotalComplete.Click
        Try
            TotSPTBS()
        Catch ex As Exception
            lbStatus.Text = "btnTotalComplete_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub TotSPTBS()
        Dim TotNormal, TotAbnormal, TotBrd As Double
        Dim TotTPHNormal, TotTPHAbnormal, TotTPHBrd As Double
        Dim TotDiffNormal, TotDiffAbnormal, TotDiffBrd As Double

        Dim GVR As GridViewRow
        Dim HaveCheck As Boolean
        Dim CB As CheckBox
        Dim SPTBSNo, CarNo As String
        Dim tbNormalC, tbAbNormalC, tbBrondolanC, tbActNormalC, tbActAbNormalC, tbActBrondolanC, tbDiffNormalC, tbDiffAbNormalC, tbDiffBrondolanC As TextBox
        Try
            HaveCheck = False
            For Each GVR In gvProcess.Rows
                CB = GVR.FindControl("cbSelect")
                If CB.Checked = True Then
                    HaveCheck = True
                End If
            Next
            TotNormal = 0
            TotAbnormal = 0
            TotBrd = 0
            TotTPHNormal = 0
            TotTPHAbnormal = 0
            TotTPHBrd = 0
            TotDiffNormal = 0
            TotDiffAbnormal = 0
            TotDiffBrd = 0
            If HaveCheck = True Then
                For Each GVR In gvProcess.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked = True Then
                        SPTBSNo = GVR.Cells(1).Text
                        CarNo = GVR.Cells(3).Text
                        tbNormalC = GVR.FindControl("tbNormalC")
                        tbAbNormalC = GVR.FindControl("tbAbNormalC")
                        tbBrondolanC = GVR.FindControl("tbBrondolanC")

                        tbActNormalC = GVR.FindControl("tbActNormalC")
                        tbActAbNormalC = GVR.FindControl("tbActAbNormalC")
                        tbActBrondolanC = GVR.FindControl("tbActBrondolanC")

                        tbDiffNormalC = GVR.FindControl("tbDiffNormalC")
                        tbDiffAbNormalC = GVR.FindControl("tbDiffAbNormalC")
                        tbDiffBrondolanC = GVR.FindControl("tbDiffBrondolanC")

                        TotNormal = TotNormal + (tbNormalC.Text)
                        TotAbnormal = TotAbnormal + (tbAbNormalC.Text)
                        TotBrd = TotBrd + (tbBrondolanC.Text)

                        TotTPHNormal = TotTPHNormal + (tbActNormalC.Text)
                        TotTPHAbnormal = TotTPHAbnormal + (tbActAbNormalC.Text)
                        TotTPHBrd = TotBrd + (tbActBrondolanC.Text)

                        TotDiffNormal = TotDiffNormal + (tbDiffNormalC.Text)
                        TotDiffAbnormal = TotDiffAbnormal + (tbDiffAbNormalC.Text)
                        TotDiffBrd = TotDiffBrd + (tbDiffBrondolanC.Text)

                    End If
                Next
            Else
                lbStatus.Text = MessageDlg("Data must have at least 1 selected")
                Exit Sub
            End If
            tbNormal.Text = TotNormal
            tbAbNormal.Text = TotAbnormal
            tbBrd.Text = TotBrd

            tbNormal2.Text = TotTPHNormal
            tbAbNormal2.Text = TotTPHAbnormal
            tbBrd2.Text = TotTPHBrd

            tbNormal3.Text = TotDiffNormal
            tbAbNormal3.Text = TotDiffAbnormal
            tbBrd3.Text = TotDiffBrd


        Catch ex As Exception
            lbStatus.Text = "btnTotalComplete_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnTotalUnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTotalUnComplete.Click
        Try
            TotUnSPTBS()
        Catch ex As Exception

        End Try
    End Sub

    Sub TotUnSPTBS()
        Dim TotNormal, TotAbnormal, TotBrd As Double
        Dim TotTPHNormal, TotTPHAbnormal, TotTPHBrd As Double
        Dim TotDiffNormal, TotDiffAbnormal, TotDiffBrd As Double

        Dim GVR As GridViewRow
        Dim HaveCheck As Boolean
        Dim CB As CheckBox
        Dim SPTBSNo, CarNo As String
        Dim tbNormalC, tbAbNormalC, tbBrondolanC, tbActNormalC, tbActAbNormalC, tbActBrondolanC, tbDiffNormalC, tbDiffAbNormalC, tbDiffBrondolanC As TextBox
        Try
            HaveCheck = False
            For Each GVR In gvUnProcess.Rows
                CB = GVR.FindControl("cbSelect")
                If CB.Checked = True Then
                    HaveCheck = True
                End If
            Next
            TotNormal = 0
            TotAbnormal = 0
            TotBrd = 0
            TotTPHNormal = 0
            TotTPHAbnormal = 0
            TotTPHBrd = 0
            TotDiffNormal = 0
            TotDiffAbnormal = 0
            TotDiffBrd = 0
            If HaveCheck = True Then
                For Each GVR In gvUnProcess.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked = True Then
                        SPTBSNo = GVR.Cells(1).Text
                        CarNo = GVR.Cells(3).Text
                        tbNormalC = GVR.FindControl("tbNormalU")
                        tbAbNormalC = GVR.FindControl("tbAbNormalU")
                        tbBrondolanC = GVR.FindControl("tbBrondolanU")

                        tbActNormalC = GVR.FindControl("tbActNormalU")
                        tbActAbNormalC = GVR.FindControl("tbActAbNormalU")
                        tbActBrondolanC = GVR.FindControl("tbActBrondolanU")

                        tbDiffNormalC = GVR.FindControl("tbDiffNormalU")
                        tbDiffAbNormalC = GVR.FindControl("tbDiffAbNormalU")
                        tbDiffBrondolanC = GVR.FindControl("tbDiffBrondolanU")

                        TotNormal = TotNormal + (tbNormalC.Text)
                        TotAbnormal = TotAbnormal + (tbAbNormalC.Text)
                        TotBrd = TotBrd + (tbBrondolanC.Text)

                        TotTPHNormal = TotTPHNormal + (tbActNormalC.Text)
                        TotTPHAbnormal = TotTPHAbnormal + (tbActAbNormalC.Text)
                        TotTPHBrd = TotBrd + (tbActBrondolanC.Text)

                        TotDiffNormal = TotDiffNormal + (tbDiffNormalC.Text)
                        TotDiffAbnormal = TotDiffAbnormal + (tbDiffAbNormalC.Text)
                        TotDiffBrd = TotDiffBrd + (tbDiffBrondolanC.Text)

                    End If
                Next
            Else
                lbStatus.Text = MessageDlg("Data must have at least 1 selected")
                Exit Sub
            End If
            tbUnNormal.Text = TotNormal
            tbUnAbNormal.Text = TotAbnormal
            tbUnbrd.Text = TotBrd

            tbUnNormal2.Text = TotTPHNormal
            tbUnAbNormal2.Text = TotTPHAbnormal
            tbUnbrd2.Text = TotTPHBrd

            tbUnNormal3.Text = TotDiffNormal
            tbUnAbNormal3.Text = TotDiffAbnormal
            tbUnbrd3.Text = TotDiffBrd


        Catch ex As Exception
            lbStatus.Text = "btnTotalUnComplete_Click Error : " + ex.ToString
        End Try
    End Sub

End Class
