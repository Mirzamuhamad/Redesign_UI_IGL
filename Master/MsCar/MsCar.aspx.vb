Imports System.Data
Partial Class MsCar_MsCar
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlType, "SELECT JenisCar, JenisCarName FROM V_JenisCar", True, "JenisCar", "JenisCarName", ViewState("DBConnection"))
            FillCombo(ddlConsumtion, "SELECT JenisBahanBakar,JenisBahanBakarName FROM V_JenisBahanBakar", True, "JenisBahanBakar", "JenisBahanBakar", ViewState("DBConnection"))

            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            'tbMaxCap.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbArea.Attributes.Add("OnKeyDown", "return PressNumeric();")

        End If
        ' hasil dari search dialog
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnSupplier" Then
                BindToText(tbSupplier, Session("Result")(0).ToString)
                BindToText(tbSuppliertName, Session("Result")(1).ToString)

            End If
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
            If tbVehicleCode.Enabled Then
                tbVehicleCode.Text = ""
            End If

            tbVehicleName.Text = ""
            tbMerk.Text = ""
            'ddlType.SelectedIndex = 0
            tbModel.Text = ""
            tbMachine.Text = ""
            tbManufacture.Text = ""
            tbBPKB.Text = ""
            tbRangka.Text = ""
            tbColor.Text = ""
            tbCylinder.Text = ""
            'ddlConsumtion.SelectedValue = 0
            tbSupplier.Text = ""
            tbSuppliertName.Text = ""
            tbKmPerLtr.Text = "0"
            tbLength.Text = "0"
            tbwidth.Text = "0"
            tbheight.Text = "0"
            tbVolume.Text = "0"
            tbDriver.Text = ""
            tbMaintenance.Text = "0"
            tbRenewal.Text = "0"
            tbPeople.Text = "0"
            tbSTNK.SelectedDate = ViewState("ServerDate") 'Today
            tbBPKBDate.SelectedDate = ViewState("ServerDate") 'Today
            tbKIR.SelectedDate = ViewState("ServerDate") 'Today
            tbIjinTrayek.SelectedDate = ViewState("ServerDate") 'Today
            
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal CarNo As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM VMsCar  WHERE CarNo = " + QuotedStr(CarNo)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbVehicleCode, DT.Rows(0)("CarNo").ToString)
            BindToText(tbVehicleName, DT.Rows(0)("CarName").ToString)
            BindToText(tbMerk, DT.Rows(0)("Merk").ToString)
            BindToText(tbModel, DT.Rows(0)("Model").ToString)
            BindToText(tbManufacture, DT.Rows(0)("ManufactYear").ToString)
            BindToText(tbCylinder, DT.Rows(0)("Cylinder").ToString)
            BindToText(tbColor, DT.Rows(0)("Color").ToString)
            BindToDropList(ddlType, DT.Rows(0)("Type").ToString)
            BindToText(tbMachine, DT.Rows(0)("MachineNo").ToString)
            BindToText(tbRangka, DT.Rows(0)("RangkaNo").ToString)
            BindToText(tbBPKB, DT.Rows(0)("BPKBNo").ToString)
            BindToDropList(ddlConsumtion, DT.Rows(0)("Comsuption").ToString)
            BindToText(tbSupplier, DT.Rows(0)("Supplier").ToString)
            BindToText(tbSuppliertName, DT.Rows(0)("SupplierName").ToString)
            BindToText(tbKmPerLtr, DT.Rows(0)("KmPerLtr").ToString)
            BindToText(tbLength, DT.Rows(0)("Length").ToString)
            BindToText(tbwidth, DT.Rows(0)("Width").ToString)
            BindToText(tbheight, DT.Rows(0)("Height").ToString)
            BindToText(tbVolume, DT.Rows(0)("Volume").ToString)
            BindToText(tbDriver, DT.Rows(0)("Driver").ToString)
            BindToText(tbMaintenance, DT.Rows(0)("Maintenance").ToString)
            BindToText(tbRenewal, DT.Rows(0)("Renewal").ToString)
            BindToText(tbPeople, DT.Rows(0)("People").ToString)
            BindToDate(tbSTNK, DT.Rows(0)("DueStnk").ToString)
            BindToDate(tbBPKBDate, DT.Rows(0)("DueBPKB").ToString)
            BindToDate(tbKIR, DT.Rows(0)("DueKIR").ToString)
            BindToDate(tbIjinTrayek, DT.Rows(0)("DueIjinTrayek").ToString)
            BindToDropList(ddlFgActive, DT.Rows(0)("FgActive").ToString)
            BindToDropList(ddlFgAngkut, DT.Rows(0)("FgInternalDelivery").ToString)
            BindToDropList(ddlFgKirim, DT.Rows(0)("FgExternalDelivery").ToString)

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
            SqlString = "SELECT * From VMsCar " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CarNo ASC"
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
            'ddlField2.SelectedValue.Replace("CarNo", "CarNo")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMasterBlockFile 'VMsCar','CarNo','CarName','Merk','Model','ManufactYear','Cylinder','Color','MachineNo','RangkaNo','BPKBNo','Comsuption','KmPerLtr','Type','FgActive', 'CAR FILE', 'Car No','Car Name','Merk','Model','Manufact Year','Cylinder','Color','Machine No','Rangka No','BPKB No','Comsuption','Km/Ltr','Type','FgActive'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMasterCarFile.frx"
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
                    tbVehicleCode.Enabled = False
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
                    tbVehicleCode.Enabled = False
                    btnHome.Visible = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                    btnCancel.Visible = True
                    tbVehicleCode.Focus()
                    'ElseIf DDL.SelectedValue = "Non Active" Then
                    '    Try
                    '        If CheckMenuLevel("Delete") = False Then
                    '            Exit Sub
                    '        End If
                    '        If GVR.Cells(8).Text = "N" Then
                    '            lstatus.Text = "<script language='javascript'> {alert('Job Plantation closed already')}</script>"
                    '            Exit Sub
                    '        End If
                    '        SQLExecuteNonQuery("UPDATE MsBlock SET Fgactive = 'N' WHERE BlockCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                    '        bindDataGrid()
                    '    Catch ex As Exception
                    '        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    '    End Try
                    'ElseIf DDL.SelectedValue = "Delete" Then
                    '    Try
                    '        If CheckMenuLevel("Delete") = False Then
                    '            Exit Sub
                    '        End If
                    '        SQLExecuteNonQuery("DELETE MsBlock WHERE BlockCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                    '        bindDataGrid()
                    '    Catch ex As Exception
                    '        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    '    End Try
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
            tbVehicleCode.Enabled = True
            'FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            ClearInput()
            ModifyInput(True, pnlInput)
            BtnSave.Visible = True
            btnReset.Visible = True
            btnCancel.Visible = True
            'ddlActive.Enabled = False
            btnHome.Visible = False
            tbVehicleCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbVehicleCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Block Code must be filled.")
                tbVehicleCode.Focus()
                Return False
            End If
            If tbVehicleName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Block Name must be filled.")
                tbVehicleName.Focus()
                Return False
            End If
            If ddlConsumtion.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg(" Est Start Tanam must be filled.")
                ddlConsumtion.Focus()
                Return False
            End If

            If tbSupplier.Text = "" Then
                lstatus.Text = MessageDlg("Supplier be filled.")
                tbSupplier.Focus()
                Return False
            End If

            If tbSuppliertName.Text = "" Then
                lstatus.Text = MessageDlg("Supplier Name must be filled.")
                tbSuppliertName.Focus()
                Return False
            End If

            If tbKmPerLtr.Text = "" Then
                lstatus.Text = MessageDlg("Km/Ltr must be filled.")
                tbKmPerLtr.Focus()
                Return False
            End If

            If tbLength.Text = "" Then
                lstatus.Text = MessageDlg("Lenght  must be filled.")
                tbLength.Focus()
                Return False
            End If

            If tbwidth.Text = "" Then
                lstatus.Text = MessageDlg("Width  must be filled.")
                tbwidth.Focus()
                Return False
            End If


            If tbheight.Text = "" Then
                lstatus.Text = MessageDlg("Height  must be filled.")
                tbheight.Focus()
                Return False
            End If


            If tbVolume.Text = "" Then
                lstatus.Text = MessageDlg("Volume  must be filled.")
                tbVolume.Focus()
                Return False
            End If

            If tbMaintenance.Text = "" Then
                lstatus.Text = MessageDlg("Maintenance  must be filled.")
                tbMaintenance.Focus()
                Return False
            End If


            If tbRenewal.Text = "" Then
                lstatus.Text = MessageDlg("Renewal must be filled.")
                tbRenewal.Focus()
                Return False
            End If

            If tbSTNK.Text = "" Then
                lstatus.Text = MessageDlg("STNK must be filled.")
                tbSTNK.Focus()
                Return False
            End If

            If ddlType.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Type must be filled.")
                ddlType.Focus()
                Return False
            End If

            If tbBPKB.Text = "" Then
                lstatus.Text = MessageDlg("BPKB must be filled.")
                tbBPKB.Focus()
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
                If SQLExecuteScalar("SELECT CarNo FROM VMsCar WHERE CarNo = " + QuotedStr(tbVehicleCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "CarNo " + QuotedStr(tbVehicleCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsCar (CarNo,	CarName,	Merk,	Model,	ManufactYear,	Cylinder,	Color,	Type, MachineNo, 	RangkaNo " + _
                ",BPKBNo,	Comsuption, Supplier,	KmPerLtr,	Length,	Width,	Height,	Volume,	Driver,	Maintenance,	Renewal, People " + _
                ",DueSTNK,	DueBPKB,	DueKIR, DueIjinTrayek, FgActive	,FgInternalDelivery, FgExternalDelivery " + _
                ",UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbVehicleCode.Text) + ", " + QuotedStr(tbVehicleName.Text) + ", " & _
                QuotedStr(tbMerk.Text) + ", " + QuotedStr(tbModel.Text) + ", " & _
                QuotedStr(tbManufacture.Text) + ", " & _
                QuotedStr(tbCylinder.Text) + ", " + _
                QuotedStr(tbColor.Text) + ", " + _
                QuotedStr(ddlType.SelectedValue) + ", " + _
                QuotedStr(tbMachine.Text) + ", " + _
                QuotedStr(tbRangka.Text) + ", " + _
                QuotedStr(tbBPKB.Text) + ", " + _
                QuotedStr(ddlConsumtion.SelectedValue) + ", " + _
                QuotedStr(tbSupplier.Text) + ", " + _
                QuotedStr(tbKmPerLtr.Text) + ", " + _
                QuotedStr(tbLength.Text) + ", " + _
                QuotedStr(tbwidth.Text) + ", " + _
                QuotedStr(tbheight.Text) + ", " + _
                QuotedStr(tbVolume.Text) + ", " + _
                QuotedStr(tbDriver.Text) + ", " + _
                QuotedStr(tbMaintenance.Text) + ", " + _
                QuotedStr(tbRenewal.Text) + ", " + _
                QuotedStr(tbPeople.Text) + ", " + _
                QuotedStr(tbSTNK.SelectedDate) + ", " + _
                QuotedStr(tbBPKBDate.SelectedDate) + ", " + _
                QuotedStr(tbKIR.SelectedDate) + ", " + _
                QuotedStr(tbIjinTrayek.SelectedDate) + ", " + _
                QuotedStr(ddlFgActive.SelectedValue) + ", " + _
                QuotedStr(ddlFgAngkut.SelectedValue) + ", " + _
                QuotedStr(ddlFgKirim.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

            Else
                SqlString = "UPDATE MsCar SET CarName = " + QuotedStr(tbVehicleName.Text) & _
                            ", Merk = " + QuotedStr(tbMerk.Text) & _
                            ", Model = " + QuotedStr(tbModel.Text) & _
                            ", ManufactYear = " + QuotedStr(tbManufacture.Text) & _
                            ", Cylinder = " + QuotedStr(tbCylinder.Text) & _
                            ", Color = " + QuotedStr(tbColor.Text) & _
                            ", MachineNo = " + QuotedStr(tbMachine.Text) & _
                            ", Type = " + QuotedStr(ddlType.SelectedValue) & _
                            ", RangkaNo = " + QuotedStr(tbRangka.Text) & _
                            ", BPKBNo = " + QuotedStr(tbBPKB.Text) & _
                            ", Comsuption = " + QuotedStr(ddlConsumtion.SelectedValue) & _
                            ", KmPerLtr = " + QuotedStr(tbKmPerLtr.Text) & _
                            ", Length = " + QuotedStr(tbLength.Text) & _
                            ", Width = " + QuotedStr(tbwidth.Text) & _
                            ", Height = " + QuotedStr(tbheight.Text) & _
                            ", Volume = " + QuotedStr(tbVolume.Text) & _
                            ", Driver = " + QuotedStr(tbDriver.Text) & _
                            ", Maintenance = " + QuotedStr(tbMaintenance.Text) & _
                            ", Renewal = " + QuotedStr(tbRenewal.Text) & _
                            ", People = " + QuotedStr(tbPeople.Text) & _
                            ", DueSTNK = " + QuotedStr(tbSTNK.SelectedDate) & _
                            ", DueBPKB = " + QuotedStr(tbBPKBDate.SelectedDate) & _
                            ", DueKIR = " + QuotedStr(tbKIR.SelectedDate) & _
                            ", DueIjinTrayek = " + QuotedStr(tbIjinTrayek.SelectedDate) & _
                            ", FgActive = " + QuotedStr(ddlFgActive.SelectedValue) & _
                            ", FgInternalDelivery = " + QuotedStr(ddlFgAngkut.SelectedValue) & _
                            ", FgExternalDelivery = " + QuotedStr(ddlFgKirim.SelectedValue) & _
                            " WHERE CarNo = " + QuotedStr(tbVehicleCode.Text)
               
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
            tbVehicleName.Focus()
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
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "SELECT Supplier_Code, Supplier_Name,Supplier_Type,Supplier_Class, GroupType FROM V_MsSupplier Where GroupType = 'Panen' And FgActive='Y' "
            CriteriaField = "Supplier_Code, Supplier_Name,Supplier_Type,Supplier_Class, GroupType"
            ResultField = "Supplier_Code, Supplier_Name,Supplier_Type,Supplier_Class, GroupType"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub
End Class
