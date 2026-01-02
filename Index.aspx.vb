Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
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
                 bindDataGridCIP()
                 bindDataGridDateExoired()

                SetInit()

                 ' Tambahkan ini untuk generate MobileMenuHtml
                Dim sqlString As String = "EXEC S_SAUserMenu " + QuotedStr(ViewState("UserId")) + ", ''"
                Dim dtMenu As DataTable = SQLExecuteQuery(sqlString, ViewState("DBConnection").ToString).Tables(0)
                Dim menu = dtMenu.Select("ISNULL(MenuLevel, 0) = 0")
                Dim sb As New StringBuilder()
                GenerateMyMenu(menu, dtMenu, sb)
                ViewState("MobileMenuHtml") = sb.ToString()

            End If
            dsKegiatan.ConnectionString = ViewState("DBConnection")
        Catch ex As Exception
            lStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub

     Protected Sub DataGridCIP_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridCIP.PageIndexChanging
        DataGridCIP.PageIndex = e.NewPageIndex
        bindDataGridCIP()
    End Sub

    Protected Sub DataGridDateExpired_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDateExpired.PageIndexChanging
        DataGridDateExpired.PageIndex = e.NewPageIndex
       bindDataGridDateExoired()
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
                pnlDashboard.Visible = False
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

            ulDashboard.InnerHtml = ""   'clear

                Dim dt2 As DataTable = tempDS.Tables(0)

                For Each dr As DataRow In dt2.Rows
                    ' Dim menuName As String = dr("Menuname").ToString()
                    ' Dim jumlah As String = dr("Outstanding").ToString()

                    ' ulDashboard.InnerHtml &= " <li class='list-group-item d-flex justify-content-between align-items-center px-2 py-1'> " & menuName & " <span class='badge bg-secondary'>" & jumlah & "</span> </li>"
                    Dim menuId As String = dr("MenuId").ToString()
                    Dim menuName As String = dr("MenuName").ToString()
                    Dim menuUrl As String = dr("MenuUrl").ToString()
                    Dim jumlah As String = dr("Outstanding").ToString()

                    ulDashboard.InnerHtml &= " <li class='list-group-item d-flex justify-content-between align-items-center px-2 py-1' style='cursor:pointer' onclick=""openMenu('" & menuUrl & "','" & menuId & "')""> " & menuName & " <span class='badge bg-info'>" & jumlah & "</span> </li>"
               
                Next

            

            

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Private Sub bindDataGridCIP()
        Dim SqlString As String
        Dim DV As DataView
        Dim tempDS As New DataSet()
        'Dim ddl As DropDownList

        Try

        Dim userGroup As String = SQLExecuteScalar("SELECT UserGroup FROM V_GetUserGroupList WHERE UserID = " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)' Pastikan ViewState ini sudah diisi saat login

            Select Case userGroup.ToUpper()
                Case "PIC"
                'PIC 
                'button Update dan Finish            
                    SqlString = " S_GetListTahapanV2 'PIC'," + QuotedStr(ViewState("UserId"))  

                Case "SPV"
                'SPV
                'Button Detail
                    SqlString = "S_GetListTahapanV2 'SPV'," + QuotedStr(ViewState("UserId"))                   
        
                Case "QC"
                'QC
                'Button Verified
                    SqlString = " S_GetListTahapanV2 'QC'," + QuotedStr(ViewState("UserId")) 

                Case "DIREKSI"
                'Direksi
                'Button Detail
                    SqlString = "S_GetListTahapanV2 'Direksi'," + QuotedStr(ViewState("UserId"))   
                
                Case "ADMIN"
                    SqlString = "SELECT * FROM V_GetTahapanDash "'WHERE TransNmbr =  'IGL/CTD/1125/0003'"
                    'SqlString = "SELECT * FROM V_GetListTahapan "'WHERE TransNmbr =  " + QuotedStr(NoPenyerahanID)
                    
            End Select

            'SqlString = "Exec S_GetMenuDashBoard 'N', " + QuotedStr(ViewState("UserId"))            
            'qlString = "SELECT * FROM V_GetTahapanDash WHERE TransNmbr =  'IGL/CTD/1125/0003'"
            'Untuk List yang muncul satu tahap satu tahap berdasarkan No dokumennya
            'SqlString = "SELECT * FROM V_GetListTahapan "'WHERE TransNmbr =  " + QuotedStr(NoPenyerahanID)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            'For Each row As GridViewRow In DataGrid.Rows
            '    ddl = row.FindControl("ddlValueEdit")
            '    ddl.Enabled = False
            'Next

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridCIP)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpression")
                DataGridCIP.DataSource = DV
                DataGridCIP.DataBind()
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

