Imports System.Data
Partial Class MsTeam_MsTeam
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlDivision, "select Code, Name From V_MsDivEstate where type = 'Division'", False, "Code", "Name", ViewState("DBConnection"))
            'FillCombo(ddlDivision, "SELECT DivisionCode, DivisionName FROM MsDivision", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            tbTotalEmp.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalNonEmp.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalMember.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPremiHadir.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPremiNatura.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPremiOther.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMaxTotalBottom.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMaxTotalTop.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMaxHKBottom.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMaxHKTop.Attributes.Add("OnKeyDown", "return PressNumeric();")

        End If
        If Not Session("Result") Is Nothing Then

            If ViewState("Sender") = "btnSupplier" Then
                tbSupplier.Text = Session("Result")(0).ToString
                tbSupplierName.Text = Session("Result")(1).ToString
            End If
            ' If ViewState("Sender") = "btnAccAsset" Then
            'tbAccAsset.Text = Session("Result")(0).ToString
            ' tbAccAssetName.Text = Session("Result")(1).ToString
            ' End If
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
            ddlTeamType.SelectedIndex = 0
            tbSupplier.Text = ""
            tbSupplierName.Text = ""
            ddlDivType.SelectedIndex = 0
            ddlDivision.SelectedIndex = 0
            tbTotalEmp.Text = "0"
            tbTotalNonEmp.Text = "0"
            tbTotalMember.Text = "0"
            tbPremiHadir.Text = ""
            tbPremiNatura.Text = ""
            tbPremiOther.Text = 0
            ddlActive.SelectedIndex = 0
            tbKetuaTeam.Text = ""
            tbTotalEmp.Text = 0
            tbMaxTotalBottom.Text = 0
            tbMaxTotalTop.Text = 0
            tbMaxHKBottom.Text = 0
            tbMaxHKTop.Text = 0


        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal TeamCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsTeamView WHERE TeamCode = " + QuotedStr(TeamCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("TeamCode").ToString)
            BindToText(tbName, DT.Rows(0)("TeamName").ToString)
            'ddlTeamType.Text = ""
            'ddlJobGroupPlant.SelectedIndex = 0
            'ddlDivision.SelectedIndex = 0
            'ddlUnit.SelectedIndex = 0
            BindToDropList(ddlTeamType, DT.Rows(0)("TeamType").ToString)
            BindToText(tbSupplier, DT.Rows(0)("Supplier").ToString)
            BindToText(tbSupplierName, DT.Rows(0)("SupplierName").ToString)
            BindToDropList(ddlDivType, DT.Rows(0)("DivType").ToString)
            'FillCombo(ddlDivision, "select Code, Name From V_MsDivEstate Where Type = " + QuotedStr(ddlDivType.SelectedIndex), False, "Code", "Name", ViewState("DBConnection"))
            BindToDropList(ddlDivision, DT.Rows(0)("Division").ToString)
            'FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            'BindToDropList(ddlUnitConvert, DT.Rows(0)("UnitConvert").ToString)
            BindToText(tbTotalEmp, DT.Rows(0)("TotalEmp").ToString)
            BindToText(tbTotalNonEmp, DT.Rows(0)("TotalNonEmp").ToString)
            BindToText(tbTotalMember, DT.Rows(0)("TotalMember").ToString)
            BindToText(tbPremiHadir, CFloat(DT.Rows(0)("PremiHadir").ToString))
            BindToText(tbPremiNatura, CFloat(DT.Rows(0)("PremiNatura").ToString))
            BindToText(tbPremiOther, CFloat(DT.Rows(0)("PremiOther").ToString))
            BindToText(tbKetuaTeam, DT.Rows(0)("KetuaTeam").ToString)
            BindToDropList(ddlActive, DT.Rows(0)("FgActive").ToString)
            If DT.Rows(0)("FgLand").ToString = "Y" Then
                FgLand.Checked = True
            Else
                FgLand.Checked = False
            End If
            If DT.Rows(0)("FgBatch").ToString = "Y" Then
                FgBatch.Checked = True
            Else
                FgBatch.Checked = False
            End If
            If DT.Rows(0)("FgBlock").ToString = "Y" Then
                FgBlock.Checked = True
            Else
                FgBlock.Checked = False
            End If
            If DT.Rows(0)("FgPanen").ToString = "Y" Then
                FgPanen.Checked = True
            Else
                FgPanen.Checked = False
            End If
            BindToText(tbMaxTotalBottom, FormatNumber(DT.Rows(0)("MaxTotalBottom").ToString))
            BindToText(tbMaxTotalTop, FormatNumber(DT.Rows(0)("MaxTotalTop").ToString))
            BindToText(tbMaxHKTop, CFloat(DT.Rows(0)("MaxHKTop").ToString))
            BindToText(tbMaxHKBottom, CFloat(DT.Rows(0)("MaxHKBottom").ToString))
            BindToDropList(ddlBpjs, CFloat(DT.Rows(0)("FgBPJS").ToString))
            BindToDropList(ddlInap, CFloat(DT.Rows(0)("FgInap").ToString))

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
            SqlString = "SELECT * From V_MsTeamView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TeamCode ASC"
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
            ddlField2.SelectedValue.Replace("TeamCode", "Team_Code")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster6 'V_MsTeamView','TeamCode','TeamName','TeamType','DivType','DivisionName','KetuaTeam','Team File','Code','Description','Team Type','Division Type','Division','Ketua Team'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
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
                        SQLExecuteNonQuery("UPDATE MsTeam SET Fgactive = 'N' WHERE TeamCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("DELETE MsTeam WHERE TeamCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
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
            ClearInput()
            ModifyInput(True, pnlInput)
            BtnSave.Visible = True
            btnReset.Visible = True '
            btnCancel.Visible = True
            ddlActive.Enabled = True
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Team Code must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Team Name must be filled.")
                tbName.Focus()
                Return False
            End If
            If ddlTeamType.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Team Type must be filled.")
                ddlTeamType.Focus()
                Return False
            End If
            If tbSupplier.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg(" Supplier must be filled.")
                tbSupplier.Focus()
                Return False
            End If
            If ddlDivType.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Div Type must be filled.")
                ddlDivType.Focus()
                Return False
            End If
            If ddlDivision.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Division must be filled.")
                ddlDivision.Focus()
                Return False
            End If
            If tbKetuaTeam.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Ketua Team must be filled.")
                tbKetuaTeam.Focus()

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString, VFgLand, VFgBatch, VFgBlock, VFgPanen As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If
            If FgLand.Checked = True Then
                VFgLand = "Y"
            Else
                VFgLand = "N"
            End If
            If FgBatch.Checked = True Then
                VFgBatch = "Y"
            Else
                VFgBatch = "N"
            End If
            If FgBlock.Checked = True Then
                VFgBlock = "Y"
            Else
                VFgBlock = "N"
            End If
            If FgPanen.Checked = True Then
                VFgPanen = "Y"
            Else
                VFgPanen = "N"
            End If
            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT TeamCode FROM V_MsTeamView WHERE TeamCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Team Code " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsTeam (TeamCode, TeamName, TeamType, Supplier, DivType, Division, TotalEmp, TotalNonEmp, TotalMember, PremiHadir, PremiNatura, PremiOther, KetuaTeam,FgLand,FgBatch,FgBlock,FgPanen,FgActive,MaxTotalBottom,MaxTotalTop,MaxHKBottom,MaxHKTop,FgBPJS, FgInap, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(ddlTeamType.SelectedValue) + ", " + QuotedStr(tbSupplier.Text) + ", " & _
                QuotedStr(ddlDivType.SelectedValue) + ", " & _
                QuotedStr(ddlDivision.SelectedValue) + ", " + _
                tbTotalEmp.Text + ", " + _
                tbTotalNonEmp.Text + ", " + _
                tbTotalMember.Text + ", " + _
                tbPremiHadir.Text + ", " + _
                tbPremiNatura.Text + ", " + _
                tbPremiOther.Text + ", " + _
                QuotedStr(tbKetuaTeam.Text) + ", " + _
                QuotedStr(VFgLand) + ", " + _
                QuotedStr(VFgBatch) + ", " + _
                QuotedStr(VFgBlock) + ", " + _
                QuotedStr(VFgPanen) + ", " + _
                QuotedStr(ddlActive.SelectedValue) + ", " + _
                tbMaxTotalBottom.Text.Replace(",", "") + ", " + _
                tbMaxTotalTop.Text.Replace(",", "") + ", " + _
                tbMaxHKBottom.Text + ", " + _
                tbMaxHKTop.Text + ", " + _
                QuotedStr(ddlBpjs.SelectedValue) + ", " + _
                QuotedStr(ddlInap.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "UPDATE MsTeam SET TeamName= " + QuotedStr(tbName.Text) & _
                            ", TeamType = " + QuotedStr(ddlTeamType.SelectedValue) & _
                            ", Supplier = " + QuotedStr(tbSupplier.Text) & _
                            ", DivType = " + QuotedStr(ddlDivType.SelectedValue) & _
                            ", Division = " + QuotedStr(ddlDivision.SelectedValue) & _
                            ", TotalEmp = " + tbTotalEmp.Text & _
                            ", TotalNonEmp = " + tbTotalNonEmp.Text & _
                            ", TotalMember = " + tbTotalMember.Text & _
                            ", KetuaTeam = " + QuotedStr(tbKetuaTeam.Text) & _
                            ", PremiHadir = " + tbPremiHadir.Text & _
                            ", PremiNatura = " + tbPremiNatura.Text & _
                            ", PremiOther = " + tbPremiOther.Text & _
                            ", FgLand = " + QuotedStr(VFgLand) & _
                            ", FgPanen = " + QuotedStr(VFgBatch) & _
                            ", FgBlock = " + QuotedStr(VFgBlock) & _
                            ", FgBatch = " + QuotedStr(VFgPanen) & _
                            ", FgActive = " + QuotedStr(ddlActive.SelectedValue) & _
                            ", MaxTotalBottom = " + tbMaxTotalBottom.Text.Replace(",", "") & _
                            ", MaxTotalTop = " + tbMaxTotalTop.Text.Replace(",", "") & _
                            ", MaxHKBottom = " + tbMaxHKBottom.Text & _
                            ", MaxHKTop = " + tbMaxHKTop.Text & _
                            ", FgInap = " + QuotedStr(ddlInap.SelectedValue) & _
                            ", FgBPJS = " + QuotedStr(ddlBpjs.SelectedValue) & _
                            " WHERE TeamCode = " + QuotedStr(tbCode.Text)
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

    Protected Sub btnSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupplier.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select Supplier_Code, Supplier_Name from V_MsSupplier WHERE FgActive = 'Y' "
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Supplier Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSupplier_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSupplier.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Supplier_Code, Supplier_Name from V_MsSupplier WHERE FgActive = 'Y' AND Supplier_Code = " + QuotedStr(tbSupplier.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbSupplier.Text = ""
                tbSupplierName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbSupplier.Text = dr("Supplier_Code").ToString
                tbSupplierName.Text = dr("Supplier_Name").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbSupplier_TextChanged Error : " + ex.ToString
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

    Protected Sub ddlTeamType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTeamType.SelectedIndexChanged

    End Sub

    Protected Sub ddlDivType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivType.SelectedIndexChanged
        Try
            FillCombo(ddlDivision, "SELECT Type, Code, Name FROM V_MsDivEstate where type = " + QuotedStr(ddlDivType.SelectedValue), True, "Code", "Name", ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = "Btn DivType Error : " + ex.ToString
        End Try
    End Sub
End Class
