Imports System.Data
Partial Class Tugas
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing

            'FillCombo(ddlCity, "SELECT CityCode, CityName FROM MsCity", True, "CityCode", "CityName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            'tbKepadatan.Attributes.Add("OnKeyDown", "return PressNumeric();")

        End If
        If Not Session("Result") Is Nothing Then

            'If ViewState("Sender") = "btnAccExpense" Then
            '    tbAccExpense.Text = Session("Result")(0).ToString
            '    tbAccExpenseName.Text = Session("Result")(1).ToString
            'End If

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
            tbPasal1.Text = ""
            tbpoint1.Text = ""

            tbPasal2.Text = ""
            tbPoint2.Text = ""

            tbPasal3.Text = ""
            tbPoint3.Text = ""

            tbPasal4.Text = ""
            tbPoint4.Text = ""

            tbPasal5.Text = ""
            tbPoint5.Text = ""

            tbPasal6.Text = ""
            tbpoint6.Text = ""

            tbPasal7.Text = ""
            tbPoint7.Text = ""

            tbPasal8.Text = ""
            tbPoint8.Text = ""

            tbPasal9.Text = ""
            tbPoint9.Text = ""

            tbPasal10.Text = ""
            tbPoint10.Text = ""

            tbNumber1.Text = ""
            tbNumber2.Text = ""
            tbNumber3.Text = ""
            tbNumber4.Text = ""
            tbNumber5.Text = ""
            tbNumber6.Text = ""
            tbNumber7.Text = ""
            tbNumber8.Text = ""
            tbNumber9.Text = ""
            tbNumber10.Text = ""

            
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal TypeSPKCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsTypeSPK WHERE TypeSPKCode = " + QuotedStr(TypeSPKCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("TypeSPKCode").ToString)
            BindToText(tbName, DT.Rows(0)("TypeSPKName").ToString)

            BindToText(tbPasal1, DT.Rows(0)("Pasal_1").ToString)
            BindToText(tbpoint1, DT.Rows(0)("Point_1").ToString)

            BindToText(tbPasal2, DT.Rows(0)("Pasal_2").ToString)
            BindToText(tbPoint2, DT.Rows(0)("Point_2").ToString)

            BindToText(tbPasal3, DT.Rows(0)("Pasal_3").ToString)
            BindToText(tbPoint3, DT.Rows(0)("Point_3").ToString)

            BindToText(tbPasal4, DT.Rows(0)("Pasal_4").ToString)
            BindToText(tbPoint4, DT.Rows(0)("Point_4").ToString)

            BindToText(tbPasal5, DT.Rows(0)("Pasal_5").ToString)
            BindToText(tbPoint5, DT.Rows(0)("Point_5").ToString)

            BindToText(tbPasal6, DT.Rows(0)("Pasal_6").ToString)
            BindToText(tbpoint6, DT.Rows(0)("Point_6").ToString)

            BindToText(tbPasal7, DT.Rows(0)("Pasal_7").ToString)
            BindToText(tbPoint7, DT.Rows(0)("Point_7").ToString)

            BindToText(tbPasal8, DT.Rows(0)("Pasal_8").ToString)
            BindToText(tbPoint8, DT.Rows(0)("Point_8").ToString)

            BindToText(tbPasal9, DT.Rows(0)("Pasal_9").ToString)
            BindToText(tbPoint9, DT.Rows(0)("Point_9").ToString)

            BindToText(tbPasal10, DT.Rows(0)("Pasal_10").ToString)
            BindToText(tbPoint10, DT.Rows(0)("Point_10").ToString)
            BindToText(tbNumber1, DT.Rows(0)("Number1").ToString)
            BindToText(tbNumber2, DT.Rows(0)("Number2").ToString)
            BindToText(tbNumber3, DT.Rows(0)("Number3").ToString)
            BindToText(tbNumber4, DT.Rows(0)("Number4").ToString)
            BindToText(tbNumber5, DT.Rows(0)("Number5").ToString)
            BindToText(tbNumber6, DT.Rows(0)("Number6").ToString)
            BindToText(tbNumber7, DT.Rows(0)("Number7").ToString)
            BindToText(tbNumber8, DT.Rows(0)("Number8").ToString)
            BindToText(tbNumber9, DT.Rows(0)("Number9").ToString)
            BindToText(tbNumber10, DT.Rows(0)("Number10").ToString)
            BindToText(tbRemarkRincian, DT.Rows(0)("RemarkRincianBiaya").ToString)
           
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
            SqlString = "SELECT * From V_MsTypeSPK" + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TypeSPKCode DESC"
                ViewState("SortOrder") = "DESC"
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

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster6 'V_MsNotaris','TypeSPKCode','TypeSPKName','Gender','TypeID','NotID','Address1+'', ''+Address2+'', ''+Desa +'', ''+ Kec+'', ''+ Kab +'', ''+ City +'', Pos: ''+ ZipCode+'', Telp : ''+ Phone','Notaris File','Code','Description','Gender','TypeID','NotarisID','Alamat'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
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
                    'ElseIf DDL.SelectedValue = "Non Active" Then
                    '    Try
                    '        If CheckMenuLevel("Delete") = False Then
                    '            Exit Sub
                    '        End If
                    '        If GVR.Cells(8).Text = "N" Then
                    '            lstatus.Text = "<script language='javascript'> {alert('Job Plantation closed already')}</script>"
                    '            Exit Sub
                    '        End If
                    '        SQLExecuteNonQuery("UPDATE MsTypeSPKSET Fgactive = 'N' WHERE JobCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                    '        bindDataGrid()
                    '    Catch ex As Exception
                    '        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    '    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("DELETE MsTypeSPK WHERE TypeSPKCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
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
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, _
           ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        'Make the selected menu item reflect the correct imageurl

        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
                'Menu1.Items(i).ImageUrl = "selectedtab.gif"
            Else
                ' Menu1.Items(i).ImageUrl = "unselectedtab.gif"
            End If
        Next
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Job Code must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Job Name must be filled.")
                tbName.Focus()
                Return False
            End If



            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString, Code, ID As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                Code = SQLExecuteScalar("SELECT TypeSPKCode FROM V_MsTypeSPK WHERE TypeSPKCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If Code = tbCode.Text Then
                    lstatus.Text = MessageDlg("Pemberi Tugas Code " + QuotedStr(tbCode.Text) + " has already been exist")
                    Exit Sub
                End If

                'ID = SQLExecuteScalar("SELECT TypeID+'|'+NotID FROM V_MsTypeSPK WHERE TypeID = " + QuotedStr(ddlTypeID.SelectedValue) + " AND NotID = " + QuotedStr(tbNotarisID.Text), ViewState("DBConnection").ToString)
                'If ID = ddlTypeID.SelectedValue + "|" + tbNotarisID.Text Then
                '    lstatus.Text = MessageDlg("Notaris With ID " + QuotedStr(tbNotarisID.Text) + " has already been exist")
                '    Exit Sub
                'End If

                SqlString = "INSERT INTO MsTypeSPK(TypeSPKCode, TypeSPKName,Pasal_1,Point_1, Pasal_2,Point_2, Pasal_3,Point_3, Pasal_4,Point_4, Pasal_5,Point_5, Pasal_6,Point_6, Pasal_7,Point_7, Pasal_8,Point_8, Pasal_9,Point_9, Pasal_10,Point_10 ," + _
                "Number1, Number2, Number3, Number4, Number5,Number6,Number7,Number8,Number9,Number10,RemarkRincianBiaya,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(tbPasal1.Text) + ", " + QuotedStr(tbpoint1.Text) + "," + _
                QuotedStr(tbPasal2.Text) + ", " + QuotedStr(tbPoint2.Text) + "," + _
                QuotedStr(tbPasal3.Text) + ", " + QuotedStr(tbPoint3.Text) + "," + _
                QuotedStr(tbPasal4.Text) + ", " + QuotedStr(tbPoint4.Text) + "," + _
                QuotedStr(tbPasal5.Text) + ", " + QuotedStr(tbPoint5.Text) + "," + _
                QuotedStr(tbPasal6.Text) + ", " + QuotedStr(tbpoint6.Text) + "," + _
                QuotedStr(tbPasal7.Text) + ", " + QuotedStr(tbPoint7.Text) + "," + _
                QuotedStr(tbPasal8.Text) + ", " + QuotedStr(tbPoint8.Text) + "," + _
                QuotedStr(tbPasal9.Text) + ", " + QuotedStr(tbPoint9.Text) + "," + _
                QuotedStr(tbPasal10.Text) + ", " + QuotedStr(tbPoint10.Text) + "," + _
                QuotedStr(tbNumber1.Text) + ", " + QuotedStr(tbNumber2.Text) + "," + _
                QuotedStr(tbNumber3.Text) + ", " + QuotedStr(tbNumber4.Text) + "," + _
                QuotedStr(tbNumber5.Text) + "," + QuotedStr(tbNumber6.Text) + "," + _
                QuotedStr(tbNumber7.Text) + "," + QuotedStr(tbNumber8.Text) + "," + _
                QuotedStr(tbNumber9.Text) + "," + QuotedStr(tbNumber10.Text) + "," + _
                QuotedStr(tbRemarkRincian.Text) + "," + _
               QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "UPDATE MsTypeSPK SET TypeSPKName = " + QuotedStr(tbName.Text) & _
                            ", Pasal_1 = " + QuotedStr(tbPasal1.Text) + ", Point_1 = " + QuotedStr(tbpoint1.Text) & _
                            ", Pasal_2 = " + QuotedStr(tbPasal2.Text) + ", Point_2 = " + QuotedStr(tbPoint2.Text) & _
                            ", Pasal_3 = " + QuotedStr(tbPasal3.Text) + ", Point_3 = " + QuotedStr(tbPoint3.Text) & _
                            ", Pasal_4 = " + QuotedStr(tbPasal4.Text) + ", Point_4 = " + QuotedStr(tbPoint4.Text) & _
                            ", Pasal_5 = " + QuotedStr(tbPasal5.Text) + ", Point_5 = " + QuotedStr(tbPoint5.Text) & _
                            ", Pasal_6 = " + QuotedStr(tbPasal6.Text) + ", Point_6 = " + QuotedStr(tbpoint6.Text) & _
                            ", Pasal_7 = " + QuotedStr(tbPasal7.Text) + ", Point_7 = " + QuotedStr(tbPoint7.Text) & _
                            ", Pasal_8 = " + QuotedStr(tbPasal8.Text) + ", Point_8 = " + QuotedStr(tbPoint8.Text) & _
                            ", Pasal_9 = " + QuotedStr(tbPasal9.Text) + ", Point_9 = " + QuotedStr(tbPoint9.Text) & _
                            ", Pasal_10 = " + QuotedStr(tbPasal10.Text) + ", Point_10 = " + QuotedStr(tbPoint10.Text) & _
                            ", Number1 = " + QuotedStr(tbNumber1.Text) + ", Number2 = " + QuotedStr(tbNumber2.Text) & _
                            ", Number3 = " + QuotedStr(tbNumber3.Text) + ", Number4 = " + QuotedStr(tbNumber4.Text) & _
                            ", Number5 = " + QuotedStr(tbNumber5.Text) + ", Number6 = " + QuotedStr(tbNumber6.Text) & _
                            ", Number7 = " + QuotedStr(tbNumber7.Text) + ", Number8 = " + QuotedStr(tbNumber8.Text) & _
                            ", Number9 = " + QuotedStr(tbNumber9.Text) + ", Number10 = " + QuotedStr(tbNumber10.Text) & _
                            ", RemarkRincianBiaya = " + QuotedStr(tbRemarkRincian.Text) & _
                            " WHERE TypeSPKCode = " + QuotedStr(tbCode.Text)
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


    'Protected Sub btnAccAsset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAsset.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "Select Account, Description from V_MsAccount WHERE FgActive = 'Y' "
    '        ResultField = "Account, Description"
    '        ViewState("Sender") = "btnAccAsset"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lstatus.Text = "btn Acc Asset Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Home Error : " + ex.ToString
        End Try
    End Sub
End Class