'==========================================Start Penambahan DashBoard =======================================

' 🔹 Group untuk Definisikan class aturan tombol
Public Class ButtonRule
    Private _Roles As String()
    Private _Buttons As String()

    Public Property Roles() As String()
        Get
            Return _Roles
        End Get
        Set(ByVal value As String())
            _Roles = value
        End Set
    End Property

    Public Property Buttons() As String()
        Get
            Return _Buttons
        End Get
        Set(ByVal value As String())
            _Buttons = value
        End Set
    End Property

    Public Sub New(ByVal roles As String(), ByVal buttons As String())
        _Roles = roles
        _Buttons = buttons
    End Sub
End Class


Protected Sub DataGridCIP_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles DataGridCIP.RowDataBound
    If e.Row.RowType <> DataControlRowType.DataRow Then Exit Sub

    Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
    Dim UserLogin As String = ViewState("UserId").ToString().Trim().ToUpper()

    ' 🔹 Ambil semua field peran dari data view
    Dim roles As New Dictionary(Of String, String)
    roles.Add("PIC", drv("PIC").ToString().Trim().ToUpper())
    roles.Add("Spv1", drv("Spv1").ToString().Trim().ToUpper())
    roles.Add("Spv2", drv("Spv2").ToString().Trim().ToUpper())
    roles.Add("Direksi", drv("Direksi").ToString().Trim().ToUpper())
    roles.Add("QcVerified", drv("QcVerified").ToString().Trim().ToUpper())

    ' 🔹 Ambil kontrol tombol dari template GridView
    Dim btnUpdate As Button = CType(e.Row.FindControl("btnUpdate"), Button)
    Dim btnFinish As Button = CType(e.Row.FindControl("btnFinish"), Button)
    Dim btnDetail As Button = CType(e.Row.FindControl("btnDetail"), Button)
    Dim btnVerified As Button = CType(e.Row.FindControl("btnVerified"), Button)

    ' 🔹 Default: sembunyikan semua tombol
    btnUpdate.Visible = False
    btnFinish.Visible = False
    btnDetail.Visible = False
    btnVerified.Visible = False

    ' 🔹 Khusus ADMIN → tampilkan semua tombol
    If UserLogin = "ADMIN" Then
        btnUpdate.Visible = True
        btnFinish.Visible = True
        btnDetail.Visible = True
        btnVerified.Visible = True
        Exit Sub
    End If

    ' 🔹 Cari role mana saja yang dimiliki user pada row ini
    Dim userRoles As New List(Of String)
    Dim kvp As KeyValuePair(Of String, String)
    For Each kvp In roles
        If kvp.Value = UserLogin Then userRoles.Add(kvp.Key)
    Next

    ' Jika user tidak ada di salah satu role field, lewati
    If userRoles.Count = 0 Then Exit Sub

    ' 🔹 Definisi aturan tombol per kombinasi role
    Dim matchedButtons As New List(Of String)

    ' Semua kombinasi yang mungkin
    If userRoles.Contains("PIC") AndAlso userRoles.Contains("Spv1") AndAlso userRoles.Contains("Spv2") AndAlso userRoles.Contains("Direksi") AndAlso userRoles.Contains("QcVerified") Then
        matchedButtons.AddRange(New String() {"Update", "Finish", "Detail", "Verified"})

    ElseIf userRoles.Contains("PIC") AndAlso Not (userRoles.Contains("Spv1") Or userRoles.Contains("Spv2") Or userRoles.Contains("Direksi") Or userRoles.Contains("QcVerified")) Then
        matchedButtons.AddRange(New String() {"Update", "Finish"})

    ElseIf userRoles.Contains("Spv1") OrElse userRoles.Contains("Spv2") Then
        If userRoles.Contains("Direksi") Then
            matchedButtons.Add("Detail")
        ElseIf userRoles.Contains("PIC") Then
            matchedButtons.AddRange(New String() {"Update", "Finish", "Detail"})
        Else
            matchedButtons.Add("Detail")
        End If

    ElseIf userRoles.Contains("Direksi") Then
        matchedButtons.Add("Detail")

    ElseIf userRoles.Contains("QcVerified") Then
        If userRoles.Contains("PIC") Then
            matchedButtons.AddRange(New String() {"Update", "Finish", "Verified"})
        Else
            matchedButtons.Add("Verified")
        End If
    End If

    ' 🔹 Terapkan hasil visibilitas
    btnUpdate.Visible = matchedButtons.Contains("Update")
    btnFinish.Visible = matchedButtons.Contains("Finish")
    btnDetail.Visible = matchedButtons.Contains("Detail")
    btnVerified.Visible = matchedButtons.Contains("Verified")
