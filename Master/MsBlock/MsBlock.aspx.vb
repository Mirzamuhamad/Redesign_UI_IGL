Imports System.Data
Partial Class MsBlock_MsBlock
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlLandScape, "SELECT LandScapeCode, LandScapeName FROM MsLandScape", True, "LandScapeCode", "LandScapeName", ViewState("DBConnection"))
            FillCombo(ddlLandType, "SELECT LandTypeCode, LandTypeName FROM MsLandType", True, "LandTypeCode", "LandTypeName", ViewState("DBConnection"))
            FillCombo(ddlAreal, "SELECT ArealCode, ArealName FROM V_MsArealView", True, "ArealCode", "ArealName", ViewState("DBConnection"))
            FillCombo(ddlKSUBlock, "SELECT KSUBlockCode, KSUBlockName FROM V_MsKSUBlock", True, "KSUBlockCode", "KSUBlockName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            tbMaxCap.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbArea.Attributes.Add("OnKeyDown", "return PressNumeric();")

        End If
        If Not Session("Result") Is Nothing Then


            Session("Result") = Nothing
            ViewState("Sender") = Nothing

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
    Private Sub ClearInput()
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If

            tbName.Text = ""
            tbBlockNo.Text = "0"
            ddlAreal.SelectedIndex = 0
            tbArea.Text = "0"
            tbEstStartPlant.SelectedDate = ViewState("ServerDate") 'Today
            tbStartPlant.SelectedDate = ViewState("ServerDate") 'Today
            ddlLandType.SelectedIndex = 0
            ddlLandScape.SelectedIndex = 0
            tbMaxCap.Text = "0"
            tbStatusTanam.Text = ""
            tbQtyTanam.Text = ""
            tbSPH.Text = ""
            ddlFgPanen.SelectedIndex = 1
            ddlKSUBlock.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal BlockCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try

            SqlString = "SELECT * FROM V_MsBlockView  WHERE BlockCode = " + QuotedStr(BlockCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("BlockCode").ToString)
            BindToText(tbName, DT.Rows(0)("BlockName").ToString)
            BindToDate(tbEstStartPlant, DT.Rows(0)("EstStartPlant").ToString)
            BindToDate(tbStartPlant, DT.Rows(0)("StartPlant").ToString)
            BindToText(tbBlockNo, DT.Rows(0)("BlockNo").ToString)
            BindToDropList(ddlAreal, DT.Rows(0)("Areal").ToString)
            BindToText(tbArea, DT.Rows(0)("Area").ToString)
            BindToDropList(ddlLandType, DT.Rows(0)("LandType").ToString)
            BindToDropList(ddlLandScape, DT.Rows(0)("LandScape").ToString)
            BindToText(tbMaxCap, DT.Rows(0)("MaxCap").ToString)
            BindToText(tbStatusTanam, DT.Rows(0)("StatusTanam").ToString)
            BindToText(tbQtyTanam, CFloat(DT.Rows(0)("QtyTanam").ToString))
            BindToText(tbSPH, FormatNumber(DT.Rows(0)("SPH").ToString))
            'FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            BindToDropList(ddlFgPanen, DT.Rows(0)("FgPanen").ToString)
            BindToDropList(ddlKSUBlock, DT.Rows(0)("KSUBlock").ToString)

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
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
            SqlString = "SELECT * From V_MsBlockView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "BlockCode ASC"
                ViewState("SortOrder") = "ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim dt As DataTable
            dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)

            If dt.Rows.Count = 0 Then
                lstatus.Text = "No Data"
                DataGrid.Visible = False
                btnAdd2.Visible = False
            Else
                DataGrid.Visible = True
                btnAdd2.Visible = True
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            ddlField2.SelectedValue.Replace("BlockCode", "Block_Code")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMasterBlockFile 'V_MsBlockView','BlockCode','BlockName','ArealName','Area','QtyTanam','MaxCap','SPH','StatusTanam','EstStartPlant','StartPlant','BlockNo','LandTypeName','LandScapeName','KSUBlock', 'Block File', 'Code','Name','Areal','Luas','Qty Tanam','Max Capacity','SPH','Status Tanam','Est Start Tanam','Start Plant','Block No','Land Type','Lans Cape','KSU'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMasterBlockFile.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
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
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(pnlHd, pnlInput)
                    FillTextBox(GVR.Cells(1).Text)
                    ViewState("State") = "View"
                    ModifyInput(False, pnlInput)
                    tbCode.Enabled = False
                    btnHome.Visible = True
                    BtnSave.Visible = False
                    btnReset.Visible = False
                    btnCancel.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    MovePanel(pnlHd, pnlInput)
                    FillTextBox(GVR.Cells(1).Text)
                    ViewState("State") = "Edit"
                    ModifyInput(True, pnlInput)
                    tbCode.Enabled = False
                    btnHome.Visible = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                    btnCancel.Visible = True
                    tbName.Focus()
                ElseIf DDL.SelectedValue = "Non Active" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        If GVR.Cells(8).Text = "N" Then
                            lstatus.Text = "<script language='javascript'> {alert('Job Plantation closed already')}</script>"
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("UPDATE MsBlock SET Fgactive = 'N' WHERE BlockCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("DELETE MsBlock WHERE BlockCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCommand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbCode.Enabled = True
            'FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            ClearInput()
            ModifyInput(True, pnlInput)
            BtnSave.Visible = True
            btnReset.Visible = True
            btnCancel.Visible = True
            'ddlActive.Enabled = False
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Block Code must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Block Name must be filled.")
                tbName.Focus()
                Return False
            End If
            If tbEstStartPlant.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg(" Est Start Tanam must be filled.")
                tbEstStartPlant.Focus()
                Return False
            End If
            If tbBlockNo.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Block No must be filled.")
                tbBlockNo.Focus()
                Return False
            End If
            If ddlAreal.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg(" Areal must be filled.")
                ddlAreal.Focus()
                Return False
            End If
            If ddlLandType.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Land Type must be filled.")
                ddlLandType.Focus()
                Return False
            End If
            If ddlLandScape.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Land Scape must be filled.")
                ddlLandScape.Focus()
                Return False
            End If
            If tbMaxCap.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Max Capacity must be filled.")
                tbMaxCap.Focus()
                Return False
            End If
            If ddlKSUBlock.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Acc Asset must be filled.")
                ddlKSUBlock.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT BlockCode FROM V_MsBlockView WHERE BlockCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Block Code " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsBlock (BlockCode, BlockName, EstStartPlant, StartPlant, BlockNo,Areal, Area, LandType, LandScape, MaxCap, FgPanen, KSUBlock, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(tbEstStartPlant.Text) + ", " + QuotedStr(tbStartPlant.Text) + ", " & _
                QuotedStr(tbBlockNo.Text) + ", " & _
                QuotedStr(ddlAreal.Text) + ", " + _
                QuotedStr(tbArea.Text) + ", " + _
                QuotedStr(ddlLandType.Text) + ", " + _
                QuotedStr(ddlLandScape.Text) + ", " + _
                QuotedStr(tbMaxCap.Text) + ", " + _
                QuotedStr(ddlFgPanen.SelectedValue) + ", " + _
                QuotedStr(ddlKSUBlock.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                'QuotedStr(tbStatusTanam.Text) + ", " + _
                'QuotedStr(tbQtyTanam.Text) + ", " + _
                'QuotedStr(tbSPH.Text) + ", " + _
               
            Else
                SqlString = "UPDATE MsBlock SET BlockName= " + QuotedStr(tbName.Text) & _
                            ", EstStartPlant = " + QuotedStr(tbEstStartPlant.Text) & _
                            ", StartPlant = " + QuotedStr(tbStartPlant.Text) & _
                            ", BlockNo = " + QuotedStr(tbBlockNo.Text) & _
                            ", Areal = " + QuotedStr(ddlAreal.SelectedValue) & _
                            ", Area = " + QuotedStr(tbArea.Text) & _
                            ", LandType = " + QuotedStr(ddlLandType.SelectedValue) & _
                            ", LandScape = " + QuotedStr(ddlLandScape.SelectedValue) & _
                            ", MaxCap = " + QuotedStr(tbMaxCap.Text) & _
                            ", FgPanen = " + QuotedStr(ddlFgPanen.SelectedValue) & _
                            ", KSUBlock = " + QuotedStr(ddlKSUBlock.SelectedValue) & _
                            " WHERE BlockCode = " + QuotedStr(tbCode.Text)
                '", StatusTM = " + QuotedStr(tbStatusTanam.Text) & _
                ' ", QtyTanam = " + QuotedStr(tbQtyTanam.Text) & _
                '", SPH = " + QuotedStr(tbSPH.Text) & _
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Home Error : " + ex.ToString
        End Try
    End Sub
End Class
