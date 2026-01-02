Imports System.Data
Public Class tbKTPNo
    Public Sub New()

    End Sub
End Class
Partial Class MsMemberPlasma_MsMemberPlasma
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlVillage, "SELECT VillageCode, VillageName FROM MsVillage", True, "VillageCode", "VillageName", ViewState("DBConnection"))
            FillCombo(ddlEstate, "SELECT EstateCode, EstateName FROM MsEstate", True, "EstateCode", "EstateName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            'tbMemberID.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKTPNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKKNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuas.Attributes.Add("OnKeyDown", "return PressNumeric();")


        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnSupplier" Then
                BindToText(tbSupplier, Session("Result")(0).ToString)
                BindToText(tbSupplierName, Session("Result")(1).ToString)
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
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
            If tbMemberID.Enabled Then
                tbMemberID.Text = ""
            End If

            tbMemberName.Text = ""
            tbMemberName.Text = ""
            tbKTPNo.Text = ""
            tbKKNo.Text = ""
            ddlVillage.SelectedIndex = 0
            ddlEstate.SelectedIndex = 0
            tbLuas.Text = ""
            tbSupplier.Text = ""
            tbSupplierName.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal BlockCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsMemberPlasmaView WHERE MemberID = " + QuotedStr(BlockCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbMemberID, DT.Rows(0)("MemberID").ToString)
            BindToText(tbMemberName, DT.Rows(0)("MemberNAme").ToString)
            BindToText(tbKTPNo, DT.Rows(0)("KTPNo").ToString)
            BindToText(tbKKNo, DT.Rows(0)("KKNo").ToString)
            BindToText(tbSupplier, DT.Rows(0)("Supplier").ToString)
            BindToText(tbSupplierName, DT.Rows(0)("SupplierName").ToString)
            BindToDropList(ddlVillage, DT.Rows(0)("Village").ToString)
            BindToDropList(ddlEstate, DT.Rows(0)("Estate").ToString)
            BindToText(tbLuas, CFloat(DT.Rows(0)("Area").ToString))
            BindToDate(tbStartJoin, DT.Rows(0)("StartJoin").ToString)
            BindToDate(tbEndJoin, DT.Rows(0)("EndJoin").ToString)
            'FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))

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
            SqlString = "SELECT * From V_MsMemberPlasmaView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MemberID ASC"
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
            Session("SelectCommand") = "S_FormPrintMaster6 'V_MsMemberPlasmaView','MemberID','MemberName','KTPNo','KKNo','VillageName','EstateName','Member Plasma','Member ID','Member Name','KTPNo','KKNo','Village','Estate'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
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
                    'tbMemeberID.Enabled = False
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
                    tbMemberID.Enabled = False
                    btnHome.Visible = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                    btnCancel.Visible = True
                    tbMemberName.Focus()
                ElseIf DDL.SelectedValue = "Non Active" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        If GVR.Cells(8).Text = "N" Then
                            lstatus.Text = "<script language='javascript'> {alert('Job Plantation closed already')}</script>"
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("UPDATE MsMemberPlasma SET Fgactive = 'N' WHERE MemberID = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("DELETE MsMemberPlasma WHERE MemberID = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
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
            tbMemberID.Enabled = True
            'FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            ClearInput()
            ModifyInput(True, pnlInput)
            BtnSave.Visible = True
            btnReset.Visible = True
            btnCancel.Visible = True
            'ddlActive.Enabled = False
            btnHome.Visible = False
            tbMemberID.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub


    Private Function cekInput() As Boolean
        Try
            If tbMemberID.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Member ID must be filled.")
                tbMemberID.Focus()
                Return False
            End If
            If tbMemberName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Member Name Name must be filled.")
                tbMemberName.Focus()
                Return False
            End If
            If tbKTPNo.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("KTP No must be filled.")
                tbKTPNo.Focus()
                Return False
            End If
            If tbKKNo.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("KK No must be filled.")
                tbKKNo.Focus()
                Return False
            End If
            If ddlVillage.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg(" Vilage must be filled.")
                ddlVillage.Focus()
                Return False
            End If
            If ddlEstate.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Estate must be filled.")
                ddlEstate.Focus()
                Return False
            End If
            If tbLuas.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Luas must be filled.")
                tbLuas.Focus()
                Return False
            End If
            If tbStartJoin.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Start Join must be filled.")
                tbStartJoin.Focus()
                Return False
            End If
            If tbEndJoin.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("End Join must be filled.")
                tbEndJoin.Focus()
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
                If SQLExecuteScalar("SELECT MemberID FROM MsMemberPlasma WHERE MemberID = " + QuotedStr(tbMemberName.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Member ID " + QuotedStr(tbMemberName.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsMemberPlasma (MemberID, MemberName, KTPNo, KKNo, Village,Supplier, Estate, Area, StartJoin, EndJoin,FgInti, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbMemberID.Text) + ", " + QuotedStr(tbMemberName.Text) + ", " & _
                QuotedStr(tbKTPNo.Text) + ", " + QuotedStr(tbKKNo.Text) + ", " & _
                QuotedStr(ddlVillage.Text) + ", " + _
                QuotedStr(tbSupplier.Text) + ", " + _
                QuotedStr(ddlEstate.Text) + ", " + _
                QuotedStr(tbLuas.Text) + ", " + _
                QuotedStr(tbStartJoin.SelectedValue) + ", " + _
                QuotedStr(tbEndJoin.SelectedValue) + ", " + _
                QuotedStr(ddlFgInti.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                'QuotedStr(tbStatusTanam.Text) + ", " + _
                'QuotedStr(tbQtyTanam.Text) + ", " + _
                'QuotedStr(tbSPH.Text) + ", " + _

            Else
                SqlString = "UPDATE MsMemberPlasma SET MemberName= " + QuotedStr(tbMemberName.Text) & _
                            ", KTPNo = " + QuotedStr(tbKTPNo.Text) & _
                            ", KKNo = " + QuotedStr(tbKKNo.Text) & _
                            ", Village = " + QuotedStr(ddlVillage.Text) & _
                            ", Estate  = " + QuotedStr(ddlEstate.Text) & _
                            ", Supplier  = " + QuotedStr(tbSupplier.Text) & _
                            ", Area    = " + QuotedStr(tbLuas.Text) & _
                            ", StartJoin = " + QuotedStr(tbStartJoin.SelectedValue) & _
                            ", EndJoin = " + QuotedStr(tbEndJoin.SelectedValue) & _
                            ", FgInti = " + QuotedStr(ddlFgInti.SelectedValue) & _
                            " WHERE MemberID = " + QuotedStr(tbMemberID.Text)
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
            tbMemberID.Focus()
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
    Protected Sub btnSupplier_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupplier.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Supplier_Code, Supplier_Name FROM V_MsSupplier "
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid.SelectedIndexChanged

    End Sub

    Protected Sub tbMemberID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMemberID.TextChanged

    End Sub
End Class