End Sub
' 🔹 End Untuk Definisikan class aturan tombol sesuai user ada di field mana

' ' ini masih manual dari user Group
' Protected Sub DataGridCIP_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles DataGridCIP.RowDataBound
'     If e.Row.RowType = DataControlRowType.DataRow Then
    
'         ' Ambil UserGroup dari ViewState
'         Dim userGroup As String = SQLExecuteScalar("SELECT UserGroup FROM V_GetUserGroupList WHERE UserID = " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)' Pastikan ViewState ini sudah diisi saat login

'         ' Temukan tombol dalam baris
'         Dim btnUpdate As Button = CType(e.Row.FindControl("btnUpdate"), Button)
'         Dim btnFinish As Button = CType(e.Row.FindControl("btnFinish"), Button)
'         Dim btnDetail As Button = CType(e.Row.FindControl("btnDetail"), Button)
'         Dim btnVerified As Button = CType(e.Row.FindControl("btnVerified"), Button)

'         ' Default: semua hide dulu (supaya aman)
'         btnUpdate.Visible = False
'         btnFinish.Visible = False
'         btnDetail.Visible = False
'         btnVerified.Visible = False

'         ' Kondisi berdasarkan UserGroup
'         Select Case userGroup.ToUpper()
'             Case "PIC"
'                 btnUpdate.Visible = True
'                 btnFinish.Visible = True

'             Case "SPV", "DIREKSI"
'                 btnDetail.Visible = True

'             Case "QC"
'                 btnVerified.Visible = True    

'             Case "ADMIN"
'                 btnUpdate.Visible = True
'                 btnFinish.Visible = True
'                 btnDetail.Visible = True
'                 btnVerified.Visible = True

'         End Select

'     End If
' End Sub

