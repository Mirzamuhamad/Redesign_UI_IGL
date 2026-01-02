Imports System.Data

Partial Class Master_MsAccount_MsAccount
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            FillCombo(ddlSubled, "SELECT * FROM V_MsFgSubLed", False, "FgSubLed", "FgSubLedName", ViewState("DBConnection"))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            GridView1.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"

        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnAccClass" Then
                tbAccClass.Text = Session("Result")(0).ToString
                tbAccClassName.Text = Session("Result")(1).ToString
                tbCode.Text = tbAccClass.Text + tbDetail.Text
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            '  Session("Criteria") = Nothing
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

            If CommandName = "Insert" Then
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
            SqlString = "SELECT * FROM V_MsAccountView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Account ASC"
            End If
            BindDataMaster(SqlString, GridView1, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim paramgo, SqlString As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlMain, pnlInput)
                    FillTextBox(GVR.Cells(1).Text)
                    ViewState("State") = "View"
                    ModifyInput(False, pnlInput)
                    tbCode.Enabled = False
                    btnHome.Visible = True
                    btnSave.Visible = False
                    btnReset.Visible = False
                    btnCancel.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    tbCode.Enabled = False
                    ModifyInput(True, pnlInput)
                    MovePanel(PnlMain, pnlInput)
                    FillTextBox(GVR.Cells(1).Text)
                    tbAccClass.Enabled = False
                    tbDetail.Enabled = False
                    btnAccClass.Visible = False
                    ddlFgActive.Enabled = ddlFgActive.SelectedValue = "N"
                    tbName.Focus()
                    ViewState("State") = "Edit"
                    btnHome.Visible = False
                    btnSave.Visible = True
                    btnReset.Visible = True
                    btnCancel.Visible = True
                    tbName.Focus()
                ElseIf DDL.SelectedValue = "Non Active" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        If GVR.Cells(7).Text = "N" Then
                            lstatus.Text = "<script language='javascript'> {alert('Account closed already')}</script>"
                            Exit Sub
                        End If
                        SqlString = "Update MsAccount Set FgActive = 'N', UserClose = " + QuotedStr(ViewState("UserId").ToString) + ", CloseDate = getDate() " + _
                        "Where Account = " + QuotedStr(GVR.Cells(1).Text.ToString)
                        SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        SqlString = "DELETE MsAccount Where Account = " + QuotedStr(GVR.Cells(1).Text.ToString)
                        SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Trans Type" Then
                    paramgo = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Account',  '" + Request.QueryString("KeyId") + "','" + paramgo + "','AssMsAccountTrans');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCommand Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_GLRptMsAccount 0,'All'" + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptCOA.frx"
            'lstatus.Text = Session("SelectCommand")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnHome.Click
        Try
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal Account As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsAccountView A WHERE Account = " + QuotedStr(Account)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("Account").ToString)
            BindToText(tbName, DT.Rows(0)("AccountName").ToString)
            BindToText(tbAccClass, DT.Rows(0)("AccClass").ToString)
            BindToText(tbAccClassName, DT.Rows(0)("AccClassName").ToString)
            BindToText(tbDetail, DT.Rows(0)("Detail").ToString)
            BindToDropList(ddlCurr, DT.Rows(0)("CurrCode").ToString)
            BindToDropList(ddlSubled, DT.Rows(0)("FgSubLed").ToString)
            BindToDropList(ddlNormal, DT.Rows(0)("FgNormal").ToString)
            BindToDropList(ddlFgActive, DT.Rows(0)("FgActive").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If
            tbName.Text = ""
            If Not ViewState("State") = "Edit" Then
                tbAccClass.Text = ""
                tbAccClassName.Text = ""
                tbDetail.Text = ""
            End If
            ddlCurr.SelectedIndex = 0
            ddlSubled.SelectedIndex = 0
            ddlNormal.SelectedIndex = 0
        Catch ex As Exception
            lstatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Try
            'If tbCode.Text.Trim = "" Then
            '    lstatus.Text = "Code Must Have Value"
            '    tbCode.Focus()
            '    Exit Sub
            'End If
            If tbName.Text.Trim = "" Then
                lstatus.Text = "Name Must Have Value"
                tbName.Focus()
                Exit Sub
            End If
            If tbAccClassName.Text.Trim = "" Then
                lstatus.Text = "Account Class Must Have Value"
                tbAccClass.Focus()
                Exit Sub
            End If

            If tbDetail.Text.Trim = "" Then
                lstatus.Text = "Detail Must Have Value"
                tbName.Focus()
                Exit Sub
            End If



            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT Account From VMsAccount WHERE Account = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Account Class " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If


                SQLString = "Insert into MsAccount (Account, AccountName, AccClass, Detail, CurrCode, FgSubled, FgNormal, FgActive, UserId, UserDate)" + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " + QuotedStr(tbAccClass.Text) + ", " + _
                QuotedStr(tbDetail.Text) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + QuotedStr(ddlSubled.SelectedValue) + "," + _
                QuotedStr(ddlNormal.SelectedValue) + ", 'Y', " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                'lstatus.Text = SQLString
                'Exit Sub
            ElseIf ViewState("State") = "Edit" Then
                SQLString = "Update MsAccount set AccountName= " + QuotedStr(tbName.Text) + _
                ", AccClass = " + QuotedStr(tbAccClass.Text) + ", Detail= " + QuotedStr(tbDetail.Text) + _
                ", CurrCode = " + QuotedStr(ddlCurr.SelectedValue) + ", FgSubled = " + QuotedStr(ddlSubled.SelectedValue) + _
                ", FgNormal = " + QuotedStr(ddlNormal.SelectedValue) + _
                ", UserDate = getDate() " + _
                ", UserId = " + QuotedStr(ViewState("UserId").ToString) + _
                " where Account = " & QuotedStr(tbCode.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If CheckMenuLevel("Insert") = False Then
                Exit Sub
            End If
            ViewState("State") = "Insert"
            PnlMain.Visible = False
            pnlInput.Visible = True
            'tbCode.Enabled = True
            tbCode.Enabled = False
            tbCode.Text = ""
            tbName.Text = ""
            tbAccClass.Text = ""
            tbAccClassName.Text = ""
            tbDetail.Text = ""
            ddlFgActive.SelectedValue = "Y"
            ddlFgActive.Enabled = False
            'ddlCurr.SelectedIndex = 0
            ddlSubled.SelectedIndex = 0
            ddlNormal.SelectedIndex = 0
            tbName.Focus()
            ModifyInput(True, pnlInput)
            tbAccClass.Enabled = True
            btnAccClass.Visible = True
            tbDetail.Enabled = True
            btnSave.Visible = True
            btnCancel.Visible = True
            btnReset.Visible = True
            btnHome.Visible = False


        Catch ex As Exception
            lstatus.Text = "btn add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd2.Click
        btnAdd_Click(sender, e)
    End Sub


    Protected Sub btnAccClass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccClass.Click
        Dim FieldResult As String
        
        Try
            Session("filter") = "Select Acc_Class, Acc_Class_Name FROM V_MsAccClass"
            FieldResult = "Acc_Class, Acc_Class_Name"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccClass"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If

        Catch ex As Exception
            lstatus.Text = "btn AccClass Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccClass_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccClass.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select AccClassCode, AccClassName From MsAccClass WHERE AccClassCode = " + QuotedStr(tbAccClass.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccClass.Text = ""
                tbAccClassName.Text = ""
                tbCode.Text = ""
                'tbDetail.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccClass.Text = dr("AccClassCode").ToString
                tbAccClassName.Text = dr("AccClassName").ToString
                tbCode.Text = tbAccClass.Text + tbDetail.Text
            End If


        Catch ex As Exception
            lstatus.Text = "tb Acc Class Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            GridView1.EditIndex = -1
            btnAdd.Visible = True
            btnAdd2.Visible = True
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbDetail_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDetail.TextChanged
        Try
            tbCode.Text = tbAccClass.Text + tbDetail.Text
        Catch ex As Exception
            lstatus.Text = "tbDetail_TextChanged Error : " + vbCrLf + ex.ToString
        End Try

    End Sub
End Class
