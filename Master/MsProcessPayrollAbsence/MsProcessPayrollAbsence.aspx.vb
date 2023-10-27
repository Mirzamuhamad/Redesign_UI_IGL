Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsProcessPayrollAbsence_MsProcessPayrollAbsence
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            'UserLevel
            'MenuParam            
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnProcessCode" Then
                tbProcessCode.Text = Session("Result")(0).ToString
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
        'Dim GVR As GridViewRow
        'Dim XTime, InMonth As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'SqlString = "Select * from V_MsProcessAbsence where ProcessCode= " + QuotedStr(tbProcessCode.Text) + " and " + StrFilter
            'SqlString = "EXEC S_PYProcessAbsenceGetDt " + QuotedStr(tbProcessCode.Text)
            SqlString = "EXEC S_PYProcessAbsenceGetDt " + QuotedStr(tbProcessCode.Text) + "," + QuotedStr(tbFilter.Text) + "," + QuotedStr(tbfilter2.Text) + "," + QuotedStr(ddlField.SelectedValue) + "," + QuotedStr(ddlField2.SelectedValue)

            'If StrFilter = "" Then
            '    SqlString = Replace(SqlString, " and ", "")
            'Else
            '    StrFilter = Replace(StrFilter, "Where", "and")
            '    SqlString = ""
            '    SqlString = "Select * from V_MsProcessAbsence where ProcessCode= " + QuotedStr(tbProcessCode.Text) + StrFilter
            'End If

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ProcessCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            'GVR = DataGrid.FooterRow
            'XTime = GVR.FindControl("XTimeAdd")
            'XTime.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'InMonth = GVR.FindControl("InMonthAdd")
            'InMonth.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim StrFilter As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("PrintType") = "Print"
    '        Session("SelectCommand") = "S_FormPrintMaster4 'VMsFrequency','FrequencyCode','FrequencyName','XTime','InMonth','Frequency','Frequency Code','Frequency Name','XTime','InMonth'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
    '        Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
    '        AttachScript("openprintdlg();", Page, Me.GetType)
    '    Catch ex As Exception
    '        lstatus.Text = "Btn Print Error : " + ex.ToString
    '    End Try
    'End Sub

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

    'Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
    '    Dim SQLString As String
    '    Dim dbCode, dbName, dbXTime, dbInMonth As TextBox
    '    Dim Validate As String = "^[a-zA-Z0-9 ]*$"
    '    Dim r As New Regex(Validate)

    '    Try
    '        If e.CommandName = "Insert" Then
    '            dbCode = DataGrid.FooterRow.FindControl("FrequencyCodeAdd")
    '            dbName = DataGrid.FooterRow.FindControl("FrequencyNameAdd")
    '            dbXTime = DataGrid.FooterRow.FindControl("XTimeAdd")
    '            dbInMonth = DataGrid.FooterRow.FindControl("InMonthAdd")

    '            If dbCode.Text.Trim.Length = 0 Then
    '                lstatus.Text = "Frequency Code must be filled."
    '                dbCode.Focus()
    '                Exit Sub
    '            End If

    '            If dbName.Text.Trim.Length = 0 Then
    '                lstatus.Text = "Frequency Name must be filled."
    '                dbName.Focus()
    '                Exit Sub
    '            End If

    '            If dbXTime.Text.Trim.Length = 0 Then
    '                lstatus.Text = "XTime must be filled."
    '                dbXTime.Focus()
    '                Exit Sub
    '            End If

    '            If dbInMonth.Text.Trim.Length = 0 Then
    '                lstatus.Text = "InMonth must be filled."
    '                dbInMonth.Focus()
    '                Exit Sub
    '            End If

    '            'If r.IsMatch(dbCode.Text) = False Then
    '            '    lstatus.Text = "Please enter valid Characters"
    '            '    dbCode.Focus()
    '            '    Exit Sub
    '            'End If

    '            'If r.IsMatch(dbName.Text) = False Then
    '            '    lstatus.Text = "Please enter valid Characters"
    '            '    dbName.Focus()
    '            '    Exit Sub
    '            'End If

    '            If SQLExecuteScalar("SELECT FrequencyCode From VMsFrequency WHERE FrequencyCode  = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
    '                lstatus.Text = "Frequency " + QuotedStr(dbName.Text) + " has already been exist"
    '                Exit Sub
    '            End If

    '            'insert the new entry
    '            SQLString = "Insert into MsFrequency (FrequencyCode, FrequencyName,XTime,InMonth, UserId, UserDate) " + _
    '            "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + _
    '            "," + QuotedStr(dbXTime.Text) + ", " + QuotedStr(dbInMonth.Text) + _
    '            "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

    '            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

    '            bindDataGrid()

    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
    '    End Try
    'End Sub

    'Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
    '    Dim txtID As Label

    '    Try
    '        If CheckMenuLevel("Delete") = False Then
    '            Exit Sub
    '        End If
    '        txtID = DataGrid.Rows(e.RowIndex).FindControl("FrequencyCode")

    '        SQLExecuteNonQuery("Delete from MsFrequency where FrequencyCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
    '        bindDataGrid()

    '    Catch ex As Exception
    '        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
    '    End Try
    'End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, DayWork, Hadir, Alpha, Sakit, Ijin, IjinAlpha, Dinas, Cuti, Lembur As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()

            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("DayWorkEdit")
            txt.Focus()

            DayWork = DataGrid.Rows(e.NewEditIndex).FindControl("DayWorkEdit")
            DayWork.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Hadir = DataGrid.Rows(e.NewEditIndex).FindControl("QtyHadirEdit")
            Hadir.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Alpha = DataGrid.Rows(e.NewEditIndex).FindControl("QtyAlphaEdit")
            Alpha.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Sakit = DataGrid.Rows(e.NewEditIndex).FindControl("QtySakitEdit")
            Sakit.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Ijin = DataGrid.Rows(e.NewEditIndex).FindControl("QtyIjinEdit")
            Ijin.Attributes.Add("OnKeyDown", "return PressNumeric();")

            IjinAlpha = DataGrid.Rows(e.NewEditIndex).FindControl("QtyIjinAlphaEdit")
            IjinAlpha.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Dinas = DataGrid.Rows(e.NewEditIndex).FindControl("QtyDinasEdit")
            Dinas.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Cuti = DataGrid.Rows(e.NewEditIndex).FindControl("QtyCutiEdit")
            Cuti.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Lembur = DataGrid.Rows(e.NewEditIndex).FindControl("TotalLemburEdit")
            Lembur.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim DayWork, Hadir, Alpha, Sakit, Ijin, IjinAlpha, Dinas, Cuti, Lembur As TextBox
        Dim lbCode, lbEmpCode As Label
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("ProcessCodeEdit")
            lbEmpCode = DataGrid.Rows(e.RowIndex).FindControl("EmpNumbEdit")

            DayWork = DataGrid.Rows(e.RowIndex).FindControl("DayworkEdit")
            Hadir = DataGrid.Rows(e.RowIndex).FindControl("QtyHadirEdit")
            Alpha = DataGrid.Rows(e.RowIndex).FindControl("QtyAlphaEdit")
            Sakit = DataGrid.Rows(e.RowIndex).FindControl("QtySakitEdit")
            Ijin = DataGrid.Rows(e.RowIndex).FindControl("QtyIjinEdit")
            IjinAlpha = DataGrid.Rows(e.RowIndex).FindControl("QtyIjinAlphaEdit")
            Dinas = DataGrid.Rows(e.RowIndex).FindControl("QtyDinasEdit")
            Cuti = DataGrid.Rows(e.RowIndex).FindControl("QtyCutiEdit")
            Lembur = DataGrid.Rows(e.RowIndex).FindControl("TotalLemburEdit")

            'dbName = DataGrid.Rows(e.RowIndex).FindControl("EmpNumbEdit")

            'If IsNumeric(Lembur.Text) = False Then
            '    lstatus.Text = "Lembur cannot be fill with letter"
            '    Lembur.Focus()
            '    Exit Sub
            'End If

            If Lembur.Text = "" Then
                Lembur.Text = "0"
            End If

            'If dbName.Text.Trim.Length = 0 Then
            '    lstatus.Text = "Frequency Name must be filled."
            '    dbName.Focus()
            '    Exit Sub
            'End If

            'If dbXTime.Text.Trim.Length = 0 Then
            '    lstatus.Text = "XTime must be filled."
            '    dbXTime.Focus()
            '    Exit Sub
            'End If

            'If dbInMonth.Text.Trim.Length = 0 Then
            '    lstatus.Text = "InMonth must be filled."
            '    dbInMonth.Focus()
            '    Exit Sub
            'End If

            'If r.IsMatch(dbName.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters"
            '    dbName.Focus()
            '    Exit Sub
            'End If

            SQLString = "Update PYProcessAbsence set DayWork='" + DayWork.Text.Replace("'", "''") + _
                        "',QtyHadir='" + Hadir.Text.Replace("'", "''") + _
                        "',QtyAlpha='" + Alpha.Text.Replace("'", "''") + _
                        "',QtySakit='" + Sakit.Text.Replace("'", "''") + _
                        "',QtyIzin='" + Ijin.Text.Replace("'", "''") + _
                        "',QtyIzinAlpha='" + IjinAlpha.Text.Replace("'", "''") + _
                        "',QtyDinas='" + Dinas.Text.Replace("'", "''") + _
                        "',QtyCuti='" + Cuti.Text.Replace("'", "''") + _
                        "',TotalLembur=" + Lembur.Text.Replace("'", "''") + _
                        " where ProcessCode = '" + lbCode.Text + "' And EmpNumb = '" + lbEmpCode.Text + "'"

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

    Protected Sub tbProcessCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProcessCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable

        Try

            DT = SQLExecuteQuery("SELECT * From PYProcessHd Where Status in ('P','C') And ProcessCode = " + QuotedStr(tbProcessCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbProcessCode.Text = Dr("ProcessCode")
            Else
                tbProcessCode.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbProcessCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProcessCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcessCode.Click
        Dim ResultField As String 'ResultSame 

        Try
            Session("Result") = Nothing
            'If ddlRefftype.SelectedValue = "Issue Request" Then
            Session("Filter") = "SELECT * From PYProcessHd Where Status in ('P','C')" 'AND Warehouse = " + QuotedStr(ddlwrhs.SelectedValue)
            'Else
            'Session("Filter") = "SELECT distinct Request_No, Request_Date, Department_Code, Department_Name, RequestBy, RemarkDt FROM V_STIssueSlipGetDataReff WHERE Request_Type = " + QuotedStr(ddlRefftype.SelectedValue)
            'End If
            ResultField = "ProcessCode, Status"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnProcessCode"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnProcessCode_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim StrFilter, SqlString, sqlstring1 As String
        Dim DS As DataSet
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            'SqlString = "EXEC S_PYProcessAbsenceGetDt " + QuotedStr(tbProcessCode.Text)
            'SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)

            'sqlstring1 = "Select * from V_MsProcessAbsence where ProcessCode= " + QuotedStr(tbProcessCode.Text) + " and " + StrFilter
            sqlstring1 = "EXEC S_PYProcessAbsenceGetDt " + QuotedStr(tbProcessCode.Text) + "," + QuotedStr(tbFilter.Text) + "," + QuotedStr(tbfilter2.Text) + "," + QuotedStr(ddlField.SelectedValue) + "," + QuotedStr(ddlField2.SelectedValue)
            'If StrFilter = "" Then
            '    sqlstring1 = Replace(sqlstring1, " and ", "")
            'Else
            '    StrFilter = Replace(StrFilter, "Where", "and")
            '    sqlstring1 = ""
            '    sqlstring1 = "Select * from V_MsProcessAbsence where ProcessCode= " + QuotedStr(tbProcessCode.Text) + StrFilter
            'End If

            DS = SQLExecuteQuery(sqlstring1, ViewState("DBConnection"))

            DataGrid.DataSource = DS.Tables(0)
            DataGrid.DataBind()
        Catch ex As Exception
            lstatus.Text = "btnGetData_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub DataGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGrid.RowDataBound
    '    DataGrid.Columns(3).HeaderText = "Work Day"
    '    e.Row.Cells(3).Visible = False
    '    e.Row.Cells(4).Visible = False
    '    e.Row.Cells(3).ColumnSpan = 2
    'End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim DayWork, Hadir, Alpha, Sakit, Ijin, IjinAlpha, Dinas, Cuti, Lembur As Label
        Dim lbCode, lbEmpCode As Label
        Dim SQLString As String
        Dim GVR As GridViewRow = Nothing

        Try
            If e.CommandName = "SetDefault" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))

                lbCode = GVR.FindControl("ProcessCode")
                lbEmpCode = GVR.FindControl("EmpNumb")

                'DayWork = GVR.FindControl("DayWorkSystem")
                Hadir = GVR.FindControl("DayHadirSystem")
                Alpha = GVR.FindControl("DayAlphaSystem")
                Sakit = GVR.FindControl("DaySakitSystem")
                Ijin = GVR.FindControl("DayIzinSystem")
                IjinAlpha = GVR.FindControl("DayIzinAplhaSystem")
                Dinas = GVR.FindControl("DayDinasSystem")
                Cuti = GVR.FindControl("DayCutiSystem")
                Lembur = GVR.FindControl("TotalLemburSystem")

                If Lembur.Text = "" Then
                    Lembur.Text = "0"
                End If

                SQLString = "Update PYProcessAbsence set QtyHadir='" + Hadir.Text.Replace("'", "''") + _
                            "',QtyAlpha='" + Alpha.Text.Replace("'", "''") + _
                            "',QtySakit='" + Sakit.Text.Replace("'", "''") + _
                            "',QtyIzin='" + Ijin.Text.Replace("'", "''") + _
                            "',QtyIzinAlpha='" + IjinAlpha.Text.Replace("'", "''") + _
                            "',QtyDinas='" + Dinas.Text.Replace("'", "''") + _
                            "',QtyCuti='" + Cuti.Text.Replace("'", "''") + _
                            "',TotalLembur=" + Lembur.Text.Replace("'", "''") + _
                            " where ProcessCode = '" + lbCode.Text + "' And EmpNumb = '" + lbEmpCode.Text + "'"

                'SQLString = "Update PYProcessAbsence set DayWork='" + DayWork.Text.Replace("'", "''") + _
                '            "',QtyHadir='" + Hadir.Text.Replace("'", "''") + _
                '            "',QtyAlpha='" + Alpha.Text.Replace("'", "''") + _
                '            "',QtySakit='" + Sakit.Text.Replace("'", "''") + _
                '            "',QtyIzin='" + Ijin.Text.Replace("'", "''") + _
                '            "',QtyIzinAlpha='" + IjinAlpha.Text.Replace("'", "''") + _
                '            "',QtyDinas='" + Dinas.Text.Replace("'", "''") + _
                '            "',QtyCuti='" + Cuti.Text.Replace("'", "''") + _
                '            "',TotalLembur=" + Lembur.Text.Replace("'", "''") + _
                '            " where ProcessCode = '" + lbCode.Text + "' And EmpNumb = '" + lbEmpCode.Text + "'"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                DataGrid.EditIndex = -1
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCommand Error : " + ex.ToString
        End Try
    End Sub
End Class
