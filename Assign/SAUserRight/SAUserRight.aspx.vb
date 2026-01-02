Imports System.Data
Partial Class Assign_SAUserRight_SAUserRight
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                'DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                pnlHd.Visible = False
                ActionVisible(False)
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSearch" Then
                    pnlHd.Visible = False
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                    If TreeView1.Nodes.Count > 0 Then
                        TreeView1.Nodes.Clear()
                    End If
                    ActionVisible(True)
                    PopulateRootLevel()
                    TreeView1.ExpandAll()
                    PopulateData()
                    pnlHd.Visible = True
                    pnlCopy.Visible = False
                    pnlDt.Visible = False
                    ddlCommand.Enabled = True
                    btnGo.Enabled = True
                    btnSave.Enabled = True
                ElseIf ViewState("Sender") = "btnCopy" Then
                    tbCopyCode.Text = Session("Result")(0).ToString
                    tbCopyName.Text = Session("Result")(1).ToString
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If            
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
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

    Protected Sub ActionVisible(ByVal bool As Boolean)
        btnAction.Visible = bool
        lbAction.Visible = bool
        ddlAction.Visible = bool
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim ResultField As String
        Try
            ddlCommand.Enabled = False
            btnGo.Enabled = False
            btnSave.Enabled = False
            Session("filter") = "select User_Id, User_Name From VSAUsers WHERE FgAdmin <> 'Y'"
            ResultField = "User_Id, User_Name"
            ViewState("Sender") = "btnSearch"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Private Sub PopulateRootLevel()
        Dim SQLString As String
        Dim Dt As Datatable
        Try
            SQLString = "SELECT A.MenuId, A.MenuParent, A.MenuName, (SELECT COUNT(*) FROM VMsMenu WHERE MenuParent = A.MenuId) AS ChildNode  FROM VMsMenu A WHERE MenuLevel = 0 ORDER BY OrderBy"
            'SQLString = "SELECT A.MenuId, A.MenuParent, A.MenuName, (SELECT COUNT(*) FROM VSAUserRight WHERE MenuParent = A.MenuId) AS ChildNode  FROM VSAUserRight A WHERE MenuLevel = 0  AND UserId = 'Acc1' ORDER BY OrderBy"
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

            PopulateNodes(Dt, TreeView1.Nodes)
        Catch ex As Exception
            Throw New Exception("Populate Root Level Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub PopulateNodes(ByVal dt As DataTable, ByVal nodes As TreeNodeCollection)
        Try
            For Each dr As DataRow In dt.Rows
                Dim tn As New TreeNode()
                tn.Text = dr("MenuName").ToString()
                tn.Value = dr("MenuId").ToString()
                tn.SelectAction = TreeNodeSelectAction.Expand
                nodes.Add(tn)
                tn.PopulateOnDemand = (CInt(dr("ChildNode")) > 0)
            Next
        Catch ex As Exception
            Throw New Exception("Populate Nodes Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub PopulateSubLevel(ByVal parentid As String, ByVal parentNode As TreeNode)
        Dim SQLString As String
        Dim Dt As DataTable
        Try
            SQLString = "SELECT MenuId, MenuParent, MenuName, (SELECT COUNT(*) FROM VMsMenu WHERE MenuParent = A.MenuId) AS ChildNode  FROM VMsMenu A WHERE MenuParent = " + QuotedStr(parentid) + " ORDER BY OrderBy"
            'SQLString = "SELECT MenuId, MenuParent, MenuName, (SELECT COUNT(*) FROM VSAUserRight WHERE MenuParent = A.MenuId) AS ChildNode  FROM VSAUserRight A WHERE MenuParent = " + QuotedStr(parentid) + " AND UserId = 'Acc1' ORDER BY OrderBy"
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            PopulateNodes(Dt, parentNode.ChildNodes)
        Catch ex As Exception
            Throw New Exception("Populate Sub Level Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub TreeView1_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles TreeView1.TreeNodePopulate
        Try
            PopulateSubLevel(e.Node.Value, e.Node)
        Catch ex As Exception
            lbStatus.Text = "Tree Node Populate Error : " + ex.ToString
        End Try
    End Sub

    Private Sub PopulateData()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim tn As TreeNode
        Dim ctrl As New Control
        Dim ContainerId As String
        Try
            dt = SQLExecuteQuery("SELECT MenuId, dbo.GetPathMenu(MenuId) AS Path FROM VSAUserRight WHERE UserId = " + QuotedStr(tbCode.Text), ViewState("DBConnection")).Tables(0)
            For Each dr In dt.Rows
                ContainerId = dr("Path").ToString
                tn = TreeView1.FindNode(dr("Path").ToString)
                tn.Checked = True
            Next
        Catch ex As Exception
            Throw New Exception("Populate Data Error : " + ex.ToString)
        End Try        
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim tn As TreeNode
        Try
            SQLExecuteNonQuery("EXEC S_SAUserRightDelete " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            TreeView1.ExpandAll()
            For Each tn In TreeView1.CheckedNodes
                SQLExecuteNonQuery("EXEC S_SAUserRightAdd " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tn.Value) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
            Next
            lbStatus.Text = MessageDlg("Data Updated")
        Catch ex As Exception
            lbStatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BindDataDt()
        Dim Dt As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_SAUserLevel " + QuotedStr(tbCode.Text), ViewState("DBConnection")).Tables(0)

            'If Dt.Rows.Count = 0 Then
            '    lbStatus.Text = "No Data"
            'End If
            DV = Dt.DefaultView
            'If ViewState("SortOrder") = Nothing Then
            'ViewState("SortOrder") = "EmpNumb ASC"
            'End If
            If DV.Count = 0 Then
                Dim DT2 As DataTable = Dt
                ShowGridViewIfEmpty(DT2, DataGrid)
            Else
                'DV.Sort = ViewState("SortImport")
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindDataDt()
    End Sub

    
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim GVR As GridViewRow
        Dim ddlInsert, ddlEdit, ddlDelete, ddlGetAppr, ddlPost, ddlUnpost, ddlPrint, ddlComplete, ddlClosing, ddlCancel As DropDownList
        Dim lbInsert, lbEdit, lbDelete, lbGetAppr, lbPost, lbUnpost, lbPrint, lbComplete, lbClosing, lbCancel As Label
        Try
            GVR = DataGrid.Rows(e.NewEditIndex)
            lbInsert = GVR.FindControl("FgInsert")
            lbEdit = GVR.FindControl("FgEdit")
            lbDelete = GVR.FindControl("FgDelete")
            lbGetAppr = GVR.FindControl("FgGetAppr")
            lbPost = GVR.FindControl("FgPost")
            lbUnpost = GVR.FindControl("FgUnPost")
            lbPrint = GVR.FindControl("FgPrint")
            lbComplete = GVR.FindControl("FgComplete")
            lbClosing = GVR.FindControl("FgClosing")
            lbCancel = GVR.FindControl("FgCancel")

            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            BindDataDt()

            GVR = DataGrid.Rows(e.NewEditIndex)

            ddlInsert = GVR.FindControl("FgInsertEdit")
            ddlEdit = GVR.FindControl("FgEditEdit")
            ddlDelete = GVR.FindControl("FgDeleteEdit")
            ddlGetAppr = GVR.FindControl("FgGetApprEdit")
            ddlPost = GVR.FindControl("FgPostEdit")
            ddlUnpost = GVR.FindControl("FgUnPostEdit")
            ddlPrint = GVR.FindControl("FgPrintEdit")
            ddlComplete = GVR.FindControl("FgCompleteEdit")
            ddlClosing = GVR.FindControl("FgClosingEdit")
            ddlCancel = GVR.FindControl("FgCancelEdit")

            If lbInsert.Text = "X" Then
                ddlInsert.SelectedValue = "N"
                ddlInsert.Enabled = False
            Else
                ddlInsert.SelectedValue = lbInsert.Text
                ddlInsert.Enabled = True
            End If

            If lbEdit.Text = "X" Then
                ddlEdit.SelectedValue = "N"
                ddlEdit.Enabled = False
            Else
                ddlEdit.SelectedValue = lbEdit.Text
                ddlEdit.Enabled = True
            End If

            If lbDelete.Text = "X" Then
                ddlDelete.SelectedValue = "N"
                ddlDelete.Enabled = False
            Else
                ddlDelete.SelectedValue = lbDelete.Text
                ddlDelete.Enabled = True
            End If

            If lbGetAppr.Text = "X" Then
                ddlGetAppr.SelectedValue = "N"
                ddlGetAppr.Enabled = False
            Else
                ddlGetAppr.SelectedValue = lbGetAppr.Text
                ddlGetAppr.Enabled = True
            End If

            If lbPost.Text = "X" Then
                ddlPost.SelectedValue = "N"
                ddlPost.Enabled = False
            Else
                ddlPost.SelectedValue = lbPost.Text
                ddlPost.Enabled = True
            End If

            If lbUnpost.Text = "X" Then
                ddlUnpost.SelectedValue = "N"
                ddlUnpost.Enabled = False
            Else
                ddlUnpost.SelectedValue = lbUnpost.Text
                ddlUnpost.Enabled = True
            End If

            If lbPrint.Text = "X" Then
                ddlPrint.SelectedValue = "N"
                ddlPrint.Enabled = False
            Else
                ddlPrint.SelectedValue = lbPrint.Text
                ddlPrint.Enabled = True
            End If

            If lbComplete.Text = "X" Then
                ddlComplete.SelectedValue = "N"
                ddlComplete.Enabled = False
            Else
                ddlComplete.SelectedValue = lbComplete.Text
                ddlComplete.Enabled = True
            End If

            If lbClosing.Text = "X" Then
                ddlClosing.SelectedValue = "N"
                ddlClosing.Enabled = False
            Else
                ddlClosing.SelectedValue = lbClosing.Text
                ddlClosing.Enabled = True
            End If

            If lbCancel.Text = "X" Then
                ddlCancel.SelectedValue = "N"
                ddlCancel.Enabled = False
            Else
                ddlCancel.SelectedValue = lbCancel.Text
                ddlCancel.Enabled = True
            End If

        Catch ex As Exception
            lbStatus.Text = "data grid row editing error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.EditIndex = -1
            BindDataDt()
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString, UserLevel, Result As String
        Dim lbCode As Label
        Dim ddlInsert, ddlEdit, ddlDelete, ddlGetAppr, ddlPost, ddlUnpost, ddlPrint, ddlComplete, ddlClosing, ddlCancel As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("ContainerIdEdit")
            ddlInsert = DataGrid.Rows(e.RowIndex).FindControl("FgInsertEdit")
            ddlEdit = DataGrid.Rows(e.RowIndex).FindControl("FgEditEdit")
            ddlDelete = DataGrid.Rows(e.RowIndex).FindControl("FgDeleteEdit")
            ddlGetAppr = DataGrid.Rows(e.RowIndex).FindControl("FgGetApprEdit")
            ddlPost = DataGrid.Rows(e.RowIndex).FindControl("FgPostEdit")
            ddlUnpost = DataGrid.Rows(e.RowIndex).FindControl("FgUnPostEdit")
            ddlPrint = DataGrid.Rows(e.RowIndex).FindControl("FgPrintEdit")
            ddlComplete = DataGrid.Rows(e.RowIndex).FindControl("FgCompleteEdit")
            ddlClosing = DataGrid.Rows(e.RowIndex).FindControl("FgClosingEdit")
            ddlCancel = DataGrid.Rows(e.RowIndex).FindControl("FgCancelEdit")
            UserLevel = ""

            If ddlInsert.Enabled Then
                UserLevel = ddlInsert.SelectedValue
            Else
                UserLevel = "X"
            End If

            If ddlEdit.Enabled Then
                UserLevel = UserLevel + ddlEdit.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlDelete.Enabled Then
                UserLevel = UserLevel + ddlDelete.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlGetAppr.Enabled Then
                UserLevel = UserLevel + ddlGetAppr.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlPost.Enabled Then
                UserLevel = UserLevel + ddlPost.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlUnpost.Enabled Then
                UserLevel = UserLevel + ddlUnpost.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlPrint.Enabled Then
                UserLevel = UserLevel + ddlPrint.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlComplete.Enabled Then
                UserLevel = UserLevel + ddlComplete.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlClosing.Enabled Then
                UserLevel = UserLevel + ddlClosing.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            If ddlCancel.Enabled Then
                UserLevel = UserLevel + ddlCancel.SelectedValue
            Else
                UserLevel = UserLevel + "X"
            End If

            Result = SQLExecuteScalar("EXEC S_SAUserLevelCekData " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text), ViewState("DBConnection").ToString)
            If Result = "0" Then
                SQLString = "EXEC S_SAUserLevelModify " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text) + ", " + QuotedStr(UserLevel) + ", 'Insert'"
            Else
                SQLString = "EXEC S_SAUserLevelModify " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text) + ", " + QuotedStr(UserLevel) + ", 'Edit'"
            End If

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            DataGrid.EditIndex = -1
            BindDataDt()
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub


    Protected Sub btnLevel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLevel.Click
        Dim GVR As GridViewRow
        Dim SQLString, UserLevel, Result As String
        Dim lbCode As Label
        Dim ddlInsert, ddlEdit, ddlDelete, ddlGetAppr, ddlPost, ddlUnpost, ddlPrint, ddlComplete, ddlClosing, ddlCancel As Label

        Try
            For Each GVR In DataGrid.Rows
                lbCode = GVR.FindControl("ContainerId")
                ddlInsert = GVR.FindControl("FgInsert")
                ddlEdit = GVR.FindControl("FgEdit")
                ddlDelete = GVR.FindControl("FgDelete")
                ddlGetAppr = GVR.FindControl("FgGetAppr")
                ddlPost = GVR.FindControl("FgPost")
                ddlUnpost = GVR.FindControl("FgUnPost")
                ddlPrint = GVR.FindControl("FgPrint")
                ddlComplete = GVR.FindControl("FgComplete")
                ddlClosing = GVR.FindControl("FgClosing")
                ddlCancel = GVR.FindControl("FgCancel")
                UserLevel = ""

                'lbStatus.Text = ddlLevel.SelectedValue
                'Exit Sub

                If ddlLevel.SelectedValue = "All" Then
                    'lbStatus.Text = "test saja"
                    'Exit Sub

                    If ddlInsert.Text <> "X" Then
                        UserLevel = ddlSelect.SelectedValue
                    ElseIf ddlInsert.Enabled Then
                        UserLevel = ddlInsert.Text
                    Else
                        UserLevel = "X"
                    End If



                    If ddlEdit.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlEdit.Enabled Then
                        UserLevel = UserLevel + ddlEdit.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    
                    If ddlDelete.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlDelete.Enabled Then
                        UserLevel = UserLevel + ddlDelete.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlGetAppr.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlGetAppr.Enabled Then
                        UserLevel = UserLevel + ddlGetAppr.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlPost.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlPost.Enabled Then
                        UserLevel = UserLevel + ddlPost.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlUnpost.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlUnpost.Enabled Then
                        UserLevel = UserLevel + ddlUnpost.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlPrint.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlPrint.Enabled Then
                        UserLevel = UserLevel + ddlPrint.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlComplete.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlComplete.Enabled Then
                        UserLevel = UserLevel + ddlComplete.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlClosing.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlClosing.Enabled Then
                        UserLevel = UserLevel + ddlClosing.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlCancel.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlCancel.Enabled Then
                        UserLevel = UserLevel + ddlCancel.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If
                    'lbStatus.Text = UserLevel
                    'Exit Sub

                Else

                    If ddlInsert.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgInsert" And ddlInsert.Text <> "X" Then
                        UserLevel = ddlSelect.SelectedValue
                    ElseIf ddlInsert.Enabled Then
                        UserLevel = ddlInsert.Text
                    Else
                        UserLevel = "X"
                    End If


                    If ddlEdit.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgEdit" And ddlEdit.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlEdit.Enabled Then
                        UserLevel = UserLevel + ddlEdit.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    'lbStatus.Text = UserLevel
                    'Exit Sub
                    If ddlDelete.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgDelete" And ddlDelete.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlDelete.Enabled Then
                        UserLevel = UserLevel + ddlDelete.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlGetAppr.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgGetAppr" And ddlGetAppr.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlGetAppr.Enabled Then
                        UserLevel = UserLevel + ddlGetAppr.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlPost.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgPost" And ddlPost.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlPost.Enabled Then
                        UserLevel = UserLevel + ddlPost.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlUnpost.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgUnPost" And ddlUnpost.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlUnpost.Enabled Then
                        UserLevel = UserLevel + ddlUnpost.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlPrint.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgPrint" And ddlPrint.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlPrint.Enabled Then
                        UserLevel = UserLevel + ddlPrint.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlComplete.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgComplete" And ddlComplete.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlComplete.Enabled Then
                        UserLevel = UserLevel + ddlComplete.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlClosing.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgClosing" And ddlClosing.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlClosing.Enabled Then
                        UserLevel = UserLevel + ddlClosing.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                    If ddlCancel.Text <> ddlSelect.SelectedValue And ddlLevel.SelectedValue = "FgCancel" And ddlCancel.Text <> "X" Then
                        UserLevel = UserLevel + ddlSelect.SelectedValue
                    ElseIf ddlCancel.Enabled Then
                        UserLevel = UserLevel + ddlCancel.Text
                    Else
                        UserLevel = UserLevel + "X"
                    End If

                End If
                'lbStatus.Text = UserLevel
                'Exit Sub
                Result = SQLExecuteScalar("EXEC S_SAUserLevelCekData " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text), ViewState("DBConnection").ToString)
                If Result = "0" Then
                    SQLString = "EXEC S_SAUserLevelModify " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text) + ", " + QuotedStr(UserLevel) + ", 'Insert'"
                Else
                    SQLString = "EXEC S_SAUserLevelModify " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text) + ", " + QuotedStr(UserLevel) + ", 'Edit'"
                End If
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                'DataGrid.EditIndex = -1
            Next
            
            BindDataDt()
        Catch ex As Exception
            Throw New Exception("btnAction_Click Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Try
            If tbCode.Text = "" Then
                Exit Sub
            End If
            
            If ddlCommand.SelectedValue = "Expand" Then
                TreeView1.ExpandAll()
            ElseIf ddlCommand.SelectedValue = "Collapse" Then
                TreeView1.CollapseAll()
            ElseIf ddlCommand.SelectedValue = "Grant" Then
                Dim tn As New TreeNode
                For Each tn In TreeView1.Nodes
                    tn.Checked = True
                    ModifyNodes(tn, True)
                Next
            Else
                Dim tn As New TreeNode
                For Each tn In TreeView1.Nodes
                    tn.Checked = False
                    ModifyNodes(tn, False)
                Next
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Go Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ModifyNodes(ByRef parentNode As TreeNode, ByVal state As Boolean)
        Try
            Dim tn As New TreeNode
            For Each tn In parentNode.ChildNodes
                tn.Checked = state
                If tn.ChildNodes.Count > 0 Then
                    ModifyNodes(tn, state)
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Modify Nodes Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim ResultField As String
        Try
            If tbCode.Text = "" Then
                Exit Sub
            End If
            Session("filter") = "select User_Id, User_Name From VSAUsers WHERE FgAdmin <> 'Y' AND User_ID <>" + QuotedStr(tbCode.Text)
            ResultField = "User_Id, User_Name"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnCopy"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Copy Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCopy.Click
        Try
            If tbCopyCode.Text.Trim = "" Then
                Exit Sub
            End If
            If tbCopyCode.Text = tbCode.Text Then
                lbStatus.Text = MessageDlg("Operation failed, cannot copy to same source")
                Exit Sub
            End If
            SQLExecuteNonQuery("EXEC S_SAUserRightCopy " + ddlAction.SelectedIndex.ToString + "," + QuotedStr(tbCode.Text) + "," + QuotedStr(tbCopyCode.Text) + "," + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)
            Dim tn As New TreeNode
            For Each tn In TreeView1.Nodes
                tn.Checked = False
                ModifyNodes(tn, False)
            Next

            PopulateData()
            pnlHd.Visible = True
            pnlCopy.Visible = False
            pnlDt.Visible = False
            lbStatus.Text = MessageDlg("Copy Success")
        Catch ex As Exception
            lbStatus.Text = "btn Save Copy Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQL As String
        Try
            ddlCommand.Enabled = False
            btnGo.Enabled = False
            btnSave.Enabled = False
            SQL = "SELECT [User_Id], [User_Name] from VSAUsers WHERE [User_Id] = " + QuotedStr(tbCode.Text.Trim)
            DT = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                tbCode.Text = Dr("User_Id")
                tbName.Text = Dr("User_Name")
                ActionVisible(True)
                If TreeView1.Nodes.Count > 0 Then
                    TreeView1.Nodes.Clear()
                End If
                PopulateRootLevel()
                TreeView1.ExpandAll()
                PopulateData()
            Else
                tbCode.Text = ""
                tbName.Text = ""
                ActionVisible(False)
                If TreeView1.Nodes.Count > 0 Then
                    TreeView1.Nodes.Clear()
                End If
            End If
            pnlHd.Visible = True
            pnlCopy.Visible = False
            pnlDt.Visible = False

            ddlCommand.Enabled = True
            btnGo.Enabled = True
            btnSave.Enabled = True
            tbCode.Focus()
                        
        Catch ex As Exception
            Throw New Exception("tb Code Change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbCopyCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCopyCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQL As String
        Try
            SQL = "SELECT [User_Id], [User_Name] from VSAUsers WHERE [User_Id] <> " + QuotedStr(tbCode.Text.Trim) + " and [User_Id] = " + QuotedStr(tbCopyCode.Text.Trim)
            DT = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                tbCopyCode.Text = Dr("User_Id")
                tbCopyName.Text = Dr("User_Name")
            Else
                tbCopyCode.Text = ""
                tbCopyName.Text = ""
            End If
            tbCopyCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Copy Code Change Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            pnlCopy.Visible = False
        Catch ex As Exception
            Throw New Exception("Btn Back Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAction.Click
        Try
            If ddlAction.SelectedValue = "Copy To" Or ddlAction.SelectedValue = "Copy From" Then
                lbCopy.Text = ddlAction.SelectedValue + " User : "
                pnlDt.Visible = False
                pnlHd.Visible = False
                pnlCopy.Visible = True
            Else
                pnlDt.Visible = True
                pnlHd.Visible = False
                pnlCopy.Visible = False
                pnlLevel.Visible = True
                PnlBack.Visible = True
                'lbUser.Text = tbCode.Text + " - " + tbName.Text
                BindDataDt()
            End If
        Catch ex As Exception
            Throw New Exception("btnAction_Click Error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub btnCancelCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelCopy.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            pnlCopy.Visible = False
        Catch ex As Exception
            Throw New Exception("btnCancelCopy_Click Error : " + ex.ToString)
        End Try
    End Sub
End Class