Protected Sub DataGridCIP_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles DataGridCIP.RowCommand
      Dim index As Integer = Convert.ToInt32(e.CommandArgument)
       ' Dim row As GridViewRow = DataGridCIP.Rows(index) 'Berjalan tetapi kadang ada bug error saat klik pagination
         Dim SqlString As String
        Dim DV As DataView
        Dim tempDS As New DataSet()
      Try
        
        Dim btn As Button = CType(e.CommandSource, Button)
        Dim row As GridViewRow = CType(btn.NamingContainer, GridViewRow)

    If e.CommandName = "ViewDetail" Then  
        'Dim ddl As DropDownList
          ' Ambil MenuID dari kolom pertama         
             Dim NoPenyerahanID As String = CType(row.FindControl("NoPenyerahanID"), Label).Text

             lbNopenyerahan.text  = NoPenyerahanID   
             lbKegiatan.text  = CType(row.FindControl("Kegiatan"), Label).Text      
             lbtglHariIni.Text = CType(row.FindControl("lbharini"), Label).Text   
             lbLamaProsesBerlangsung.Text = CType(row.FindControl("lbTotalHari"), Label).Text                    

            'SqlString = "SELECT Top 20 * FROM MsMenu "'WHERE MenuId "'"=  " + QuotedStr(menuId)
            SqlString = "SELECT * FROM V_GetTahapanDash WHERE TransNmbr =  " + QuotedStr(NoPenyerahanID)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridDetail)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpression")
                GridDetail.DataSource = DV
                GridDetail.DataBind()
            End If
            

            ' Binding ke GridDetail
            ' GridDetail.DataSource = dt
            ' GridDetail.DataBind()
            txtComment.Visible = False
            ' Tampilkan modal
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetailModal", "showDetailModal();", True)
             ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModal();", True)


    Elseif e.CommandName = "ViewVerified" Then 
        Dim NoPenyerahanID As String = CType(row.FindControl("NoPenyerahanID"), Label).Text
        Dim ItemNo As String = CType(row.FindControl("ItemNo"), Label).Text
        Dim ItemNoDt As String = CType(row. FindControl("ItemNoDt"), Label).Text
        Dim StepTahapan As String = CType(row.FindControl("Step"), Label).Text
        
            Try
                ' Jalankan stored procedure VerifiedUpdate
                SqlString = "EXEC S_VerifiedUpdate " + QuotedStr(NoPenyerahanID) + ", " + QuotedStr(ViewState("UserId")) + "," + QuotedStr(ItemNo) + "," + QuotedStr(ItemNoDt) 
                SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)

                ' Tampilkan pesan sukses ke user
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('✅ Proses verifikasi berhasil untuk nomor: " & NoPenyerahanID & " dengan step: " & StepTahapan & "');", True)

            BindDataGridCIP()
            Catch ex As Exception
                ' Jika ada error, tampilkan pesan error
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertError", "alert('❌ Terjadi kesalahan: " & ex.Message.Replace("'", "\'") & "');", True)
            End Try
            
            
    Elseif e.CommandName = "ViewUpdate" Then 
        ViewState("NoPenyerahanID")  = CType(row.FindControl("NoPenyerahanID"), Label).Text 
        ViewState("ItemNo") = CType(row.FindControl("ItemNo"), Label).Text
        ViewState("ItemNoDt") = CType(row. FindControl("ItemNoDt"), Label).Text
        ViewState("StepTahapan") = CType(row.FindControl("Step"), Label).Text       
        tbLog.text = CType(row.FindControl("lbKeterlambatan"), Label).Text   

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModalUpdate();", True)
        tbKeterangan.focus()
    Elseif e.CommandName = "ViewFinish" Then 
        ' lstatus.text = MessageDlg(ConfigurationManager.ConnectionStrings("DBConnection").ConnectionString)
        ' Exit Sub

        ViewState("NoPenyerahanID") = CType(row.FindControl("NoPenyerahanID"), Label).Text 
        ViewState("ItemNo") = CType(row.FindControl("ItemNo"), Label).Text
        ViewState("ItemNoDt") = CType(row. FindControl("ItemNoDt"), Label).Text
        ViewState("StepTahapan") = CType(row.FindControl("Step"), Label).Text
        ViewState("FgPermanent") = CType(row.FindControl("lbfgPermanent"), Label).Text

        'Untuk Alert Confirmation             
        lbNotifFinish.Text = "Apakah Anda yakin ingin selesaikan step " & ViewState("StepTahapan") & " ?"

        If ViewState("FgPermanent") = "N" Then
        
        ' Dim confirmScript As String = "if (confirm('Apakah Anda yakin ingin selesaikan step ini ?')) {" & "__doPostBack('" & btnConfirmVerify.UniqueID & "', 'confirm'); }"
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "confirmVerify", confirmScript, True)
            
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModalFinish();", True)
        Else
            'Untuk tampilkan Row Manual
            ' Dim dt As New DataTable
            ' dt.Columns.Add("Empty")  ' kolom dummy            
 
            ' dt.Rows.Add(dt.NewRow()) ' 1 baris kosong
            ' '   dt.Rows.Add(dt.NewRow()) ' 1 baris kosong

            'Untuk Tampilkan Row Tetapi No Dokumen Kosong
            Dim Sql As String = "SELECT * FROM V_CeKGeTKolomFinish WHERE Reference = " + QuotedStr(ViewState("NoPenyerahanID"))
            Dim ds As DataSet = SQLExecuteQuery(Sql, ViewState("DBConnection").ToString)
            Dim dt As DataTable


            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                dt = ds.Tables(0)
            Else
                dt = New DataTable()
            End If
            
            If dt.Rows.Count = 0 Then
                dt = New DataTable()
                dt.Columns.Add("Empty")
                dt.Rows.Add(dt.NewRow())
            End If

            GridDetailFinish.DataSource = dt
            GridDetailFinish.DataBind()


            ' 'Code Untuk isi No Dokumen otomatis CTDnya mereference to Mustasi sertifikat    
            ' Dim Sql As String = "SELECT * FROM V_CeKGeTKolomFinish WHERE Reference = " & QuotedStr(ViewState("NoPenyerahanID"))
            ' Dim ds As DataSet = SQLExecuteQuery(Sql, ViewState("DBConnection").ToString)
            ' Dim dt As DataTable

            ' If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            '     dt = ds.Tables(0)
            ' Else
            '     dt = New DataTable()
            ' End If

            ' ' Simpan hasil ke ViewState untuk pengisian otomatis row
            ' ViewState("ListNoDok") = dt

            ' ' Jika kosong → tampilkan 1 baris saja
            ' If dt.Rows.Count = 0 Then
            '     Dim dtEmpty As New DataTable()
            '     dtEmpty.Columns.Add("Empty")
            '     dtEmpty.Rows.Add(dtEmpty.NewRow())
            '     GridDetailFinish.DataSource = dtEmpty
            ' Else
            '     GridDetailFinish.DataSource = dt
            ' End If

            ' GridDetailFinish.DataBind()

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModalFlagY();", True)
        End If

    End If

       Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid ViewDetail Error: " & ex.ToString
        Finally
        End Try 
