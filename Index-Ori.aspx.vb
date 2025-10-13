Imports System.Data
Imports System.Data.SqlClient
'Imports OboutInc.EasyMenu_Pro

Partial Class Index
    Inherits System.Web.UI.Page
    Private Sub Index_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Try
        '    Dim SQL As String
        '    Dim lastMenuId As String = ""
        '    Dim FgChangePeriod As String = "N"

        '    SQL = "SELECT FgChangePeriod FROM VSAUsers WHERE [User_Id] = " + QuotedStr(Session("UserId"))
        '    FgChangePeriod = SQLExecuteScalar(SQL, Session("DBConnection").ToString)
        '    If FgChangePeriod = "N" Then
        '        someID.Text = "Period Accounting :"
        '        someID.Enabled = False
        '    Else
        '        someID.Text = "Change Period :"
        '        someID.Enabled = True
        '    End If
        'Catch ex As Exception
        '    lStatus.Text = "index init error : " + ex.ToString
        'End Try
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

    Private Sub SetInit()
        Try
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
            ddlYear.SelectedValue = Year(ViewState("ServerDate"))
            lbYearAfter.Text = " Beginning, " + (ddlYear.SelectedValue + 1).ToString
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim param As String
        Try
            If Not IsPostBack Then
                ViewState("KeyId") = Request.QueryString("KeyId") ' contoh
                HiddenKeyId.Value = ViewState("KeyId")

                If Request.QueryString("KeyId") Is Nothing Then
                    'Session.Clear()
                    'Response.Redirect("Default.Aspx")
                End If
                If Not Request.QueryString("KeyId") Is Nothing Then
                    ViewState("KeyId") = Request.QueryString("KeyId")
                    If Session(ViewState("KeyId").ToString) Is Nothing Then
                        'Session.Clear()
                        Response.Redirect("Default.Aspx")
                    End If
                End If



                'lStatus.Text = Request.QueryString("ContainerId")

                'lStatus.Text = Right(Request.Url.ToString(), 6)

                If Right(Request.Url.ToString(), 6) = "DashID" Or Len(Request.Url.ToString()) = 47 Or Len(Request.Url.ToString()) = 44 Or Len(Request.Url.ToString()) = 56 Or Len(Request.Url.ToString()) = 54 Then
                    pnlSearch.Visible = True
                    PKon.Visible = False
                    PnlTransfer.Visible = False

                Else
                    ForInFrame.Visible = False
                    PKon.Visible = True
                    PnlTransfer.Visible = False
                    pnlSearch.Visible = False
                End If
                InitProperty()

                Dim SQL As String
                Dim lastMenuId As String = ""
                Dim FgChangePeriod As String = "N"

                SQL = "SELECT FgChangePeriod FROM VSAUsers WHERE [User_Id] = " + QuotedStr(ViewState("UserId"))
                FgChangePeriod = SQLExecuteScalar(SQL, ViewState("DBConnection").ToString)
                If FgChangePeriod = "N" Then
                    someID.Text = "Period Accounting :"
                    someID.Enabled = False
                Else
                    someID.Text = "Change Period :"
                    someID.Enabled = True
                End If

                lbCompany.Text = ViewState("CompanyName")
                lbYear.Text = ViewState("GLPeriodName").ToString + ", " + ViewState("GLYear").ToString
                lbUser.Text = ViewState("UserName").ToString
                DataModule()

            End If

            If Not IsPostBack Then
                bindDataGrid()
                SetInit()
                'untuk Get Ip dan nama komputer
                'Dim IPAdd As String = String.Empty
                'Dim IPName As String = String.Empty
                'IPAdd = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
                'If String.IsNullOrEmpty(IPAdd) Then
                '    IPAdd = Request.ServerVariables("REMOTE_ADDR")
                '    IPName = System.Net.Dns.GetHostName()
                'End If
                'lStatus.Text = MessageDlg("Your IP" + IPAdd)
            End If

        Catch ex As Exception
            lStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        Try
            lbYearAfter.Text = " Beginning, " + (ddlYear.SelectedValue + 1).ToString
            lStatus.Text = ""
        Catch ex As Exception
            Throw New Exception("ddlYear selectedindexchanged Error : " + ex.ToString)
        End Try
    End Sub




    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Try
            SQLExecuteNonQuery("EXEC S_GLTransferYear " + ddlYear.SelectedValue, ViewState("DBConnection").ToString)
            lStatus.Text = MessageDlg("Process Transfer Ending Balance is Complete")
        Catch ex As Exception
            Throw New Exception("btnProcess click Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim Url As String
        Dim GVR As GridViewRow

        Try

            If e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("MenuId")
                lbName = GVR.FindControl("MenuUrl")
                'lStatus.Text = MessageDlg(lbName.Text)
                ForInFrame.Visible = True
                Url = lbName.Text + "?KeyId=" + ViewState("KeyId".ToString) + "&ContainerId=" + lbCode.Text
                'Dim s As String = "window.open('" & Url & "', '_Blank');"
                Dim s As String = "window.open('" & Url & "', 'InFrame');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)
                DataGrid.Visible = True
                'Response.Redirect(Url)
                'Untuk Hide Chart
                pnlSearch.Visible = False
                PnlTransfer.Visible = False
            End If

        Catch ex As Exception
            lStatus.Text = lStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub



    Private Sub bindDataGrid()
        Dim SqlString As String
        Dim DV As DataView
        Dim tempDS As New DataSet()
        'Dim ddl As DropDownList

        Try
            SqlString = "Exec S_GetMenuDashBoard 'N', " + QuotedStr(ViewState("UserId"))
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            'For Each row As GridViewRow In DataGrid.Rows
            '    ddl = row.FindControl("ddlValueEdit")
            '    ddl.Enabled = False
            'Next

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGrid)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpression")
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Try
            'Session.Clear()
            If Not Session(Request.QueryString("KeyId")) Is Nothing Then
                Session(Request.QueryString("KeyId")) = Nothing
            End If

            Response.Redirect("Default.aspx")
            pnlSearch.Visible = False
            PnlTransfer.Visible = False


        Catch ex As Exception
            lStatus.Text = "link Button Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lkbHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkbHome.Click
        Try

            Response.Redirect("Index.aspx?KeyId=" + ViewState("KeyId").ToString)
        Catch ex As Exception
            lStatus.Text = "link Button Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataModule()
    
        Dim ContainerId As String

        If Not Request.QueryString("ContainerId") Is Nothing Then
            ContainerId = Request.QueryString("ContainerId").ToString
        Else
            ContainerId = ""
        End If




        Dim Dt As DataTable
        Dim Index As Integer
        Dim SQLString As String

        SQLString = "EXEC S_SAUserMenu " + QuotedStr(ViewState("UserId")) + ", '' "
        Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
        
        Index = 1
        For Each row As DataRow In Dt.Rows
            Dim MenuHd As MenuItem
            MenuHd = New MenuItem(row("MenuName").ToString, row("MenuId").ToString) ' CStr(Index).Trim)

            MenuHd.NavigateUrl = row("MenuUrl").ToString + "?KeyId=" + ViewState("KeyId".ToString) + "&ContainerId=" + row("menuid").ToString '+ "&ModuleId=" + CStr(Index).Trim
            menuTop.Items.Add(MenuHd)
            If Index = 1 And ContainerId = "" Then
                ContainerId = row("MenuId").ToString

            End If
            Index = Index + 1
        Next
        menuTop.FindItem(ContainerId).Selected = True
        'menuTop.FindItem(ModuleId).Selected = True

        SQLString = "EXEC S_SAUserMenu " + QuotedStr(ViewState("UserId")) + ", '" + ContainerId.ToString + "' "
        Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
        Dim drparent As DataRow()
        drparent = Dt.Select("MenuParent = '" + ContainerId.ToString + "'")
        Dim sbmy As StringBuilder = New StringBuilder()
        Dim unorderedListmy As String = GenerateMyMenu(drparent, Dt, sbmy)

        'lStatus.Text = SQLString
        my_menu.InnerHtml = unorderedListmy

        AttachScript("var myMenu; myMenu = new SDMenu(""my_menu""); myMenu.init(); myMenu.collapseAll();", Page, Me.GetType)
    End Sub

    'Public Sub GetMenuData(ByVal ContainerId As String)
    '    Dim Dt As DataTable
    '    Dim Con As String
    '    Con = Session("DBConnection").ToString
    '    Dim SQLString As String
    '    SQLString = "EXEC S_SAUserMenu " + QuotedStr(Session("UserId"))
    '    Dt = SQLExecuteQuery(SQLString, Con).Tables(0)
    '    Dim ViewHd As DataView
    '    ViewHd = New DataView(Dt)
    '    ViewHd.RowFilter = "MenuParent = '" + ContainerId.ToString.Trim + "' "
    '    ViewHd.Sort = "OrderBy ASC"
    '    For Each row As DataRowView In ViewHd
    '        Dim MenuHd As MenuItem
    '        MenuHd = New MenuItem(row("MenuName").ToString, row("MenuId").ToString)
    '        If row("MenuUrl").ToString.Length <= 3 Then
    '            MenuHd.Target = row("UrlTarget").ToString
    '            MenuHd.NavigateUrl = "#"
    '        Else
    '            MenuHd.Target = row("UrlTarget").ToString
    '            If TrimStr(row("MenuParam")).ToString <> "" Then
    '                MenuHd.NavigateUrl = row("MenuUrl").ToString + "?ContainerId=" + row("MenuId").ToString + "&MenuParam=" + row("MenuParam").ToString
    '            Else
    '                MenuHd.NavigateUrl = row("MenuUrl").ToString + "?ContainerId=" + row("MenuId").ToString
    '            End If
    '        End If
    '    Next
    'End Sub

    'Private Sub AddChildItems(ByVal table As DataTable, ByVal menuItem As MenuItem)
    '    Dim viewItem As DataView = New DataView(table)
    '    viewItem.RowFilter = "MenuParent= '" + menuItem.Value + "'"
    '    viewItem.Sort = "OrderBy ASC"
    '    For Each childView As DataRowView In viewItem
    '        Dim childItem As MenuItem = New MenuItem(childView("MenuName").ToString, childView("MenuId").ToString)
    '        If childView("MenuUrl").ToString.Length <= 3 Then
    '            childItem.Target = childView("UrlTarget").ToString
    '            childItem.NavigateUrl = "#"
    '        Else
    '            childItem.Target = childView("UrlTarget").ToString
    '            If TrimStr(childView("MenuParam")).ToString <> "" Then
    '                childItem.NavigateUrl = childView("MenuUrl").ToString + "?ContainerId=" + childView("MenuId").ToString + "&MenuParam=" + childView("MenuParam").ToString
    '            Else
    '                childItem.NavigateUrl = childView("MenuUrl").ToString + "?ContainerId=" + childView("MenuId").ToString
    '            End If
    '        End If
    '        menuItem.ChildItems.Add(childItem)
    '        AddChildItems(table, childItem)
    '    Next
    'End Sub

    Public Function GenerateMyMenu(ByVal menu As DataRow(), ByVal table As DataTable, ByVal sb As StringBuilder) As String
        If (menu.Length > 0) Then
            For Each dr As DataRow In menu
                Dim pid As String = dr("MenuId").ToString()
                Dim handler As String = dr("MenuUrl").ToString()
                Dim menuText As String = dr("MenuName").ToString()
                sb.AppendLine("<div class=""dmenu"">")
                sb.AppendLine("<span class=""mn""> " + menuText + "</span>")

                Dim subMenu As DataRow() = table.Select("MenuParent = '" + pid + "'")
                If (subMenu.Length > 0) Then
                    For Each Subdr As DataRow In subMenu
                        Dim subhandler As String = Subdr("MenuUrl").ToString() + "?KeyId=" + ViewState("KeyId").ToString + "&ContainerId=" + Subdr("MenuId").ToString()

                        If TrimStr(Subdr("MenuParam")).ToString <> "" Then
                            subhandler = subhandler + "&MenuParam=" + Subdr("MenuParam").ToString
                        End If


                        Dim submenuText As String = Subdr("MenuName").ToString()
                        Dim submenuId As String = Subdr("MenuId").ToString()





                        If Subdr("MenuUrl").ToString().Length > 5 Then

                            Dim line As String = String.Format("<a href=""{0}"" Target='MainFrame'>{1}</a>", subhandler, submenuText)
                            sb.Append(line)


                        Else
                            pnlSearch.Visible = False
                            PnlTransfer.Visible = False
                            Dim sb2 As StringBuilder = New StringBuilder()
                            GenerateMySubMenu(Subdr, table, sb2)
                            sb.Append(sb2)

                        End If
                    Next
                End If
                sb.Append("</div>")
            Next
        End If
        Return sb.ToString()
    End Function


    Public Function GenerateMySubMenu(ByVal dr As DataRow, ByVal table As DataTable, ByVal sb As StringBuilder) As String
        Dim pid As String = dr("MenuId").ToString()
        Dim handler As String = dr("MenuUrl").ToString()
        Dim menuText As String = dr("MenuName").ToString()
        sb.AppendLine("<div class=""sub"">")
        sb.AppendLine("<span class=""submn"" id=""sub" + pid.Trim + """ >" + menuText + "</span>")
        Dim subMenu As DataRow() = table.Select("MenuParent = '" + pid + "'")
        If (subMenu.Length > 0) Then
            For Each Subdr As DataRow In subMenu
                Dim subhandler As String = Subdr("MenuUrl").ToString() + "?KeyId=" + ViewState("KeyId").ToString + "&ContainerId=" + Subdr("MenuId").ToString()

                'lStatus.Text = subhandler
                If TrimStr(Subdr("MenuParam")).ToString <> "" Then
                    subhandler = subhandler + "&MenuParam=" + Subdr("MenuParam").ToString
                End If
                Dim submenuText As String = Subdr("MenuName").ToString()
                Dim submenuId As String = Subdr("MenuId").ToString()
                Dim line As String = String.Format("<a class=""submn"" href=""{0}"" Target='MainFrame'>{1}</a>", subhandler, submenuText)
                sb.Append(line)

                'lStatus.Text = line
            Next
        End If
        sb.Append("</div>")
        Return sb.ToString()
    End Function


End Class
