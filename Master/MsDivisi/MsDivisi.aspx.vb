Imports System.Data

Partial Class Master_MsDivisi_MsDivisi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            FillCombo(ddlestate, "Select EstateCode, EstateName From V_MsEstate", True, "EstateCode", "EstateName", ViewState("DBConnection"))
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If

        If Not Session("Result") Is Nothing Then
            'Dim tbmanager, tbmanagerName As New TextBox
            If ViewState("Sender") = "btnmanager" Then
                tbmanager.Text = Session("Result")(0).ToString
                tbmanagerName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnaccounting" Then
                tbaccounting.Text = Session("Result")(0).ToString
                tbaccountingName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnasisten" Then
                tbasisten.Text = Session("Result")(0).ToString
                tbasistenname.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnaskep" Then
                tbaskep.Text = Session("Result")(0).ToString
                tbaskepname.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnaskepproduksi" Then
                tbaskepproduksi.Text = Session("Result")(0).ToString
                tbaskepproduksiName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnauditor" Then
                tbauditor.Text = Session("Result")(0).ToString
                tbauditorName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnkrani" Then
                tbkrani.Text = Session("Result")(0).ToString
                tbkraniName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnktu" Then
                tbKTU.Text = Session("Result")(0).ToString
                tbKTUName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnmandor" Then
                tbmandor.Text = Session("Result")(0).ToString
                tbmandorname.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnppic" Then
                tbPPIC.Text = Session("Result")(0).ToString
                tbPPICName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnwarehouse" Then
                tbwarehouse.Text = Session("Result")(0).ToString
                tbwarehouseName.Text = Session("Result")(1).ToString
            End If
            'tbAccount.Focus()
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
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim StrFilter As String
        Try
            tempDS = SQLExecuteQuery(" SELECT * From V_MsDivisiView ", ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView
            StrFilter = ""
            If tbFilter.Text.Trim.Length > 0 Then
                If tbfilter2.Text.Trim.Length > 0 And pnlSearch.Visible Then
                    StrFilter = ddlField.SelectedValue + " like '%" + tbFilter.Text + "%' " + _
                    ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " like '%" + tbfilter2.Text + "%'"
                Else
                    StrFilter = ddlField.SelectedValue + " like '%" + tbFilter.Text + "%'"
                End If
            Else
                StrFilter = ""
            End If
            DV.RowFilter = StrFilter

            If DV.Count = 0 Then
                'Dim DT As DataTable = tempDS.Tables(0)
                'ShowGridViewIfEmpty(DT, DataGrid)
                'DV = DT.DefaultView
                lstatus.Text = "No Data"
                DataGrid.Visible = False
            Else
                If ViewState("SortExpression") = Nothing Then
                    ViewState("SortExpression") = "DivisionCode ASC"
                End If

                DataGrid.Visible = True
                DV.Sort = ViewState("SortExpression")
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim GVR As GridViewRow = Nothing
        Dim DDL As DropDownList
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Insert" Then
                
            ElseIf e.CommandName = "View" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                ViewState("Nmbr") = GVR.Cells(1).Text
                pnlHd.Visible = False
            ElseIf e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                ViewState("Nmbr") = GVR.Cells(1).Text
                If DDL.SelectedValue = "View" Then
                    ViewState("State") = "View"
                    FillTextBox(ViewState("Nmbr"))
                    BtnSave.Visible = False
                    btnReset.Visible = False
                    ModifyInput(False)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    ViewState("State") = "Edit"
                    FillTextBox(ViewState("Nmbr"))
                    ModifyInput(True)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                    tbDivisionCode.Enabled = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                    tbDivisionName.Focus()
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("UPDATE MsDivision SET FgActive = 'N' where DivisionCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        'SQLExecuteNonQuery("Delete from MsPayTypeCharge where DivisionCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        'SQLExecuteNonQuery("Delete from MsPayType where DivisionCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Dim StrFilter, SQLString As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            SQLString = "S_FormPrintMaster5 'V_MsDivisiView', 'DivisionCode', 'DivisionName', 'EstateName', 'Area', 'FgActive'," + QuotedStr(lbltitle.text) + ", 'Divisi Code', 'Divisi Name', 'Estate', 'Area', 'Active', " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            'lstatus.Text = SQLString
            'Exit Sub
            'SQLString = Replace(SQLString, "DivisionCode", "Payment_Code")
            'SQLString = Replace(SQLString, "DivisionName", "Payment_Name")
            'SQLString = Replace(SQLString, "CurrCode", "Currency")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbDivisionCode.Enabled = True
            ModifyInput(True)
            ClearInput()
            tbDivisionCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ClearInput()
        Try
            If tbDivisionCode.Enabled Then
                tbDivisionCode.Text = ""
            End If

            tbDivisionName.Text = ""
            tbarea.Text = "0"
            ddlestate.SelectedIndex = 0

            tbmanager.Text = ""
            tbmanagerName.Text = ""
            tbKTU.Text = ""
            tbKTUName.Text = ""
            tbasisten.Text = ""
            tbasistenname.Text = ""
            tbauditor.Text = ""
            tbauditorName.Text = ""
            tbaccounting.Text = ""
            tbaccountingName.Text = ""
            tbwarehouse.Text = ""
            tbwarehouseName.Text = ""
            tbmandor.Text = ""
            tbmandorname.Text = ""
            tbPPIC.Text = ""
            tbPPICName.Text = ""
            tbaskep.Text = ""
            tbaskepname.Text = ""
            tbkrani.Text = ""
            tbkraniName.Text = ""
            tbaskepproduksi.Text = ""
            tbaskepproduksiName.Text = ""
            tbarea.Enabled = False
            ddlfgbatch.SelectedValue = "N"
            ddlActive.SelectedValue = "Y"
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
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

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbDivisionName.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If
            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT DivisionCode From MsDivision WHERE DivisionCode = " + QuotedStr(tbDivisionCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Division " + QuotedStr(tbDivisionCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "Insert into MsDivision (DivisionCode, DivisionName, Estate, Area, EmpManager, EmpAsisten, EmpAskep, EmpPPIC, EmpMandor, EmpAuditor, EmpWarehouse, EmpKTU, " + _
                "EmpAccounting, EmpKrani, EmpAskepProduksi, FgBatch, FgActive, UserID, UserDate) " + _
                "SELECT " + QuotedStr(tbDivisionCode.Text) + "," + QuotedStr(tbDivisionName.Text) + "," + QuotedStr(ddlestate.SelectedValue) + ", 0," + _
                QuotedStr(tbmanager.Text) + "," + QuotedStr(tbasisten.Text) + ",  " + QuotedStr(tbaskep.Text) + ", " + QuotedStr(tbPPIC.Text) + ", " + QuotedStr(tbmandor.Text) + ", " + QuotedStr(tbauditor.Text) + ", " + _
                QuotedStr(tbwarehouse.Text) + ", " + QuotedStr(tbKTU.Text) + "," + QuotedStr(tbaccounting.Text) + ", " + QuotedStr(tbkrani.Text) + "," + _
                QuotedStr(tbaskepproduksi.Text) + "," + QuotedStr(ddlfgbatch.SelectedValue) + "," + QuotedStr(ddlActive.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "Update MsDivision set DivisionName =" + QuotedStr(tbDivisionName.Text) + ", Estate =" + QuotedStr(ddlestate.SelectedValue) + ", Area =" + QuotedStr(tbarea.Text) + ", " + _
                "EmpManager = " + QuotedStr(tbmanager.Text.Trim) + ", EmpAsisten = " + QuotedStr(tbasisten.Text.Trim) + ", EmpAskep = " + QuotedStr(tbaskep.Text.Trim) + ", EmpPPIC = " + QuotedStr(tbPPIC.Text.Trim) + ", " + _
                "EmpMandor = " + QuotedStr(tbmandor.Text.Trim) + ", EmpAuditor = " + QuotedStr(tbauditor.Text.Trim) + ", EmpWarehouse = " + QuotedStr(tbwarehouse.Text.Trim) + ", EmpKTU = " + QuotedStr(tbKTU.Text.Trim) + ", " + _
                "EmpAccounting = " + QuotedStr(tbaccounting.Text.Trim) + ", EmpKrani = " + QuotedStr(tbkrani.Text.Trim) + ", EmpAskepProduksi = " + QuotedStr(tbaskepproduksi.Text.Trim) + ", FgBatch = " + QuotedStr(ddlfgbatch.SelectedValue) + ", " + _
                "FgActive = " + QuotedStr(ddlActive.SelectedValue) + ", UserId = " + QuotedStr(ViewState("UserId").ToString) + ", UserDate = GetDate() " + _
                " Where DivisionCode = " & QuotedStr(tbDivisionCode.Text)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Private Function cekInput() As Boolean
        Try
            If tbDivisionCode.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Division Code must be filled');</script>"
                tbDivisionCode.Focus()
                Exit Function
            End If
            If tbDivisionName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Division Name must be filled');</script>"
                tbDivisionName.Focus()
                Exit Function
            End If
            If ddlestate.SelectedValue = "" Then
                lstatus.Text = "<script language='javascript'>alert('Estate must be filled');</script>"
                ddlestate.Focus()
                Exit Function
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function


    Protected Sub FillTextBox(ByVal DivisionCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsDivisiView A WHERE DivisionCode = " + QuotedStr(DivisionCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbDivisionCode, DT.Rows(0)("DivisionCode").ToString)
            BindToText(tbDivisionName, DT.Rows(0)("DivisionName").ToString)
            BindToText(tbarea, FormatFloat(DT.Rows(0)("Area").ToString, 2))
            BindToDropList(ddlestate, DT.Rows(0)("Estate").ToString)
            BindToText(tbmanager, DT.Rows(0)("EmpManager").ToString)
            BindToText(tbmanagerName, DT.Rows(0)("EmpManagerName").ToString)
            BindToText(tbKTU, DT.Rows(0)("EmpKTU").ToString)
            BindToText(tbKTUName, DT.Rows(0)("EmpKTUName").ToString)
            BindToText(tbasisten, DT.Rows(0)("EmpAsisten").ToString)
            BindToText(tbasistenname, DT.Rows(0)("EmpAsistenName").ToString)
            BindToText(tbauditor, DT.Rows(0)("EmpAuditor").ToString)
            BindToText(tbauditorName, DT.Rows(0)("EmpAuditorName").ToString)
            BindToText(tbaccounting, DT.Rows(0)("EmpAccounting").ToString)
            BindToText(tbaccountingName, DT.Rows(0)("EmpAccountingName").ToString)
            BindToText(tbmandor, DT.Rows(0)("EmpMandor").ToString)
            BindToText(tbmandorname, DT.Rows(0)("EmpMandorName").ToString)
            BindToText(tbwarehouse, DT.Rows(0)("EmpWarehouse").ToString)
            BindToText(tbwarehouseName, DT.Rows(0)("EmpWarehouseName").ToString)
            BindToText(tbPPIC, DT.Rows(0)("EmpPPIC").ToString)
            BindToText(tbPPICName, DT.Rows(0)("EmpPPICName").ToString)
            BindToText(tbaskepproduksi, DT.Rows(0)("Empaskepproduksi").ToString)
            BindToText(tbaskepproduksiName, DT.Rows(0)("EmpaskepproduksiName").ToString)
            BindToText(tbaskep, DT.Rows(0)("EmpAskep").ToString)
            BindToText(tbaskepname, DT.Rows(0)("EmpAskepName").ToString)
            BindToDropList(ddlActive, DT.Rows(0)("FgActive").ToString)
            BindToDropList(ddlfgbatch, DT.Rows(0)("FgBatch").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        tbDivisionCode.Enabled = State
        tbDivisionName.Enabled = State
        tbarea.Enabled = False
        ddlestate.Enabled = State
        tbmanager.Enabled = State
        tbKTU.Enabled = State
        tbkrani.Enabled = State
        tbasisten.Enabled = State
        tbauditor.Enabled = State
        tbaccounting.Enabled = State
        tbmandor.Enabled = State
        tbwarehouse.Enabled = State
        tbPPIC.Enabled = State
        tbaskep.Enabled = State
        tbaskepproduksi.Enabled = State
        'btnAcc.Visible = State
        BtnSave.Visible = State
        btnReset.Visible = State
        ddlfgbatch.Enabled = State
        ddlActive.Enabled = State
    End Sub


    Protected Sub btnasisten_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnasisten.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnasisten"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnasisten Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnaskep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaskep.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnaskep"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnaskep Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnaskepproduksi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaskepproduksi.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnaskepproduksi"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnaskepproduksi Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnauditor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnauditor.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnauditor"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnauditor Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnkrani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnkrani.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnkrani"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnkrani Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnKTU_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKTU.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnktu"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnKTU Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnmanager_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmanager.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnmanager"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnmanager Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnmandor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmandor.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnmandor"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnmandor Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPPIC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPPIC.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnppic"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnPPIC Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnwarehouse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnwarehouse.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnwarehouse"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnwarehouse Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnaccounting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaccounting.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name, Job_Title_Name, Job_Level_Name, Work_Place_Name from V_MsEmployee WHERE Fg_Active = 'Y' "
            FieldResult = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnaccounting"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn btnaccounting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbaccounting_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbaccounting.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbaccounting.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbaccounting.Text = ""
                tbaccountingName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbaccounting.Text = dr("Emp_No").ToString
                tbaccountingName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbaccounting Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbasisten_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbasisten.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbasisten.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbasisten.Text = ""
                tbasistenname.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbasisten.Text = dr("Emp_No").ToString
                tbasistenname.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbasisten Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbaskep_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbaskep.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbaskep.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbaskep.Text = ""
                tbaskepname.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbaskep.Text = dr("Emp_No").ToString
                tbaskepname.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbaskep Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbaskepproduksi_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbaskepproduksi.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbaskepproduksi.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbaskepproduksi.Text = ""
                tbaskepproduksiName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbaskepproduksi.Text = dr("Emp_No").ToString
                tbaskepproduksiName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbaskepproduksi Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbauditor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbauditor.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbauditor.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbauditor.Text = ""
                tbauditorName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbauditor.Text = dr("Emp_No").ToString
                tbauditorName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbauditor Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbkrani_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbkrani.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbkrani.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbkrani.Text = ""
                tbkraniName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbkrani.Text = dr("Emp_No").ToString
                tbkraniName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbkrani Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbKTU_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKTU.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbKTU.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbKTU.Text = ""
                tbKTUName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbKTU.Text = dr("Emp_No").ToString
                tbKTUName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbKTU Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbmanager_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbmanager.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbmanager.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbmanager.Text = ""
                tbmanagerName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbmanager.Text = dr("Emp_No").ToString
                tbmanagerName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbmanager Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbmandor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbmandor.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbmandor.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbmandor.Text = ""
                tbmandorname.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbmandor.Text = dr("Emp_No").ToString
                tbmandorname.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbmandor Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPPIC_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPIC.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbPPIC.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbPPIC.Text = ""
                tbPPICName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbPPIC.Text = dr("Emp_No").ToString
                tbPPICName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbPPIC Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbwarehouse_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbwarehouse.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No =" + QuotedStr(tbwarehouse.Text.Trim), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbwarehouse.Text = ""
                tbwarehouseName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbwarehouse.Text = dr("Emp_No").ToString
                tbwarehouseName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbwarehouse Changed Error : " + ex.ToString
        End Try
    End Sub
End Class