End Sub

Protected Sub GridDetailFinish_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridDetailFinish.RowDataBound

    If e.Row.RowType = DataControlRowType.DataRow Then

        Dim tbNoDok As TextBox = CType(e.Row.FindControl("tbNoDok"), TextBox)

        ' Ambil NoDok dari ViewState
        Dim dt As DataTable = TryCast(ViewState("ListNoDok"), DataTable)

        If dt IsNot Nothing AndAlso dt.Rows.Count > e.Row.RowIndex Then
            tbNoDok.Text = dt.Rows(e.Row.RowIndex)("NoDokumen").ToString()
        End If

    End If

End Sub



Protected Sub btnFinishFlagY_Click(sender As Object, e As EventArgs) Handles btnFinishFlagY.Click
    Dim SqlString As String
    Dim tbTglTerbit, tbTglExpired, tbTglTerbitCek, tbTglExpiredCek  As BasicFrame.WebControls.BasicDatePicker
        Try

    Dim errorMsg As String = ""

        'Cek dahulu Kolom dari masing2 Row yang akan di terima

        For Each row2 As GridViewRow In GridDetailFinish.Rows
            Dim ddlKegiatanCek As DropDownList = CType(row2.FindControl("ddlKegiatan"), DropDownList)
            Dim tbNoDokCek As TextBox = CType(row2.FindControl("tbNoDok"), TextBox)
            Dim tbPerihalCek As TextBox = CType(row2.FindControl("tbPerihal"), TextBox)
            Dim tbInstansiCek As TextBox = CType(row2.FindControl("tbInstansi"), TextBox)
             tbTglTerbitCek  = row2.FindControl("tbTglTerbit")
             tbTglExpiredCek = row2.FindControl("tbTglExpired")         

            If ddlKegiatanCek.SelectedValue = "" Then
                errorMsg &= "- Kegiatan belum dipilih pada baris " & row2.RowIndex + 1 & "\n"
            End If

            If tbNoDokCek.Text.Trim() = "" Then
                errorMsg &= "- Nomor Dokumen kosong pada baris " & row2.RowIndex + 1 & "\n"
            End If

            If tbPerihalCek.Text.Trim() = "" Then
                errorMsg &= "- Perihal kosong pada baris " & row2.RowIndex + 1 & "\n"
            End If

            If tbInstansiCek.Text.Trim() = "" Then
                errorMsg &= "- Instansi kosong pada baris " & row2.RowIndex + 1 & "\n"
            End If

            If  tbTglTerbitCek.SelectedDate = Nothing Then
                errorMsg &= "- Tanggal Terbit kosong pada baris " & row2.RowIndex + 1 & "\n"
            End If

            ' If  tbTglExpiredCek.SelectedDate = Nothing Then
            '     errorMsg &= "- Tanggal Expired kosong pada baris " & row2.RowIndex + 1 & "\n"
            ' End If
        Next 


        'Jika ada error → batalkan semua proses insert
        If errorMsg <> "" Then
        ' Escape untuk alert()
            Dim safeMsg As String = errorMsg.Replace("'", "\'")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ok", "alert('❌ Lengkapi semua data sebelum menyimpan:\n" & safeMsg & "');", True)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModalFlagY();", True)

                Exit Sub
        End If
        
        'Proses Input data
        For Each row As GridViewRow In GridDetailFinish.Rows
            Dim PenyerahanID As String =  ViewState("NoPenyerahanID")
            Dim ddlKegiatan As DropDownList = CType(row.FindControl("ddlKegiatan"), DropDownList)
            Dim tbNoDok As TextBox = CType(row.FindControl("tbNoDok"), TextBox)
            Dim tbPerihal As TextBox = CType(row.FindControl("tbPerihal"), TextBox)
            Dim tbInstansi As TextBox = CType(row.FindControl("tbInstansi"), TextBox)
             tbTglTerbit  = row.FindControl("tbTglTerbit")
             tbTglExpired   = row.FindControl("tbTglExpired")
             Dim ItemNo As String = ViewState("ItemNo") 
            Dim ItemNoDt As String = ViewState("ItemNoDt")

            if tbTglExpired.SelectedDate = Nothing Then
                tbTglExpired.SelectedDate = ViewState("ServerDate")
            End If
            ' Insert data dari masing2 row yang di input sesuai dengan banyaknya dokumen yang di terima
            SqlString = "DECLARE @A VARCHAR(225) EXEC S_FinishGenerateDocReceive " + QuotedStr(PenyerahanID) + ", " + QuotedStr(ViewState("UserId")) + ", " + ItemNo + ", " + ItemNoDt + ", " + QuotedStr(ddlKegiatan.SelectedValue) + ", " + QuotedStr(tbNoDok.Text) + ", " + QuotedStr(tbPerihal.Text) + ", " + QuotedStr(tbInstansi.Text) + ", '" + Format(tbTglTerbit.SelectedDate, "yyyy-MM-dd") + "', '" + Format(tbTglExpired.SelectedDate, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("FgPermanent")) + ", @A OUT SELECT @A "
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
      
        Next
         ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('✅ Proses Finish berhasil untuk nomor: " & ViewState("NoPenyerahanID") & " dengan step: " & ViewState("StepTahapan") & "');", True)

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ok", "alert('✅ Data berhasil disimpan melalui SP !');", True)
         BindDataGridCIP()

    Catch ex As Exception
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "err", "alert('ERROR: " & ex.Message.Replace("'", "\'") & "');", True)
    End Try
