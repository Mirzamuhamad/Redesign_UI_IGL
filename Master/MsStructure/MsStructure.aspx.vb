Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO
Imports System.Configuration

Partial Class MsStructure_MsStructureNew
    Inherits System.Web.UI.Page

    Dim SQLString, strAreaCode, strKawasanCode, strLevelCode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            'FillCombo(ddlKawasan, "SELECT KawasanCode, KawasanName FROM MsKawasan ORDER BY KawasanName", True, "KawasanCode", "KawasanName", ViewState("DBConnection"))
            'FillCombo(ddlLandType, "SELECT LandTypeCode, LandTypeName FROM MsLandType", True, "LandTypeCode", "LandTypeName", ViewState("DBConnection"))
            'FillCombo(ddlArea, "SELECT AreaCode, AreaName FROM V_MsArea ORDER BY AreaName", True, "AreaCode", "AreaName", ViewState("DBConnection"))
            FillCombo(ddlLevel, "SELECT LevelCode, LevelName FROM MsLevelProperty ORDER BY LevelCode", True, "LevelCode", "LevelName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
            'tbMaxCap.Attributes.Add("OnKeyDown ", "return PressNumeric();")
            'tbArea.Attributes.Add("OnKeyDown", "return PressNumeric();")
        End If
        If Not Session("Result") Is Nothing Then

            If ViewState("Sender") = "btnAccount" Then
                tbAccount.Text = Session("Result")(0).ToString
                tbAccountName.Text = Session("Result")(1).ToString
                'BindToDropList(ddlTerm, Session("Result")(4).ToString)
                'ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection").ToString)
                'tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
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
            'If tbCode.Enabled Then
            tbCode.Text = ""
            'End If
            tbParentID.Text = ""
            tbStructureCode.Text = ""
            tbStructureName.Text = ""
            tbAccount.Text = ""
            tbAccountName.Text = ""
            ddlLevel.SelectedIndex = 0
            'ddlKawasan.SelectedIndex = 0
            'tbArea.Text = "0"
            'tbEstStartPlant.SelectedDate = ViewState("ServerDate") 'Today
            'tbStartPlant.SelectedDate = ViewState("ServerDate") 'Today
            'ddlLandScape.SelectedIndex = 0
            tbLuasLahan.Text = "0"
            tbLuasBangunan.Text = "0"
            'tbQtyTanam.Text = ""
            'tbSPH.Text = ""
            'ddlFgPanen.SelectedIndex = 1
            'ddlKSUBlock.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal StructureCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsStructure  WHERE ID = " + QuotedStr(StructureCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("ID").ToString)
            BindToText(tbParentID, DT.Rows(0)("ParentID").ToString)
            BindToText(tbStructureCode, DT.Rows(0)("StructureCode").ToString)
            BindToText(tbStructureName, DT.Rows(0)("StructureName").ToString)
            BindToDropList(ddlLevel, DT.Rows(0)("LevelCode").ToString)
            BindToText(tbLuasLahan, DT.Rows(0)("LandArea").ToString)
            BindToText(tbLuasBangunan, DT.Rows(0)("BuildingArea").ToString)
            BindToText(tbAccount, DT.Rows(0)("Account").ToString)
            BindToText(tbAccountName, DT.Rows(0)("AccountName").ToString)
            'BindToDate(tbEstStartPlant, DT.Rows(0)("EstStartPlant").ToString)
            'BindToDate(tbStartPlant, DT.Rows(0)("StartPlant").ToString)
            'BindToDropList(ddlLandType, DT.Rows(0)("LandType").ToString)
            'BindToDropList(ddlLandScape, DT.Rows(0)("LandScape").ToString)
            'BindToText(tbLuasBangunan, CFloat(DT.Rows(0)("QtyTanam").ToString))
            'BindToText(tbSPH, FormatNumber(DT.Rows(0)("SPH").ToString))
            ''FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            'BindToDropList(ddlFgPanen, DT.Rows(0)("FgPanen").ToString)
            'BindToDropList(ddlKSUBlock, DT.Rows(0)("KSUBlock").ToString)

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindDataGrid()
            pnlUpload.Visible = False
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

    Private Sub BindDataGrid()
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM V_MsStructure " + StrFilter + " ORDER BY ID "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ID ASC"
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
            'ddlField2.SelectedValue.Replace("ClusterCode", "Block_Code")
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"

            'Session("SelectCommand") = "S_FormPrintMasterStructure " + QuotedStr(ViewState("UserId"))
            ''Session("SelectCommand") = "EXEC S_PRLandOfferingSurvey ''" + QuotedStr(GVR.Cells(2).Text) + "'', '0'," + QuotedStr(ViewState("UserId"))
            'Session("ReportFile") = ".../../../Rpt/RptPrintMasterStructure.frx"


            'V2
            Session("SelectCommand") = "S_FormPrintMasterStructureV2 " + QuotedStr(ViewState("UserId"))
            'Session("SelectCommand") = "EXEC S_PRLandOfferingSurvey ''" + QuotedStr(GVR.Cells(2).Text) + "'', '0'," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMasterStructurer-v2.1.frx"

            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindDataGrid()
    End Sub

    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            BindDataGrid()
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
                    'tbCode.Enabled = False
                    btnHome.Visible = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                    btnCancel.Visible = True
                    tbParentID.Focus()
                ElseIf DDL.SelectedValue = "Non Active" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        If GVR.Cells(8).Text = "N" Then
                            lstatus.Text = "<script language='javascript'> {alert('Structure Code closed already')}</script>"
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("UPDATE MsStructure SET FgActive = 'N' WHERE ID = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        BindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        'SQLExecuteNonQuery("DELETE MsStructure WHERE StructureCode = '" & GVR.Cells(3).Text & "' ", ViewState("DBConnection").ToString)
                        SQLExecuteNonQuery("UPDATE MsStructure SET FgActive = 'N' WHERE ID = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        BindDataGrid()
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
            pnlUpload.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("ID must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbParentID.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("ParentID must be filled.")
                tbParentID.Focus()
                Return False
            End If
            If tbStructureCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Structure Code must be filled.")
                tbStructureCode.Focus()
                Return False
            End If
            If tbStructureName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Structure Name must be filled.")
                tbStructureName.Focus()
                Return False
            End If
            If ddlLevel.Text.Trim = "" Then
                lstatus.Text = "Level Must Have Value"
                ddlLevel.Focus()
                Exit Function
            End If
            'If ddlAreal.Text.Trim.Length < 0 Then
            '    lstatus.Text = MessageDlg(" Areal must be filled.")
            '    ddlAreal.Focus()
            '    Return False
            'End Ifs
            'If ddlLandType.Text.Trim.Length < 0 Then
            '    lstatus.Text = MessageDlg("Land Type must be filled.")
            '    ddlLandType.Focus()
            '    Return False
            'End If
            'If ddlLandScape.Text.Trim.Length < 0 Then
            '    lstatus.Text = MessageDlg("Land Scape must be filled.")
            '    ddlLandScape.Focus()
            '    Return False
            'End If
            If tbLuasLahan.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Land Area must be filled.")
                tbLuasLahan.Focus()
                Return False
            End If
            If tbLuasBangunan.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Building Area must be filled.")
                tbLuasBangunan.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        'Dim SqlString As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT ID FROM MsStructure WHERE ID = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("ID Code " + QuotedStr(tbCode.Text) + " has already been exist")
                    Exit Sub
                End If


                If SQLExecuteScalar("SELECT StructureCode FROM MsStructure WHERE StructureCode = " + QuotedStr(tbStructureCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("Structure Code " + QuotedStr(tbStructureCode.Text) + " has already been exist")
                    Exit Sub
                End If


                SQLString = "INSERT INTO MsStructure (ID,ParentID,StructureCode,StructureName,LevelCode,LandArea,BuildingArea,FgActive,Account,UserId,UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbParentID.Text) + ", " + QuotedStr(tbStructureCode.Text) + ", " + QuotedStr(tbStructureName.Text) + ", " + _
                QuotedStr(ddlLevel.SelectedValue) + "," + QuotedStr(tbLuasLahan.Text) + "," + QuotedStr(tbLuasBangunan.Text) + "," + "'Y'" + "," + QuotedStr(tbAccount.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

            ElseIf ViewState("State") = "Edit" Then
                SQLString = "UPDATE MsStructure SET StructureCode = " + QuotedStr(tbStructureCode.Text) + ", ParentID = " + QuotedStr(tbParentID.Text) + _
                ", StructureName = " + QuotedStr(tbStructureName.Text) + ", LevelCode = " + QuotedStr(ddlLevel.SelectedValue) + _
                ", LandArea = " + QuotedStr(tbLuasLahan.Text) + ", BuildingArea = " + QuotedStr(tbLuasBangunan.Text) + _
                ",  Account = " + QuotedStr(tbAccount.Text) + _
                " WHERE ID = " + QuotedStr(tbCode.Text)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            BindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbCode.Focus()
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

    Private Sub DataExcel(ByVal sheet As String, ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Try
            Dim conStr As String = ""
            Select Case Extension
                Case ".xls"
                    'Excel 97-2003 
                    'conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                    conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                    Exit Select
                Case ".xlsx"
                    'Excel 2007 
                    'conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR={1}'"
                    conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                    Exit Select
            End Select

            'Get the Sheets in Excel WorkBoo 
            conStr = String.Format(conStr, FilePath, isHDR)
            Dim query As String = "SELECT * FROM [" + sheet + "]"
            Dim conn As New OleDbConnection(conStr)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim cmd As New OleDbCommand(query, conn)
            Dim da As New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            GVExcelFile.DataSource = ds.Tables(0)
            GVExcelFile.DataBind()

            'Dim count As Integer
            'count = ds.Tables(0).Columns.Count - 1
            'ddlFieldNIK.Items.Clear()
            'ddlFieldNIK.Items.Add(New ListItem("--Choose One--", ""))
            'ddlFieldDate.Items.Clear()
            'ddlFieldDate.Items.Add(New ListItem("--Choose One--", ""))
            'ddlFieldTime.Items.Clear()
            'ddlFieldTime.Items.Add(New ListItem("--Choose One--", ""))
            'For j = 0 To count
            '    ddlFieldNIK.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
            '    ddlFieldDate.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
            '    ddlFieldTime.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
            'Next
            da.Dispose()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            lstatus.Text = "DataExcel Error : " + ex.ToString
        End Try
    End Sub

    Private Sub GetExcelSheets(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-2003 
                'conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                Exit Select
            Case ".xlsx"
                'Excel 2007 
                'conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR={1}'"
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select

        'Get the Sheets in Excel WorkBoo 
        conStr = String.Format(conStr, FilePath, isHDR)
        'lstatus.Text = conStr
        'Exit Sub
        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()

        cmdExcel.Connection = connExcel
        'If connExcel.State = ConnectionState.Closed Then
        connExcel.Open()
        'End If
        'Bind the Sheets to DropDownList 
        ddlSheets.Items.Clear()
        'ddlSheets.Items.Add(New ListItem("--Select Sheet--", ""))
        ddlSheets.DataSource = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        ddlSheets.DataTextField = "TABLE_NAME"
        ddlSheets.DataValueField = "TABLE_NAME"
        ddlSheets.DataBind()
        connExcel.Close()

        ddlSheets.SelectedIndex = 0
        DataExcel(ddlSheets.SelectedValue, FilePath, Extension, isHDR)
    End Sub

    Protected Sub DeleteTemp()

        Try

            'lstatus.Text = ViewState("UserId").ToString
        Catch ex As Exception

        End Try

    End Sub


    Protected Sub InsertMaster()
        Try
           

        Catch ex As Exception

        End Try
    End Sub



    Protected Sub ExecuteImportDB()
        Dim GVR As GridViewRow
        Dim strSQL As String
        Dim sqlstring, msgerror As String
        Try

            'If ddlSheets.SelectedValue = "" Then
            '    lstatus.Text = MessageDlg("Gagal")
            '    Exit Sub
            'End If

            SQLExecuteNonQuery("DELETE MsStructureTemp WHERE UserID = " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection"))

            For Each GVR In GVExcelFile.Rows

                'SQLExecuteNonQuery("EXEC S_GLMsStructure " + QuotedStr(GVR.Cells(0).Text) + ", " + QuotedStr(GVR.Cells(1).Text) + ", " + _
                'QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(GVR.Cells(3).Text) + "," + QuotedStr(GVR.Cells(4).Text) + ", " + QuotedStr(GVR.Cells(5).Text) + "," + _
                'QuotedStr(GVR.Cells(6).Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(GVR.Cells(10).Text), ViewState("DBConnection"))


                SQLExecuteNonQuery("INSERT INTO MsStructureTemp (ID,ParentID,StructureCode,StructureName,LevelCode,LandArea,BuildingArea,FgActive,UserID,UserDate, Account) " + _
                              " SELECT " + _
                              QuotedStr(GVR.Cells(0).Text) + ", " + QuotedStr(GVR.Cells(1).Text) + ", " + _
                              QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(GVR.Cells(3).Text) + "," + QuotedStr(GVR.Cells(4).Text) + ", " + QuotedStr(GVR.Cells(5).Text) + "," + _
                              QuotedStr(GVR.Cells(6).Text) + ",'Y'," + QuotedStr(ViewState("UserId").ToString) + ",getDate(), " + QuotedStr(GVR.Cells(10).Text), ViewState("DBConnection"))

            Next

            sqlstring = "Declare @A VarChar(255) EXEC S_GLMsStructureCek " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
            msgerror = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            'lstatus.Text = sqlstring
            If msgerror.Length > 2 Then
                lstatus.Text = MessageDlg(msgerror)
                'lstatus.Text = MessageDlg("Another ID or Structure Already exist!!")
                Exit Sub
            End If

            'ddlSheets.Items.Clear()
            lstatus.Text = MessageDlg("Records inserted successfully")
            btnSearch_Click(Nothing, Nothing)
        Catch ex As Exception
            lstatus.Text = "Import to database Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataImportDB()
        'Dim Dt As DataTable
        'Dim DV As DataView
        'Try
        '    'Dt = SQLExecuteQuery("EXEC S_PYAbsImportData '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
        '    'If Dt.Rows.Count = 0 Then
        '    '    lstatus.Text = "No Data"
        '    'End If
        '    DV = Dt.DefaultView
        '    If ViewState("SortImportDb") = Nothing Then
        '        ViewState("SortImportDb") = "ID ASC"
        '    End If
        '    If DV.Count = 0 Then
        '        'Dim DT2 As DataTable = Dt
        '        'ShowGridViewIfEmpty(DT2, DataGrid)
        '    Else
        '        DV.Sort = ViewState("SortImportDb")
        '        DataGrid.DataSource = DV
        '        DataGrid.DataBind()
        '    End If
        'Catch ex As Exception
        '    'lstatus.Text = "BindDataImportDb Error : " + ex.ToString
        'End Try
    End Sub

    Private Sub Import_To_Grid(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-2003 
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                Exit Select
            Case ".xlsx"
                'Excel 2007 
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select
        conStr = String.Format(conStr, FilePath, isHDR)

        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        Dim dt As New DataTable()

        cmdExcel.Connection = connExcel
        'Get the name of First Sheet 
        connExcel.Open()
        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()
        'Read Data from First Sheet 
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * FROM [" & SheetName & "]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)
        connExcel.Close()
        'Bind Data to GridView 
        GVExcelFile.Caption = Path.GetFileName(FilePath)
        GVExcelFile.DataSource = dt
        GVExcelFile.DataBind()
    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            If fuLocation.HasFile Then
                Dim FileName As String = Path.GetFileName(fuLocation.PostedFile.FileName)
                Dim Extension As String = Path.GetExtension(fuLocation.PostedFile.FileName)
                Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

                Dim FilePath As String = Server.MapPath("~/ImportExcel/" + FileName)
                fuLocation.SaveAs(FilePath)
                ViewState("FilePath") = FilePath
                ViewState("Extension") = Extension
                GetExcelSheets(FilePath, Extension, "Yes")
            End If
        Catch ex As Exception
            lstatus.Text = "btnUpload_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GVExcelFile_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVExcelFile.PageIndexChanging
        Try
            GVExcelFile.PageIndex = e.NewPageIndex
            DataExcel(ddlSheets.SelectedValue, ViewState("FilePath"), ViewState("Extension"), "YES")
        Catch ex As Exception
            lstatus.Text = "GVExcelFile_PageIndexChanging Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub MoveView(ByVal page As Integer)
        pnlUpload.Visible = False
        pnlHd.Visible = True
      

        If page = 0 Then
            PnlHd.Visible = True
        ElseIf page = 1 Then
            pnlUpload.Visible = True
        End If
    End Sub

    Protected Sub ddlSheets_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSheets.SelectedIndexChanged
        Try
            DataExcel(ddlSheets.SelectedValue, ViewState("FilePath"), ViewState("Extension"), "Yes")

            MoveView(1)
        Catch ex As Exception
            lstatus.Text = "ddlSheets_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnImportDB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportDB.Click
        Try
            'MoveView(6)
            'DeleteTemp()
            ExecuteImportDB()
            'InsertMaster()
            'pnlUpload.Visible = False
            'BindDataImportDB()
            'lstatus.Text = MessageDlg("Records inserted successfully")
            'btnSearch_Click(Nothing, Nothing)
        Catch ex As Exception
            lstatus.Text = "btnImportDb_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccount.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Account, Description  from V_MsAccount"
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccount"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub
End Class
