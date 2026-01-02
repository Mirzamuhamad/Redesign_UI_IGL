Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_MsOvertime_MsOvertime
    Inherits System.Web.UI.Page
    Dim PrevStart As Double

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If

        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            'GridDtLine.PageSize = CInt(Session("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnSave.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'FillCombo(ddlProduct, "SELECT * FROM V_STStockLotReffDt", True, "ProLoc", "ProLoc_Name", ViewState("DBConnection"))
            tbWorkPlaceCode.Attributes.Add("ReadOnly", "True")
            tbWorkPlaceName.Attributes.Add("ReadOnly", "True")
            If Not Request.QueryString("Code") Is Nothing Then
                FromTransaction()
            End If
            VisibleGrid()
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            If MultiView1.ActiveViewIndex = 0 Then
                VisibleGrid()
                bindDataGrid(tbWorkPlaceCode.Text)
            End If
            Session("AdvanceFilter") = ""
        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnWorkPlace" Then
                tbWorkPlaceCode.Text = Session("Result")(0).ToString
                tbWorkPlaceName.Text = Session("Result")(1).ToString
                VisibleGrid()
                bindDataGrid(tbWorkPlaceCode.Text)
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If        
        lstatus.Text = ""
    End Sub

    Private Sub FromTransaction()
        Dim param() As String
        Try
            param = Request.QueryString("Code").ToString.Split("|")
            tbWorkPlaceCode.Text = param(0)
        Catch ex As Exception
            Throw New Exception("Load Assigned Code Error : " + ex.ToString)
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
            'If CommandName = "Insert" Then
            'If ViewState("FgInsert") = "N" Then
            'lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            'Return False
            'Exit Function
            'End If
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

    Private Sub BindDataHd()
        'Dim SqlString As String
        'Dim tb As DataTable
        'Dim dr As DataRow
        'Try
        '    SqlString = "SELECT Year, Month, YearReff, MonthReff FROM MsAsumsiMonth " + _
        '    " WHERE Year = " + ddlYear.SelectedValue + " AND Month = " + ddlMonth.SelectedValue
        '    tb = SQLExecuteQuery(SqlString, Session("DBConnection")).Tables(0)

        '    If tb.Rows.Count > 0 Then
        '        dr = tb.Rows(0)
        '        BindToDropList(ddlYearReffExp, tb.Rows(0)("YearReff").ToString)
        '        BindToDropList(ddlMonthReffExp, tb.Rows(0)("MonthReff").ToString)
        '    Else
        '        ddlYearReffExp.SelectedValue = Session("GLYear")
        '        ddlMonthReffExp.SelectedValue = Session("GLPeriod")
        '    End If
        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "BindDataHd Error: " & ex.ToString
        'Finally
        'End Try
    End Sub

    Private Sub bindDataGrid(ByVal WorkPlace As String)
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim GVR As GridViewRow
        Dim OTHour, OTHourWork, OTHourHoli As TextBox

        Try
            SqlString = "SELECT * FROM VMsOvertime " + _
                        " WHERE WorkPlaceCode = " + QuotedStr(WorkPlace)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))
            ViewState("Dt") = tempDS.Tables(0)
            DV = tempDS.Tables(0).DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGrid)
            Else
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If

            GVR = DataGrid.FooterRow
            OTHour = GVR.FindControl("OTHourAdd")
            OTHour.Attributes.Add("OnKeyDown", "return PressNumeric();")

            OTHourWork = GVR.FindControl("OTHOurWorkingAdd")
            OTHourWork.Attributes.Add("OnKeyDown", "return PressNumeric();")

            OTHourHoli = GVR.FindControl("OTHourHolidayAdd")
            OTHourHoli.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "bindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim tbOTHour, tbOTHourWorking, tbOTHourHoliday As TextBox
        Dim GVR As GridViewRow = Nothing
        Try

            If e.CommandName = "Insert" Then
                tbOTHour = DataGrid.FooterRow.FindControl("OTHourAdd")
                tbOTHourWorking = DataGrid.FooterRow.FindControl("OTHourWorkingAdd")
                tbOTHourHoliday = DataGrid.FooterRow.FindControl("OTHourHolidayAdd")

                If CFloat(tbOTHour.Text) < 0 Then
                    lstatus.Text = MessageDlg("Hour Netto must be filled.")
                    tbOTHour.Focus()
                    Exit Sub
                End If                
                If CFloat(tbOTHourWorking.Text) <= 0 Then
                    lstatus.Text = MessageDlg("OT Hour Working must be filled.")
                    tbOTHourWorking.Focus()
                    Exit Sub
                End If
                If CFloat(tbOTHourHoliday.Text) <= 0 Then
                    lstatus.Text = MessageDlg("OT Hour Holiday must be filled.")
                    tbOTHourHoliday.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbOTHour.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Hour Netto must be in numeric.")
                    tbOTHour.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbOTHourWorking.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("OT Hour Working must be in numeric.")
                    tbOTHourWorking.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbOTHourHoliday.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("OT Hour Holiday must be in numeric.")
                    tbOTHourHoliday.Focus()
                    Exit Sub
                End If

                Dim SQL, result As String
                SQL = "DECLARE @A VARCHAR(255) EXEC S_MsOvertimeCekSave '" + tbWorkPlaceCode.Text + "', " + PrevStart.ToString + ", " + tbOTHour.Text + ", 0, @A OUT SELECT @A"
                result = SQLExecuteScalar(SQL, ViewState("DBConnection").ToString)
                If result.Length > 2 Then
                    lstatus.Text = result
                    Exit Sub
                End If

                'cek exists data
                Dim ExistRow As DataRow()
                ExistRow = ViewState("Dt").Select("WorkPlaceCode = " + QuotedStr(tbWorkPlaceCode.Text) + " AND OTHour = " + QuotedStr(tbOTHour.Text.Replace(",", "")))
                If ExistRow.Count = 0 Then
                    SQLString = "INSERT INTO MsOvertime (WorkPlace, OTHour, OTHourWorking, OTHourHoliday, UserId, UserDate) " + _
                    "SELECT " + QuotedStr(tbWorkPlaceCode.Text) + ", " + tbOTHour.Text.Replace(",", "") + ", " + _
                    tbOTHourWorking.Text.Replace(",", "") + ", " + tbOTHourHoliday.Text.Replace(",", "") + ", " + QuotedStr(ViewState("UserId")) + ", GetDate()"
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    bindDataGrid(tbWorkPlaceCode.Text)
                Else
                    lstatus.Text = MessageDlg("Data Already Exists")
                    Exit Sub
                End If

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("OTHour")

            'Dim dr() As DataRow
            'dr = ViewState("Dt").Select("OTHour = " + txtID.Text.Replace(",", "") + " AND WorkPlaceCode = " + QuotedStr(tbWorkPlaceCode.Text))
            'dr(0).Delete()
            SQLExecuteNonQuery("DELETE FROM MsOvertime WHERE OTHour = " + txtID.Text.Replace(",", "") + " AND WorkPlace = " + QuotedStr(tbWorkPlaceCode.Text), ViewState("DBConnection").ToString)

            bindDataGrid(tbWorkPlaceCode.Text)
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim OTHourWork, OTHourHoli As TextBox
        Dim lbl As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid(tbWorkPlaceCode.Text)
            obj = DataGrid.Rows(e.NewEditIndex)
            lbl = obj.FindControl("OTHourEdit")
            PrevStart = CFloat(lbl.Text)
            
            OTHourWork = DataGrid.Rows(e.NewEditIndex).FindControl("OTHourWorkingEdit")
            OTHourWork.Attributes.Add("OnKeyDown", "return PressNumeric();")

            OTHourHoli = DataGrid.Rows(e.NewEditIndex).FindControl("OTHourHolidayEdit")
            OTHourHoli.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim tbOTHourWorking, tbOTHourHoliday As TextBox
        Dim lbOTHour As Label
        Try
            lbOTHour = DataGrid.Rows(e.RowIndex).FindControl("OTHourEdit")
            tbOTHourWorking = DataGrid.Rows(e.RowIndex).FindControl("OTHourWorkingEdit")
            tbOTHourHoliday = DataGrid.Rows(e.RowIndex).FindControl("OTHourHolidayEdit")

            If CFloat(tbOTHourWorking.Text) <= 0 Then
                lstatus.Text = MessageDlg("OT Hour Working must be filled.")
                tbOTHourWorking.Focus()
                Exit Sub
            End If
            If CFloat(tbOTHourHoliday.Text) <= 0 Then
                lstatus.Text = MessageDlg("OT HOur Holiday must be filled.")
                tbOTHourHoliday.Focus()
                Exit Sub
            End If
            If IsNumeric(tbOTHourWorking.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("OT Hour Working must be in numeric.")
                tbOTHourWorking.Focus()
                Exit Sub
            End If
            If IsNumeric(tbOTHourHoliday.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("OT Hour Holiday must be in numeric.")
                tbOTHourHoliday.Focus()
                Exit Sub
            End If

            'Dim SQL, result As String
            'SQL = "DECLARE @A VARCHAR(255) EXEC S_MsOvertimeCekSave '" + tbWorkPlaceCode.Text + "', " + PrevStart.ToString + ", " + lbOTHour.Text + ", 0, @A OUT SELECT @A"
            'result = SQLExecuteScalar(SQL, ViewState("DBConnection").ToString)
            'If result.Length > 2 Then
            '    lstatus.Text = result
            '    Exit Sub
            'End If

            SQLString = "UPDATE MsOvertime SET OTHourWorking = " + tbOTHourWorking.Text.Replace(",", "") + ", OTHourHoliday = " + tbOTHourHoliday.Text.Replace(",", "") + _
            " WHERE WorkPlace = " + QuotedStr(tbWorkPlaceCode.Text) + " AND OTHour =  " + lbOTHour.Text.Replace(",", "")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid(tbWorkPlaceCode.Text)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_RowUpdating Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid(tbWorkPlaceCode.Text)
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 0 Then
                VisibleGrid()
                bindDataGrid(tbWorkPlaceCode.Text)
            End If
        Catch ex As Exception
            lstatus.Text = "Menu Item Click Error : " + ex.ToString
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
            bindDataGrid(tbWorkPlaceCode.Text)
        Catch ex As Exception
            lstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex

        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid(tbWorkPlaceCode.Text)
    End Sub

    Protected Sub btnWorkPlace_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkPlace.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM MsWorkplace ORDER BY WorkplaceCode"
            ResultField = "WorkPlaceCode, WorkPlaceName"
            ViewState("Sender") = "btnWorkPlace"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub VisibleGrid()
        Try
            If tbWorkPlaceCode.Text.Trim <> "" Then
                Panel2.Visible = True
            Else
                Panel2.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("VisibleGrid Error : " + ex.ToString)
        End Try
    End Sub
End Class