End Sub


Protected Sub btnConfirmVerify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirmVerify.click
        Dim SqlString As String
        Dim NoPenyerahanID As String = ViewState("NoPenyerahanID") 
        Dim ItemNo As String = ViewState("ItemNo") 
        Dim ItemNoDt As String = ViewState("ItemNoDt") 
        Dim UserId As String = ViewState("UserId").ToString()
        Dim FgPermanent As String = ViewState("FgPermanent")
        Dim StepTahapan As String = ViewState("StepTahapan")
        Try  
            'Proses Finish
             SqlString = "EXEC S_VerifiedFinish " + QuotedStr(NoPenyerahanID) + ", " + QuotedStr(ViewState("UserId")) + "," + QuotedStr(ItemNo) + "," + QuotedStr(ItemNoDt) + ",  " + QuotedStr(FgPermanent) 
                SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)          
            ' Tampilkan pesan sukses 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('✅ Proses Finish Step " & StepTahapan & " berhasil ');", True)
            BindDataGridCIP()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertError", "alert('❌ Terjadi kesalahan: " & ex.Message.Replace("'", "\'") & "');", True)
        End Try        
End Sub



Protected Sub btnUpdateTahapan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateTahapan.Click

 Dim SqlString As String
        Try
        ' Pastikan data dari modal sudah terisi
        Dim NoPenyerahanID As String = ViewState("NoPenyerahanID") 
        Dim ItemNo As String = ViewState("ItemNo") 
        Dim ItemNoDt As String = ViewState("ItemNoDt") 
        Dim UserId As String = ViewState("UserId").ToString()
        Dim Keterangan As String = tbKeterangan.Text
        Dim StepTahapan As String = ViewState("StepTahapan")

        If tbKeterangan.text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertError", "alert('❌ Keterangan Harus di isi..!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModalUpdate();", True)
            tbKeterangan.focus()
            Exit SUb
        End If
 

        ' Jalankan stored procedure UpdateLogKeterlambatan
        SqlString = "EXEC S_UpdateLogKeterlambatan " + QuotedStr(NoPenyerahanID) + ", " + QuotedStr(ViewState("UserId")) + "," + QuotedStr(ItemNo) + "," + QuotedStr(ItemNoDt) + ",  " + QuotedStr(Keterangan) 
                SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)

        ' ✅ Pesan sukses
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage","alert('✅ Keterangan berhasil diperbarui untuk nomor: " & NoPenyerahanID & " dengan step: " & StepTahapan & "');", True)
        ' Optional: refresh grid agar data baru langsung muncul
        BindDataGridCIP()
        tbKeterangan.text = ""

    Catch ex As Exception
        ' ❌ Tampilkan pesan error
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertError", "alert('❌ Terjadi kesalahan: " & ex.Message.Replace("'", "\'") & "');", True)
    End Try
    End Sub


