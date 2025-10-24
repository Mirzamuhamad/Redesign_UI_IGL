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
            ' ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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


                InitProperty()
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

     

                 bindDataGrid()
                SetInit()

                 ' Tambahkan ini untuk generate MobileMenuHtml
                Dim sqlString As String = "EXEC S_SAUserMenu " + QuotedStr(ViewState("UserId")) + ", ''"
                Dim dtMenu As DataTable = SQLExecuteQuery(sqlString, ViewState("DBConnection").ToString).Tables(0)
                Dim menu = dtMenu.Select("ISNULL(MenuLevel, 0) = 0")
                Dim sb As New StringBuilder()
                GenerateMyMenu(menu, dtMenu, sb)
                ViewState("MobileMenuHtml") = sb.ToString()

                       
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



' Private Sub DataModule()
'     Dim SQLString As String = "EXEC S_SAUserMenu " + QuotedStr(ViewState("UserId")) + ", ''"
'     Dim dtMenu As DataTable = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

'     menuTop.Items.Clear()

'     ' Ambil menu level 0 (menu utama)
'     Dim rootMenus = dtMenu.Select("ISNULL(MenuLevel, 0) = 0")
'     For Each dr As DataRow In rootMenus
'         Dim menuItem As New MenuItem(dr("MenuName").ToString(), dr("MenuId").ToString())
'         If dr("MenuUrl").ToString().Length > 3 Then
'         '     menuItem.NavigateUrl = dr("MenuUrl").ToString() + "?KeyId=" + ViewState("KeyId").ToString() + "&ContainerId=" + dr("MenuId").ToString()
'         ' menuItem.Target = "MainFrame"

'         menuItem.NavigateUrl = "javascript:loadMenu('" & dr("MenuId").ToString() & "')"

'         End If
'         AddChildItems(menuItem, dtMenu)
'         menuTop.Items.Add(menuItem)
'     Next
' End Sub

Private Sub DataModule()
    Dim SQLString As String = "EXEC S_SAUserMenu " + QuotedStr(ViewState("UserId")) + ", ''"
    Dim dtMenu As DataTable = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

    menuTop.Items.Clear()

    ' Ambil menu level 0 (menu utama)
    Dim rootMenus = dtMenu.Select("ISNULL(MenuLevel, 0) = 0")
    For Each dr As DataRow In rootMenus
        Dim menuItem As New MenuItem(dr("MenuName").ToString(), dr("MenuId").ToString())

        ' Ganti NavigateUrl menjadi javascript
        If dr("MenuUrl").ToString().Length > 3 Then
    Dim url As String = dr("MenuUrl").ToString() + "?KeyId=" + ViewState("KeyId").ToString() + "&ContainerId=" + dr("MenuId").ToString()
    menuItem.NavigateUrl = "javascript:window.open('" & url & "', 'InFrame');"
Else
    menuItem.NavigateUrl = "javascript:loadMenu('" & dr("MenuId").ToString() & "')"
End If


        AddChildItems(menuItem, dtMenu)
        menuTop.Items.Add(menuItem)
    Next
End Sub



Private Sub AddChildItems(ByVal parentItem As MenuItem, ByVal table As DataTable)
    Dim childRows = table.Select("MenuParent = '" + parentItem.Value + "'", "OrderBy ASC")

    ' Ambil ClientID dari panel-panel untuk dipakai di JavaScript
    Dim pnlSearchId As String = pnlSearch.ClientID
    Dim pnlTransferId As String = PnlTransfer.ClientID

    For Each dr As DataRow In childRows
        Dim childItem As New MenuItem(dr("MenuName").ToString(), dr("MenuId").ToString())
If dr("MenuUrl").ToString().Length > 3 Then
   ' Ambil ClientID dari panel-panel

Dim url As String = dr("MenuUrl").ToString() + "?KeyId=" + ViewState("KeyId").ToString() + "&ContainerId=" + dr("MenuId").ToString()

If TrimStr(dr("MenuParam").ToString()) <> "" Then
    url &= "&MenuParam=" + dr("MenuParam").ToString()
End If

' Gunakan JavaScript custom function
childItem.NavigateUrl = "javascript:hidePanelsAndLoadInFrame('" & url & "');"

End If


        AddChildItems(childItem, table) ' Untuk cucu menu
        parentItem.ChildItems.Add(childItem)
    Next
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
    sb.AppendLine("<ul class='mobile-menu'>")
    For Each dr As DataRow In menu
        GenerateMenuItem(dr, table, sb)
    Next
    sb.AppendLine("</ul>")
    Return sb.ToString()
End Function

Private Sub GenerateMenuItem(ByVal dr As DataRow, ByVal table As DataTable, ByVal sb As StringBuilder)
    Dim pid As String = dr("MenuId").ToString()
    Dim menuText As String = dr("MenuName").ToString()
    Dim url As String = dr("MenuUrl").ToString()
    Dim hasUrl As Boolean = (url.Length > 3)

    Dim fullUrl As String = url
    If hasUrl Then
        fullUrl &= "?KeyId=" + ViewState("KeyId").ToString() + "&ContainerId=" + pid
        If TrimStr(dr("MenuParam").ToString()) <> "" Then
            fullUrl &= "&MenuParam=" + dr("MenuParam").ToString()
        End If
    End If

    Dim childRows As DataRow() = table.Select("MenuParent = '" + pid + "'")

    If childRows.Length > 0 Then
        sb.AppendLine("<li class='has-submenu'>")
        sb.AppendLine("<span onclick='toggleSubmenu(this)'>" + menuText + " ▸</span>")
        sb.AppendLine("<ul class='submenu'>")
        For Each child In childRows
            GenerateMenuItem(child, table, sb)
        Next
        sb.AppendLine("</ul>")
        sb.AppendLine("</li>")
    Else
        If hasUrl Then
            sb.AppendLine("<li><a href='" + fullUrl + "' target='InFrame'>" + menuText + "</a></li>")
        Else
            sb.AppendLine("<li><span>" + menuText + "</span></li>")
        End If
    End If
End Sub



End Class
