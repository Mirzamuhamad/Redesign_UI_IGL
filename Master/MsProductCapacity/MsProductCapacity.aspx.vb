Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_MsProductCapacity_MsProductCapacity
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
            tbProductName.Attributes.Add("ReadOnly", "True")
            If Not Request.QueryString("Code") Is Nothing Then
                FromTransaction()
            End If
            VisibleGrid()
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            If MultiView1.ActiveViewIndex = 0 Then
                VisibleGrid()
                bindDataGrid(tbProductCode.Text)
            End If
            Session("AdvanceFilter") = ""
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnProduct" Then
                tbProductCode.Text = Session("Result")(0).ToString
                tbProductName.Text = Session("Result")(1).ToString
                VisibleGrid()
                bindDataGrid(tbProductCode.Text)
            End If
            If ViewState("Sender") = "btnSearchFrom" Then
                tbFromCode.Text = Session("Result")(0).ToString
                BindToText(tbFromName, Session("Result")(1).ToString)
            End If
            If ViewState("Sender") = "btnSearchTo" Then
                tbToCode.Text = Session("Result")(0).ToString
                BindToText(tbToName, Session("Result")(1).ToString)
            End If
            If ViewState("Sender") = "btnMachineGroupAdd" Or ViewState("Sender") = "btnMachineGroupEdit" Then
                Dim MachineGroup As TextBox
                Dim MachineGroupName As Label
                If ViewState("Sender") = "btnMachineGroupAdd" Then
                    MachineGroup = DataGrid.FooterRow.FindControl("MachineGroupAdd")
                    MachineGroupName = DataGrid.FooterRow.FindControl("MachineGroupNameAdd")
                Else
                    MachineGroup = DataGrid.Rows(DataGrid.EditIndex).FindControl("MachineGroupEdit")
                    MachineGroupName = DataGrid.Rows(DataGrid.EditIndex).FindControl("MachineGroupNameEdit")
                End If
                MachineGroup.Text = Session("Result")(0).ToString
                MachineGroupName.Text = Session("Result")(1).ToString
                MachineGroup.Focus()
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
            tbProductCode.Text = param(0)
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

    Private Sub bindDataGrid(ByVal Product As String)
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim GVR As GridViewRow
        Dim CapQty, CapMachine, CycleTime, SetTime, MaxMachRun, CycleTimeMan, ManPower As TextBox

        Try
            SqlString = "SELECT * FROM VMsProductCapacity WHERE ProductCode = " + QuotedStr(Product)
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
            CapQty = GVR.FindControl("CapacityQtyAdd")
            CapQty.Attributes.Add("OnKeyDown", "return PressNumeric();")

            CapMachine = GVR.FindControl("CapacityHourAdd")
            CapMachine.Attributes.Add("OnKeyDown", "return PressNumeric();")

            CycleTime = GVR.FindControl("CycleTimeAdd")
            CycleTime.Attributes.Add("OnKeyDown", "return PressNumeric();")

            SetTime = GVR.FindControl("SettingTimeAdd")
            SetTime.Attributes.Add("OnKeyDown", "return PressNumeric();")

            MaxMachRun = GVR.FindControl("MaxMachineAdd")
            MaxMachRun.Attributes.Add("OnKeyDown", "return PressNumeric();")

            CycleTimeMan = GVR.FindControl("CycleTimeManAdd")
            CycleTimeMan.Attributes.Add("OnKeyDown", "return PressNumeric();")

            ManPower = GVR.FindControl("ManPowerAdd")
            ManPower.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "bindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim tbMachineGrp, tbCapacityQty, tbCapacityHour, tbCycleTime As TextBox
        Dim tbSettingTime, tbMaxMachine, tbCycleTimeMan, tbManPower As TextBox
        Dim ddlFgDefault As DropDownList
        Dim GVR As GridViewRow = Nothing
        Try

            If e.CommandName = "Insert" Then
                tbMachineGrp = DataGrid.FooterRow.FindControl("MachineGroupAdd")
                tbCapacityQty = DataGrid.FooterRow.FindControl("CapacityQtyAdd")
                tbCapacityHour = DataGrid.FooterRow.FindControl("CapacityHourAdd")
                tbCycleTime = DataGrid.FooterRow.FindControl("CycleTimeAdd")
                tbSettingTime = DataGrid.FooterRow.FindControl("SettingTimeAdd")
                tbMaxMachine = DataGrid.FooterRow.FindControl("MaxMachineAdd")
                tbCycleTimeMan = DataGrid.FooterRow.FindControl("CycleTimeManAdd")
                tbManPower = DataGrid.FooterRow.FindControl("ManPowerAdd")
                ddlFgDefault = DataGrid.FooterRow.FindControl("FgDefaultAdd")

                If tbMachineGrp.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Machine Group must be filled.")
                    tbMachineGrp.Focus()
                    Exit Sub
                End If
                If CFloat(tbCapacityQty.Text) < 0 Then
                    lstatus.Text = MessageDlg("Capacity Qty must be filled.")
                    tbCapacityQty.Focus()
                    Exit Sub
                End If
                If CFloat(tbCapacityHour.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Capacity / Machine (Hours) must be filled.")
                    tbCapacityHour.Focus()
                    Exit Sub
                End If                
                'If CFloat(tbCycleTime.Text) <= 0 Then
                '    lstatus.Text = MessageDlg("Cycle Time must be filled.")
                '    tbCycleTime.Focus()
                '    Exit Sub
                'End If
                If CFloat(tbSettingTime.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Setting Time must be filled.")
                    tbSettingTime.Focus()
                    Exit Sub
                End If
                If CFloat(tbMaxMachine.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Max Machine Run must be filled.")
                    tbMaxMachine.Focus()
                    Exit Sub
                End If
                If CFloat(tbCycleTimeMan.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Cycle Time / Man must be filled.")
                    tbCycleTimeMan.Focus()
                    Exit Sub
                End If
                If CFloat(tbManPower.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Man Power must be filled.")
                    tbManPower.Focus()
                    Exit Sub
                End If

                If IsNumeric(tbCapacityQty.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Capacity Qty must be in numeric.")
                    tbCapacityQty.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbCapacityHour.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Capacity / Machine (Hours) must be in numeric.")
                    tbCapacityHour.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbCycleTime.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Cycle Time must be in numeric.")
                    tbCycleTime.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbSettingTime.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Setting Time must be in numeric.")
                    tbSettingTime.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbMaxMachine.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Max Machine Run must be in numeric.")
                    tbMaxMachine.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbCycleTimeMan.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Cycle Time / Man must be in numeric.")
                    tbCycleTimeMan.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbManPower.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Man Power must be in numeric.")
                    tbManPower.Focus()
                    Exit Sub
                End If
                
                'cek exists data
                Dim ExistRow As DataRow()
                ExistRow = ViewState("Dt").Select("ProductCode = " + QuotedStr(tbProductCode.Text) + " AND MachineGroup_Code = " + QuotedStr(tbMachineGrp.Text))
                If ExistRow.Count = 0 Then
                    SQLString = "INSERT INTO MsProductCapacity (ProductCode, MachineGroup, CapacityQty, CapacityHour, CycleTime, SettingTime, MaxMachine, CycleTimeMan, ManPower, FgDefault, UserId, UserDate) " + _
                    "SELECT " + QuotedStr(tbProductCode.Text) + ", " + QuotedStr(tbMachineGrp.Text) + ", " + tbCapacityQty.Text.Replace(",", "") + ", " + tbCapacityHour.Text.Replace(",", "") + ", " + _
                    tbCycleTime.Text.Replace(",", "") + ", " + tbSettingTime.Text.Replace(",", "") + ", " + tbMaxMachine.Text.Replace(",", "") + ", " + _
                    tbCycleTimeMan.Text.Replace(",", "") + ", " + tbManPower.Text.Replace(",", "") + ", " + QuotedStr(ddlFgDefault.SelectedValue) + ", " + _
                    QuotedStr(ViewState("UserId")) + ", GetDate()"
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    bindDataGrid(tbProductCode.Text)
                Else
                    lstatus.Text = MessageDlg("Data Already Exists")
                End If
            ElseIf e.CommandName = "btnMachineGroupAdd" Or e.CommandName = "btnMachineGroupEdit" Then
                Dim FieldResult As String

                If e.CommandName = "btnMachineGroupAdd" Then
                    Session("filter") = "Select * FROM VMsMachineGroup "
                    ViewState("Sender") = "btnMachineGroupAdd"
                Else
                    Session("filter") = "Select * FROM VMsMachineGroup "
                    ViewState("Sender") = "btnMachineGroupEdit"
                End If
                FieldResult = "MachineGrpCode, MachineGrpName"
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("MachineGroupCode")

            Dim dr() As DataRow
            dr = ViewState("Dt").Select("MachineGroup_Code = " + QuotedStr(txtID.Text) + " AND ProductCode = " + QuotedStr(tbProductCode.Text))
            dr(0).Delete()
            SQLExecuteNonQuery("DELETE FROM MsProductCapacity WHERE MachineGroup = " + QuotedStr(txtID.Text) + " AND ProductCode = " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString)

            bindDataGrid(tbProductCode.Text)
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim tbCapacityQty, tbCapacityHour, tbCycleTime As TextBox
        Dim tbSettingTime, tbMaxMachine, tbCycleTimeMan, tbManPower As TextBox
        Dim ddlFgDefault As DropDownList
        Dim lbl As Label
        Dim CapQty, CapMachine, CycleTime, SetTime, MaxMachRun, CycleTimeMan, ManPower As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid(tbProductCode.Text)
            obj = DataGrid.Rows(e.NewEditIndex)
            tbCapacityQty = obj.FindControl("CapacityQtyEdit")
            tbCapacityHour = obj.FindControl("CapacityHourEdit")
            tbCycleTime = obj.FindControl("CycleTimeEdit")
            tbSettingTime = obj.FindControl("SettingTimeEdit")
            tbMaxMachine = obj.FindControl("MaxMachineEdit")
            tbCycleTimeMan = obj.FindControl("CycleTimeManEdit")
            tbManPower = obj.FindControl("ManPowerEdit")
            ddlFgDefault = obj.FindControl("FgDefaultEdit")
            lbl = obj.FindControl("MachineGroupEdit")
            PrevStart = CFloat(lbl.Text)
            tbCapacityQty.Focus()

            CapQty = DataGrid.Rows(e.NewEditIndex).FindControl("CapacityQtyEdit")
            CapQty.Attributes.Add("OnKeyDown", "return PressNumeric();")

            CapMachine = DataGrid.Rows(e.NewEditIndex).FindControl("CapacityHourEdit")
            CapMachine.Attributes.Add("OnKeyDown", "return PressNumeric();")

            CycleTime = DataGrid.Rows(e.NewEditIndex).FindControl("CycleTimeEdit")
            CycleTime.Attributes.Add("OnKeyDown", "return PressNumeric();")

            SetTime = DataGrid.Rows(e.NewEditIndex).FindControl("SettingTimeEdit")
            SetTime.Attributes.Add("OnKeyDown", "return PressNumeric();")

            MaxMachRun = DataGrid.Rows(e.NewEditIndex).FindControl("MaxMachineEdit")
            MaxMachRun.Attributes.Add("OnKeyDown", "return PressNumeric();")

            CycleTimeMan = DataGrid.Rows(e.NewEditIndex).FindControl("CycleTimeManEdit")
            CycleTimeMan.Attributes.Add("OnKeyDown", "return PressNumeric();")

            ManPower = DataGrid.Rows(e.NewEditIndex).FindControl("ManPowerEdit")
            ManPower.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim tbCapacityQty, tbCapacityHour, tbCycleTime As TextBox
        Dim tbSettingTime, tbMaxMachine, tbCycleTimeMan, tbManPower As TextBox
        Dim ddlFgDefault As DropDownList
        Dim lbl As Label
        Try
            lbl = DataGrid.Rows(e.RowIndex).FindControl("MachineGroupEdit")
            tbCapacityQty = DataGrid.Rows(e.RowIndex).FindControl("CapacityQtyEdit")
            tbCapacityHour = DataGrid.Rows(e.RowIndex).FindControl("CapacityHourEdit")
            tbCycleTime = DataGrid.Rows(e.RowIndex).FindControl("CycleTimeEdit")
            tbSettingTime = DataGrid.Rows(e.RowIndex).FindControl("SettingTimeEdit")
            tbMaxMachine = DataGrid.Rows(e.RowIndex).FindControl("MaxMachineEdit")
            tbCycleTimeMan = DataGrid.Rows(e.RowIndex).FindControl("CycleTimeManEdit")
            tbManPower = DataGrid.Rows(e.RowIndex).FindControl("ManPowerEdit")
            ddlFgDefault = DataGrid.Rows(e.RowIndex).FindControl("FgDefaultEdit")

            If CFloat(tbCapacityQty.Text) <= 0 Then
                lstatus.Text = MessageDlg("Capacity Qty must be filled.")
                tbCapacityQty.Focus()
                Exit Sub
            End If
            If CFloat(tbCapacityHour.Text) <= 0 Then
                lstatus.Text = MessageDlg("Capacity Hour must be filled.")
                tbCapacityHour.Focus()
                Exit Sub
            End If
            'If CFloat(tbCycleTime.Text) <= 0 Then
            '    lstatus.Text = MessageDlg("Cycle Time must be filled.")
            '    tbCycleTime.Focus()
            '    Exit Sub
            'End If
            If CFloat(tbSettingTime.Text) <= 0 Then
                lstatus.Text = MessageDlg("Setting Time must be filled.")
                tbSettingTime.Focus()
                Exit Sub
            End If
            If CFloat(tbMaxMachine.Text) <= 0 Then
                lstatus.Text = MessageDlg("Max Machine must be filled.")
                tbMaxMachine.Focus()
                Exit Sub
            End If
            If CFloat(tbCycleTimeMan.Text) <= 0 Then
                lstatus.Text = MessageDlg("Cycle Time Man must be filled.")
                tbCycleTimeMan.Focus()
                Exit Sub
            End If
            If CFloat(tbManPower.Text) <= 0 Then
                lstatus.Text = MessageDlg("Man Power must be filled.")
                tbManPower.Focus()
                Exit Sub
            End If

            If IsNumeric(tbCapacityQty.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Capacity Qty must be in numeric.")
                tbCapacityQty.Focus()
                Exit Sub
            End If
            If IsNumeric(tbCapacityHour.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Capacity Hour must be in numeric.")
                tbCapacityHour.Focus()
                Exit Sub
            End If
            If IsNumeric(tbCycleTime.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Cycle Time must be in numeric.")
                tbCycleTime.Focus()
                Exit Sub
            End If
            If IsNumeric(tbSettingTime.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Setting Time must be in numeric.")
                tbSettingTime.Focus()
                Exit Sub
            End If
            If IsNumeric(tbMaxMachine.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Max Machine must be in numeric.")
                tbMaxMachine.Focus()
                Exit Sub
            End If
            If IsNumeric(tbCycleTimeMan.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Cycle Time Man must be in numeric.")
                tbCycleTimeMan.Focus()
                Exit Sub
            End If
            If IsNumeric(tbManPower.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Man Power must be in numeric.")
                tbManPower.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsProductCapacity SET CapacityQty = " + tbCapacityQty.Text.Replace(",", "") + _
            ", CapacityHour = " + tbCapacityHour.Text.Replace(",", "") + _
            ", CycleTime = " + tbCycleTime.Text.Replace(",", "") + _
            ", SettingTime = " + tbSettingTime.Text.Replace(",", "") + _
            ", MaxMachine = " + tbMaxMachine.Text.Replace(",", "") + _
            ", CycleTimeMan = " + tbCycleTimeMan.Text.Replace(",", "") + _
            ", ManPower = " + tbManPower.Text.Replace(",", "") + _
            " WHERE ProductCode = " + QuotedStr(tbProductCode.Text) + " AND MachineGroup =  " + QuotedStr(lbl.Text)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid(tbProductCode.Text)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_RowUpdating Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid(tbProductCode.Text)
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
                bindDataGrid(tbProductCode.Text)
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
            bindDataGrid(tbProductCode.Text)
        Catch ex As Exception
            lstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex

        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid(tbProductCode.Text)
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM MsProduct WHERE FgProduce = 'Y' AND FgActive = 'Y' ORDER BY ProductCode"
            ResultField = "ProductCode, ProductName"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnProduct Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub VisibleGrid()
        Try
            If tbProductName.Text.Trim <> "" Then
                Panel2.Visible = True
            Else
                Panel2.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("VisibleGrid Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub MachineGroupAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim MachineGroup, tb As TextBox
        Dim MachineGroupName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try

            tb = sender
            If tb.ID = "MachineGroupAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                MachineGroup = dgi.FindControl("MachineGroupAdd")
                MachineGroupName = dgi.FindControl("MachineGroupNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                MachineGroup = dgi.FindControl("MachineGroupEdit")
                MachineGroupName = dgi.FindControl("MachineGroupNameEdit")
            End If

            ds = SQLExecuteQuery("Select MachineGrpCode, MachineGrpName From VMsMachineGroup WHERE MachineGrpCode = " + QuotedStr(MachineGroup.Text), ViewState("DBConnection").ToString)

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                MachineGroup.Text = ""
                MachineGroupName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                MachineGroup.Text = dr("MachineGrpCode").ToString
                MachineGroupName.Text = dr("MachineGrpName").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tb Machine Group Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCopyTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyTo.Click
        Try
            pnlAssign.Visible = False
            pnlCopy.Visible = True
            btnCopyTo.Enabled = False
            If tbProductCode.Text.Trim <> "" Then
                tbFromCode.Text = tbProductCode.Text
                tbFromName.Text = tbProductName.Text
            Else
                tbFromCode.Text = ""
                tbFromName.Text = ""
            End If
            tbToCode.Text = ""
            tbToName.Text = ""
        Catch ex As Exception
            lstatus.Text = "btn copy to Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlCopy.Visible = False
            pnlAssign.Visible = True
            btnCopyTo.Enabled = True
        Catch ex As Exception
            lstatus.Text = "btn cancel ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFromCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFromCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Product", tbFromCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFromCode.Text = Dr("Product_Code")
                tbFromName.Text = Dr("Product_Name")
                tbFromCode.Focus()
            Else
                tbFromCode.Text = ""
                tbFromName.Text = ""
                tbFromCode.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Product", tbProductCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbProductCode.Focus()
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbProductCode.Focus()
            End If
            VisibleGrid()
            bindDataGrid(tbProductCode.Text)
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearchFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchFrom.Click
        Try
            Dim ResultField As String
            Session("filter") = "SELECT * FROM MsProduct WHERE FgProduce = 'Y' AND FgActive = 'Y' ORDER BY ProductCode"
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "ProductCode, ProductName"
            ViewState("Sender") = "btnSearchFrom"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btnSearchFrom Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearchTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchTo.Click
        Try
            Dim ResultField As String
            Session("filter") = "SELECT * FROM MsProduct WHERE FgProduce = 'Y' AND FgActive = 'Y' ORDER BY ProductCode"
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "ProductCode, ProductName"
            ViewState("Sender") = "btnSearchTo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btnSearchTo Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbToCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbToCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Product", tbToCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbToCode.Text = Dr("Product_Code")
                tbToName.Text = Dr("Product_Name")
                tbToCode.Focus()
            Else
                tbToCode.Text = ""
                tbToName.Text = ""
                tbToCode.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tbToCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            If tbFromCode.Text = tbToCode.Text Then
                lstatus.Text = "<script language='javascript'>alert('Cannot copy to the same source');</script>"
                Exit Sub
            End If
            If tbFromName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Product From must be filled');</script>"
                tbFromCode.Focus()
                Exit Sub
            End If
            If tbToName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Product To must be filled');</script>"
                tbToCode.Focus()
                Exit Sub
            End If
            SQLExecuteNonQuery("EXEC S_MsProductCapacityCopyFrom " + QuotedStr(tbFromCode.Text) + "," + QuotedStr(tbToCode.Text) + ",0," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)

            pnlCopy.Visible = False
            btnCopyTo.Enabled = True
            pnlAssign.Visible = True
            tbProductCode.Text = tbToCode.Text
            tbProductName.Text = tbToName.Text
            bindDataGrid(tbProductCode.Text)
        Catch ex As Exception
            Throw New Exception("btnCopy_Click change Error : " + ex.ToString)
        End Try
    End Sub
End Class