Protected Sub DataGridCIP_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles DataGridCIP.RowEditing
    ' Penting! Kosongkan agar GridView tidak auto-trigger Edit mode
End Sub


Protected Sub GridDetail_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridDetail.RowCommand
    If e.CommandName = "ViewLogComment" Then
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = GridDetail.Rows(index)

        ' Ambil MenuId dari LinkButton yang diklik
        Dim idLink As LinkButton = CType(row.FindControl("btnLinkComment"), LinkButton)
        Dim lbLog AS Label = row.FindControl("lbLog")
        Dim menuId As String = idLink.Text

        ' Tampilkan textbox dan isi dengan MenuId
        txtComment.Visible = True
        'txtComment.Text = "Alasan Keterlambatan : " & lbLog.text
        txtComment.Text = "Alasan Keterlambatan '" +menuId+ "' :" & Environment.NewLine & lbLog.Text

       'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetailModal", "showDetailModal();", True)
       ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModal();", True)
       txtComment.focus
    End If
End Sub

'Versi langsung update data grid
' Protected Sub tmExpired_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmExpired.Tick
'     bindDataGridDateExoired()
'     updExpired.Update()
    
' End Sub

' ' Versi hitung Total record dahulu sebelum refresh data, ada perubahan jumlah record atau tidak, kalau ada akan jalankan refreshh grid
'' di tutup dahulu karena kalau di hp jadi flicker
' Protected Sub tmExpired_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmExpired.Tick
'     Dim newCount As Integer = GetExpiredCount()

'     ' Jika first-load, simpan dulu total record awal
'     If ViewState("ExpiredLastCount") Is Nothing Then
'         ViewState("ExpiredLastCount") = newCount
'         bindDataGridDateExoired()
'         updExpired.Update()
'         Exit Sub
'     End If

'     ' Hanya refresh jika jumlah data berubah
'     If newCount <> CInt(ViewState("ExpiredLastCount")) Then
'         bindDataGridDateExoired()
'         ViewState("ExpiredLastCount") = newCount
'          System.Threading.Thread.Sleep(100) ' stabilisasi Safari iOS
'         updExpired.Update()
'     End If
    
' End Sub

' 'Ambil total record pertama kali reload page
' Private Function GetExpiredCount() As Integer
'     Dim count As Integer =  SQLExecuteScalar("SELECT COUNT(TransNmbr) FROM V_GetDokExpiredReminder", ViewState("DBConnection").ToString)

'     Return count
' End Function

'Untuk Refresh by data
        <System.Web.Services.WebMethod()> _
        Public Shared Function GetExpiredCount() As Integer
            Dim count As Integer = 0
            
            Dim connStr As String = ConfigurationManager.ConnectionStrings("DBConnection").ConnectionString

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand("SELECT COUNT(TransNmbr) FROM V_GetDokExpiredReminder", conn)
                    conn.Open()
                    count = CInt(cmd.ExecuteScalar())
                End Using
            End Using

            Return count
        End Function



    
        Private Sub bindDataGridDateExoired()
        Dim SqlString As String
        Dim DV As DataView
        Dim tempDS As New DataSet()
        'Dim ddl As DropDownList

        Try
            'SqlString = "Exec S_GetMenuDashBoard 'N', " + QuotedStr(ViewState("UserId"))
            SqlString = "SELECT * FROM V_GetDokExpiredReminder"
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            'For Each row As GridViewRow In DataGrid.Rows
            '    ddl = row.FindControl("ddlValueEdit")
            '    ddl.Enabled = False
            'Next

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDateExpired)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpression")
                DataGridDateExpired.DataSource = DV
                DataGridDateExpired.DataBind()
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    '==============================================================================End DashBoard


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Try
            'Session.Clear()
            If Not Session(Request.QueryString("KeyId")) Is Nothing Then
                Session(Request.QueryString("KeyId")) = Nothing
            End If

            Response.Redirect("Default.aspx")
            pnlSearch.Visible = False
            PnlTransfer.Visible = False
            pnlDashboard.Visible = False


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
