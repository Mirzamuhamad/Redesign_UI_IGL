Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsPanen_MsPanen
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            ' FillCombo(ddlAbsStatus, "EXEC S_GetAbsenceDt 'Cuti'", True, "AbsenceCode", "AbsenceName", ViewState("DBConnection"))
            FillCombo(ddlStatusTanam, "SELECT PlantPeriodCode, PlantPeriodName FROM MsPlantPeriod", True, "PlantPeriodCode", "PlantPeriodName", ViewState("DBConnection"))
            FillCombo(ddlStatusTanam2, "SELECT PlantPeriodCode, PlantPeriodName FROM MsPlantPeriod", True, "PlantPeriodCode", "PlantPeriodName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            tbStartBJR.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbEndBJR.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbHKPerBasis.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbEkuivalenABnormal.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbJanjangPerBasis.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKrgPerBasis.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbJanjangPerBasisHt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKrgPerBasisHt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceNBPerJanjang.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceNBPerKrg.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceNBPerJanjangHt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceNBPerKrgHt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceUPPerJanjang.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceUPPerKrg.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceUPPerJanjangHt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceUPPerKrgHt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPremiBasis.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPremiBasisKrg.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPremiBasisHt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPremiBasisKrgHt.Attributes.Add("OnKeyDown", "return PressNumeric();")


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
            ddlStatusTanam.SelectedValue = ""
            ddlStatusTanam2.SelectedValue = ""
            tbStartBJR.Text = "0"
            tbEndBJR.Text = "0"
            tbHKPerBasis.Text = "0"
            tbEkuivalenABnormal.Text = "0"
            tbJanjangPerBasis.Text = "0"
            tbKrgPerBasis.Text = "0"
            tbKrgPerBasisHt.Text = "0"
            tbJanjangPerBasisHt.Text = "0"
            tbKrgPerBasisHt.Text = "0"
            tbPriceNBPerJanjang.Text = "0"
            tbPriceNBPerKrg.Text = "0"
            tbPriceNBPerJanjangHt.Text = "0"
            tbPriceNBPerKrgHt.Text = "0"
            tbPriceUPPerJanjang.Text = "0"
            tbPriceUPPerKrg.Text = "0"
            tbPriceUPPerJanjangHt.Text = "0"
            tbPriceUPPerKrgHt.Text = "0"
            tbPremiBasis.Text = "0"
            tbPremiBasisKrg.Text = "0"
            tbPremiBasisHt.Text = "0"
            tbPremiBasisKrgHt.Text = "0"

            'tbMaxRecuring.Enabled = (ddlFgRecuring.SelectedValue = "Y")
            'tbLeadTime.Enabled = (ddlFgRecuring.SelectedValue = "Y")
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
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
            SqlString = "SELECT PanenCode, PanenName, StatusTanam, StatusTanam2, StartBJR, EndBJR, HKPerBasis, EkuivalenABnormal, JanjangperBasis,PriceNBPerJanjang,PriceUPPerJanjang,PremiBasis,JanjangperBasisHt,KrgPerBasis,KrgPerBasisHt,PriceNBPerJanjangHt,PriceNBPerKrg,PriceNBPerKrgHt,PriceUPPerJanjangHt,PriceUPPerKrg,PriceUPPerKrgHt,PremiBasisHt,PremiBasisKrg,PremiBasisKrgHt,UserId,UserDate FROM MsPanen" + StrFilter
            '" INNER JOIN VMsAbsStatus B ON A.AbsStatus = B.AbsStatusCode " + _
            '" LEFT OUTER JOIN MsLeaveType C ON A.LeaveType = C.LeaveTypeCode " + StrFilter + " ORDER BY A.LeaveCode"
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "PanenCode ASC"
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
            ddlField2.SelectedValue.Replace("PanenCode", "Panen_Code")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster6 'V_MsPanenView','PanenCode','PanenName','StatusTanam','StatusTanam2','HkPerBasis','EkuivalenAbnormal','Panen File','Code','Name','Status Tanam','Status Tanam','HK PerBasis','AbNormal'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
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
        'Dim SQLString As String
        'Dim dbCode, dbName As TextBox
        'Dim cbxWrhsGroup As DropDownList
        'Dim cbxWrhsArea As DropDownList
        'Dim cbxWrhsType As DropDownList
        'Dim cbxWrhsCondition As DropDownList
        'Dim cbxFgActive As DropDownList

        Try
            If e.CommandName = "Insert" Then
                'dbCode = DataGrid.FooterRow.FindControl("WrhsCodeAdd")
                'dbName = DataGrid.FooterRow.FindControl("WrhsNameAdd")
                'cbxWrhsGroup = DataGrid.FooterRow.FindControl("WrhsGroupAdd")
                'cbxWrhsArea = DataGrid.FooterRow.FindControl("WrhsAreaAdd")
                'cbxWrhsType = DataGrid.FooterRow.FindControl("WrhsTypeAdd")
                'cbxWrhsCondition = DataGrid.FooterRow.FindControl("WrhsConditionAdd")
                'cbxFgActive = DataGrid.FooterRow.FindControl("FgActiveAdd")

                'If dbCode.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Code must be filled."
                '    dbCode.Focus()
                '    Exit Sub
                'End If
                'If tbName.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Name must be filled."
                '    tbName.Focus()
                '    Exit Sub
                'End If

                'If SQLExecuteScalar("SELECT Wrhs_Code From VMsLeaves WHERE Wrhs_Code = " + QuotedStr(dbCode.Text), Session("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "Warehouse " + QuotedStr(dbCode.Text) + " has already been exist"
                '    Exit Sub
                'End If

                ''insert the new entry
                'SQLString = "Insert into MsLeaves (WrhsCode, WrhsName, WrhsGroup, WrhsArea, WrhsType, WrhsCondition, FgActive, UserId, UserDate ) " + _
                '"SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " & _
                'QuotedStr(cbxWrhsGroup.SelectedValue) + ", " + QuotedStr(cbxWrhsArea.SelectedValue) + ", " & _
                'QuotedStr(cbxWrhsType.SelectedItem.ToString) + ", " + QuotedStr(cbxWrhsCondition.SelectedValue) + ", " & _
                'QuotedStr(cbxFgActive.SelectedValue) + ", " & _
                'QuotedStr(Session("userId").ToString) + ", getDate()"
                'SQLExecuteNonQuery(SQLString)
                'bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        'Dim txtID As Label
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'txtID = DataGrid.Rows(e.RowIndex).FindControl("ItemNo")

            SQLExecuteNonQuery("DELETE FROM MsPanen WHERE PanenCode = '" & GVR.Cells(0).Text & "' ", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            obj = DataGrid.Rows(e.NewEditIndex)

            pnlHd.Visible = False
            pnlInput.Visible = True
            tbCode.Enabled = False


            BindToText(tbCode, obj.Cells(0).Text)
            BindToText(tbName, obj.Cells(1).Text)
            BindToDropList(ddlStatusTanam, obj.Cells(2).Text)
            BindToDropList(ddlStatusTanam2, obj.Cells(3).Text)
            BindToText(tbStartBJR, obj.Cells(4).Text)
            BindToText(tbEndBJR, obj.Cells(5).Text)
            BindToText(tbHKPerBasis, obj.Cells(6).Text)
            BindToText(tbEkuivalenABnormal, obj.Cells(7).Text)

            BindToText(tbJanjangPerBasis, obj.Cells(8).Text)
            BindToText(tbKrgPerBasis, obj.Cells(9).Text)
            BindToText(tbJanjangPerBasisHt, obj.Cells(10).Text)
            BindToText(tbKrgPerBasisHt, obj.Cells(11).Text)

            BindToText(tbPriceNBPerJanjang, obj.Cells(12).Text)
            BindToText(tbPriceNBPerKrg, obj.Cells(13).Text)
            BindToText(tbPriceNBPerJanjangHt, obj.Cells(14).Text)
            BindToText(tbPriceNBPerKrgHt, obj.Cells(15).Text)

            BindToText(tbPriceUPPerJanjang, obj.Cells(16).Text)
            BindToText(tbPriceUPPerKrg, obj.Cells(17).Text)
            BindToText(tbPriceUPPerJanjangHt, obj.Cells(18).Text)
            BindToText(tbPriceUPPerKrgHt, obj.Cells(19).Text)

            BindToText(tbPremiBasis, obj.Cells(20).Text)
            BindToText(tbPremiBasisKrg, obj.Cells(21).Text)
            BindToText(tbPremiBasisHt, obj.Cells(22).Text)
            BindToText(tbPremiBasisKrgHt, obj.Cells(23).Text)

            'BindToText(tbEndBJR, obj.Cells(6).Text)
            'BindToText(tbFactorRate, obj.Cells(7).Text)
            'BindToDropList(ddlFgHoliday, obj.Cells(8).Text)
            'BindToDropList(ddlAbsStatus, obj.Cells(9).Text)
            'BindToDropList(ddlLeaveType, obj.Cells(10).Text)
            ViewState("State") = "Edit"

            ' tbMaxRecuring.Enabled = (ddlFgRecuring.SelectedValue = "Y")
            'tbLeadTime.Enabled = (ddlFgRecuring.SelectedValue = "Y")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        'Dim SQLString As String
        'Dim dbName As TextBox
        'Dim lbCode As Label
        'Dim CbxWrhsGroup As DropDownList
        'Dim CbxWrhsArea As DropDownList
        'Dim CbxWrhsType As DropDownList
        'Dim CbxFgActive As DropDownList

        'Try
        '    lbCode = DataGrid.Rows(e.RowIndex).FindControl("WrhsCodeEdit")
        '    dbName = DataGrid.Rows(e.RowIndex).FindControl("WrhsNameEdit")
        '    CbxWrhsGroup = DataGrid.Rows(e.RowIndex).FindControl("WrhsGroupEdit")
        '    CbxWrhsArea = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaEdit")
        '    CbxWrhsType = DataGrid.Rows(e.RowIndex).FindControl("WrhsTypeEdit")
        '    CbxFgActive = DataGrid.Rows(e.RowIndex).FindControl("FgActiveEdit")

        '    If dbName.Text.Trim.Length = 0 Then
        '        lstatus.Text = " Wrhs Name must be filled."
        '        dbName.Focus()
        '        Exit Sub
        '    End If

        '    SQLString = "Update MsLeaves set WrhsName= " + QuotedStr(dbName.Text) + "," & _
        '    "WrhsGroup = " + QuotedStr(CbxWrhsGroup.SelectedValue) & _
        '    "WrhsArea = " + QuotedStr(CbxWrhsArea.SelectedValue) & _
        '    "WrhsType = " + QuotedStr(CbxWrhsType.SelectedItem.ToString) & _
        '    "FgActive = " + QuotedStr(CbxFgActive.SelectedValue) & _
        '    " where WrhsCode = " & QuotedStr(lbCode.Text)

        '    SQLExecuteNonQuery(SQLString)

        '    DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '    DataGrid.EditIndex = -1
        '    bindDataGrid()

        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbCode.Enabled = True
            ClearInput()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Module Code must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Module Name must be filled.")
                tbName.Focus()
                Return False
            End If
            If ddlStatusTanam.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Cluster must be filled.")
                ddlStatusTanam.Focus()
                Return False
            End If
            If IsNumeric(tbStartBJR.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Max Capacity must be in numeric.")
                tbStartBJR.Focus()
                Exit Function
            End If
            If IsNumeric(tbEndBJR.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Area must be in numeric.")
                tbEndBJR.Focus()
                Exit Function
            End If
            If tbHKPerBasis.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Dispensasi must be filled.")
                tbHKPerBasis.Focus()
                Exit Function
            End If


            If tbJanjangPerBasis.Text = "" Then
                lstatus.Text = MessageDlg("Janjang PerBasis must be filled.")
                tbJanjangPerBasis.Focus()
                Return False
            End If

            If tbKrgPerBasis.Text = "" Then
                lstatus.Text = MessageDlg("Karung PerBasis must be filled.")
                tbKrgPerBasis.Focus()
                Return False
            End If

            If tbJanjangPerBasisHt.Text = "" Then
                lstatus.Text = MessageDlg("Jenjang PerBasis HT must be filled.")
                tbJanjangPerBasisHt.Focus()
                Return False
            End If

            If tbKrgPerBasisHt.Text = "" Then
                lstatus.Text = MessageDlg("Karung PerBasis HT must be filled.")
                tbKrgPerBasisHt.Focus()
                Return False
            End If

            If tbPriceNBPerJanjang.Text = "" Then
                lstatus.Text = MessageDlg("Price NB Perjanjang must be filled.")
                tbPriceNBPerJanjang.Focus()
                Return False
            End If

            If tbPriceNBPerKrg.Text = "" Then
                lstatus.Text = MessageDlg("Price NB Perkarung must be filled.")
                tbPriceNBPerKrg.Focus()
                Return False
            End If

            If tbPriceNBPerJanjangHt.Text = "" Then
                lstatus.Text = MessageDlg("Price NB Perjenjang HT must be filled.")
                tbPriceNBPerJanjangHt.Focus()
                Return False
            End If


            If tbPriceNBPerKrgHt.Text = "" Then
                lstatus.Text = MessageDlg("Price NB Perkarung HT must be filled.")
                tbPriceNBPerKrgHt.Focus()
                Return False
            End If


            If tbPriceUPPerJanjang.Text = "" Then
                lstatus.Text = MessageDlg("Price UP PerJanjang must be filled.")
                tbPriceUPPerJanjang.Focus()
                Return False
            End If

            If tbPriceUPPerKrg.Text = "" Then
                lstatus.Text = MessageDlg("Price UP Perkarung must be filled.")
                tbPriceUPPerKrg.Focus()
                Return False
            End If

            If tbPriceUPPerJanjangHt.Text = "" Then
                lstatus.Text = MessageDlg("Price UP PerJanjang HT must be filled.")
                tbPriceUPPerJanjangHt.Focus()
                Return False
            End If

            If tbPriceUPPerKrgHt.Text = "" Then
                lstatus.Text = MessageDlg("Price UP Perkarung HT must be filled.")
                tbPriceUPPerKrgHt.Focus()
                Return False
            End If

            If tbPremiBasis.Text = "" Then
                lstatus.Text = MessageDlg("Premi Basis must be filled.")
                tbPremiBasis.Focus()
                Return False
            End If

            If tbPremiBasisKrg.Text = "" Then
                lstatus.Text = MessageDlg("Premi Basis Karung must be filled.")
                tbPremiBasisKrg.Focus()
                Return False
            End If

            If tbPremiBasisHt.Text = "" Then
                lstatus.Text = MessageDlg("Premi Basis HT must be filled.")
                tbPremiBasisHt.Focus()
                Return False
            End If

            If tbPremiBasisKrgHt.Text = "" Then
                lstatus.Text = MessageDlg("Premi Basis Karung HT must be filled.")
                tbPremiBasisKrgHt.Focus()
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
                If SQLExecuteScalar("SELECT PanenCode FROM V_MsPanenView WHERE PanenCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Panen Code " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'SqlString = "INSERT INTO MsPanen (PanenCode, PanenName, StatusTanam, StatusTanam2,StartBJR,EndBJR,HKPerBasis,EkuivalenAbnormal,JanjangperBasis,PriceNBPerJanjang,PriceUPPerJanjang,PremiBasis,JanjangperBasisHt,KrgPerBasis,KrgPerBasisHt,PriceNBPerJanjangHt,PriceNBPerKrg,PriceNBPerKrgHt,PriceUPPerJanjangHt,PriceUPPerKrg,PriceUPPerKrgHt,PremiBasisHt,PremiBasisKrg,PremiBasisKrgHt,UserId,UserDate ) " + _
                SqlString = "INSERT INTO MsPanen (PanenCode, PanenName, StatusTanam, StatusTanam2,StartBJR,EndBJR,HKPerBasis,EkuivalenAbnormal,JanjangperBasis,KrgPerBasis,JanjangperBasisHt,KrgPerBasisHt,PriceNBPerJanjang,PriceNBPerKrg,PriceNBPerJanjangHt,PriceNBPerKrgHt,PriceUPPerJanjang,PriceUPPerKrg,PriceUPPerJanjangHt,PriceUPPerKrgHt,PremiBasis,PremiBasisKrg,PremiBasisHt,PremiBasisKrgHt,UserId,UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(ddlStatusTanam.Text) + ", " + QuotedStr(ddlStatusTanam2.Text) + ", " + tbStartBJR.Text + ", " & _
                tbEndBJR.Text + ", " & _
                tbHKPerBasis.Text + ", " + _
                tbEkuivalenABnormal.Text + ", " + _
                tbJanjangPerBasis.Text + ", " + _
                tbKrgPerBasis.Text + ", " + _
                tbJanjangPerBasisHt.Text + ", " + _
                tbKrgPerBasisHt.Text + ", " + _
                tbPriceNBPerJanjang.Text + ", " + _
                tbPriceNBPerKrg.Text + ", " + _
                tbPriceNBPerJanjangHt.Text + ", " + _
                tbPriceNBPerKrgHt.Text + ", " + _
                tbPriceUPPerJanjang.Text + ", " + _
                tbPriceUPPerKrg.Text + ", " + _
                tbPriceUPPerJanjangHt.Text + ", " + _
                tbPriceUPPerKrgHt.Text + ", " + _
                tbPremiBasis.Text + ", " + _
                tbPremiBasisKrg.Text + ", " + _
                tbPremiBasisHt.Text + ", " + _
                tbPremiBasisKrgHt.Text + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "UPDATE MsPanen SET PanenName= " + QuotedStr(tbName.Text) & _
                            ", StatusTanam = " + QuotedStr(ddlStatusTanam.SelectedValue) & _
                            ", StatusTanam2 = " + QuotedStr(ddlStatusTanam2.SelectedValue) & _
                            ", StartBJR = " + tbStartBJR.Text & _
                            ", EndBJR = " + tbEndBJR.Text & _
                            ", HKPerBasis = " + tbHKPerBasis.Text & _
                            ", EkuivalenABnormal = " + tbEkuivalenABnormal.Text & _
                            ", JanjangPerBasis = " + tbJanjangPerBasis.Text.Replace(",", "") & _
                            ", KrgPerBasis = " + tbKrgPerBasis.Text.Replace(",", "") & _
                            ", JanjangPerBasisHt = " + tbJanjangPerBasisHt.Text.Replace(",", "") & _
                            ", KrgPerBasisHt = " + tbKrgPerBasisHt.Text.Replace(",", "") & _
                            ", PriceNBPerJanjang = " + tbPriceNBPerJanjang.Text.Replace(",", "") & _
                            ", PriceNBPerKrg = " + tbPriceNBPerKrg.Text.Replace(",", "") & _
                            ", PriceNBPerJanjangHt = " + tbPriceNBPerJanjangHt.Text.Replace(",", "") & _
                            ", PriceNBPerKrgHt = " + tbPriceNBPerKrgHt.Text.Replace(",", "") & _
                            ", PriceUPPerJanjang = " + tbPriceUPPerJanjang.Text.Replace(",", "") & _
                            ", PriceUPPerKrg = " + tbPriceUPPerKrg.Text.Replace(",", "") & _
                            ", PriceUPPerJanjangHt = " + tbPriceUPPerJanjangHt.Text.Replace(",", "") & _
                            ", PriceUPPerKrgHt = " + tbPriceUPPerKrgHt.Text.Replace(",", "") & _
                            ", PremiBasis = " + tbPremiBasis.Text.Replace(",", "") & _
                            ", PremiBasisKrg = " + tbPremiBasisKrg.Text.Replace(",", "") & _
                            ", PremiBasisHt = " + tbPremiBasisHt.Text.Replace(",", "") & _
                            ", PremiBasisKrgHt = " + tbPremiBasisKrgHt.Text.Replace(",", "") & _
                            " WHERE PanenCode = " + QuotedStr(tbCode.Text)

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

    ' Protected Sub ddlFgRecuring_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgRecuring.SelectedIndexChanged
    '    Try
    '  If ddlFgRecuring.SelectedValue = "Y" Then
    'tbMaxRecuring.Enabled = True
    'tbLeadTime.Enabled = True
    'tbMaxRecuring.Text = "0"
    'tbLeadTime.Text = "0"
    'Else
    'tbMaxRecuring.Enabled = False
    'tbLeadTime.Enabled = False
    'tbMaxRecuring.Text = "0"
    'tbLeadTime.Text = "0"
    'End If
    '   Catch ex As Exception
    '      lstatus.Text = "ddlFgRecuring_SelectedIndexChanged Error : " + ex.ToString
    ' End Try
    'End Sub

    Protected Sub tbPremiBasisHt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPremiBasisHt.TextChanged

    End Sub
End Class
