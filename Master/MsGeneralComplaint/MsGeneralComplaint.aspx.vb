
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Net.Mail

Partial Class MsGeneralComplaint
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
           
            'FillCombo(ddlUnit, "SELECT UnitCode, UnitName FROM MsUnit", True, "UnitCode", "UnitName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            'tbKepadatan.Attributes.Add("OnKeyDown", "return PressNumeric();")

            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"

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


    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged

        Try

            If Left(tbCode.Text, 4) <> "S/" Then
                tbCode.Text = "S/" + tbCode.Text
            Else
                tbCode.Text = tbCode.Text
            End If

        Catch ex As Exception
            lstatus.Text = "ddl Term Error : " + ex.ToString
        End Try
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
            ' If tbCode.Enabled Then
            '     tbCode.Text = ""
            ' End If

            ' tbName.Text = ""
            ' ddlTypeID.SelectedIndex = 0
            ' ddlGender.SelectedIndex = 0
            ' tbSellerID.Text = ""
            ' tbAddress.Text = ""
            ' tbAddress2.Text = ""
            ' tbCity.Text = ""
            ' tbZipCode.Text = ""
            ' tbEmail.Text = ""
            ' tbPhone.Text = ""
            ' tbNpwp.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal SellCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_GeneralComplaintList  WHERE Id = " + QuotedStr(SellCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("Id").ToString)
             BindToText(tbstatus, DT.Rows(0)("Status").ToString)
            BindToText(tbName, DT.Rows(0)("Nama").ToString)
            BindToText(tbRoleType, DT.Rows(0)("Phone").ToString)
            BindToText(tbTitle, DT.Rows(0)("Schedule").ToString)
            BindToText(tbDescrip, DT.Rows(0)("Description").ToString)
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
            SqlString = "SELECT * From V_GeneralComplaintList " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ComplaintId DESC"
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

    ' Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '     Dim StrFilter As String
    '     Try
    '         ddlField2.SelectedValue.Replace("JobCode", "Job_Code")
    '         StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '         Session("DBConnection") = ViewState("DBConnection")
    '         Session("PrintType") = "Print"
    '         Session("SelectCommand") = "S_FormPrintMaster6 'V_GeneralComplaintPortal','SellCode','SellName','Gender','TypeID','SellID','Address1+'', ''+Address2+'', ''+ Desa+'', ''+ Kec+'', ''+ Kab +'', ''+ City +'', Pos: ''+ ZipCode+'', Telp : ''+ Phone','Seller File','Code','Description','Gender','TypeID','SellerID','Alamat'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
    '         Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
    '         AttachScript("openprintdlg();", Page, Me.GetType)
    '     Catch ex As Exception
    '         lstatus.Text = "btn print Error = " + ex.ToString
    '     End Try
    ' End Sub

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
     
        Try
            
        
        ' ================== ACTION MENU ==================
        If e.CommandName = "Go" Then

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim GVR As GridViewRow = DataGrid.Rows(index)
            Dim DDL As DropDownList = CType(GVR.FindControl("ddl"), DropDownList)

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
                If CheckMenuLevel("Edit") = False Then Exit Sub

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

            ElseIf DDL.SelectedValue = "Delete" Then
                If CheckMenuLevel("Delete") = False Then Exit Sub

                SQLExecuteNonQuery("DELETE GeneralComplaintPortal WHERE RequestId = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString())
                bindDataGrid()


            ElseIf DDL.SelectedValue = "Close Tiket" Then

                    Dim lnk As LinkButton = DirectCast(GVR.FindControl("lnkPreview"), LinkButton)                    
                    ' 2. Simpan RequestId ke ViewState
                    ViewState("SelectedRequestId") = lnk.CommandArgument

                     Dim targetEmail As String = ""
                    Dim targetName As String = ""
                    Dim idReq As String = ViewState("SelectedRequestId").ToString()
                    Dim sqlFetch As String = "SELECT Status FROM GeneralComplaintPortal"
                    Dim dtReq As DataTable = BindDataTransaction(sqlFetch,"Id = " + QuotedStr(idReq),ViewState("DBConnection").ToString())
                  
                    If dtReq.Rows(0)("Status").ToString() <> "Proses" Then
                        lstatus.Text = MessageDlg("Cannot Close. The ticket has been " & dtReq.Rows(0)("Status").ToString())
                        exit sub
                        Else
                                  ' Reset alasan
                     tbRemarkClose.Text = ""             
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModalReject();", True)
                    tbRemarkClose.focus()
                    End If
    
    
            
            End If
        End If


        ' ================== LIGHTBOX PREVIEW ==================
        If e.CommandName = "PreviewLightbox" Then

            Dim ComplaintId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim sb As New StringBuilder()
            sb.Append("[")

        Using conn As New System.Data.SqlClient.SqlConnection(ViewState("DBConnection").ToString())
    conn.Open()

                Dim sql As String = "SELECT dbo.Portal_ImageUrl(FilePath) as FilePath ,FileType FROM GeneralComplaintImages WHERE ComplaintId = @ComplaintId ORDER BY Id"

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@ComplaintId", ComplaintId)

                    Using dr As SqlDataReader = cmd.ExecuteReader()
                        While dr.Read()
                           sb.Append("{path:'" & dr("FilePath").ToString() & "',type:'" & dr("FileType").ToString() & "'},")

                        End While
                    End Using
                End Using
            End Using

            If sb.Length > 1 Then sb.Length -= 1
            sb.Append("]")

            ClientScript.RegisterStartupScript( Me.GetType(),"Lightbox","openLightbox(" & sb.ToString() & ");",True)

        End If

        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCommand Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnRejectOK_Click(sender As Object, e As EventArgs) Handles btnRejectOK.Click
    Dim SqlUpdate As String

    Try
        ' 1️⃣ Validasi Remark Close wajib diisi
        If (tbRemarkClose.Text = "") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertRemark", "alert('⚠️ Remark Close harus diisi.');", True)
             ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowDetail", "showCustomModalReject();", True)
            Exit Sub
        End If

        Dim idReq As String = ViewState("SelectedRequestId").ToString()

        ' 2️⃣ Cek status terakhir
        Dim sqlFetch As String = "SELECT Status FROM GeneralComplaintPortal"
        Dim dtReq As DataTable = BindDataTransaction(sqlFetch,"Id = " & QuotedStr(idReq),ViewState("DBConnection").ToString())

        If dtReq.Rows.Count = 0 Then
            lstatus.Text = MessageDlg("Data tidak ditemukan.")
            Exit Sub
        End If

        If dtReq.Rows(0)("Status").ToString() <> "Proses" Then
            lstatus.Text = MessageDlg( "Cannot Close. The Ticket has been " & dtReq.Rows(0)("Status").ToString())
            Exit Sub
        End If

        ' 3️⃣ Update data
        SqlUpdate = " UPDATE GeneralComplaintPortal SET Status = 'SELESAI', ApprovedAt = GETDATE(), ApproveBy = " + QuotedStr(ViewState("UserId").ToString) + " , RemarkClose = " & QuotedStr(tbRemarkClose.Text) & " WHERE Id = " & QuotedStr(idReq) 
        SQLExecuteNonQuery(SqlUpdate, ViewState("DBConnection").ToString())

        ' 4️⃣ Alert berhasil
        ScriptManager.RegisterStartupScript( Me, Me.GetType(),"alertSuccess","alert('✅ Close Ticket berhasil.');", True)

        ' 5️⃣ Refresh grid
        bindDataGrid()

    Catch ex As Exception
        lstatus.Text = MessageDlg("Error in Close Ticket: " & ex.Message)
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
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            ' If tbCode.Text.Trim.Length = 0 Then
            '     lstatus.Text = MessageDlg("ID Code must be filled.")
            '     tbCode.Focus()
            '     Return False
            ' End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    ' Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
    '     Dim SqlString As String
    '     Try
    '         If cekInput() = False Then
    '             Exit Sub
    '         End If

    '         If ViewState("State") = "Insert" Then
    '             If SQLExecuteScalar("SELECT RequestId FROM V_GeneralComplaintPortal WHERE Email = " + QuotedStr(tbEmail.Text), ViewState("DBConnection").ToString).Length > 0 Then
    '                 lstatus.Text = "Email " + QuotedStr(tbEmail.Text) + " has already been exist"
    '                 Exit Sub
    '             End If

    '             SqlString = "INSERT INTO GeneralComplaintPortal (FullName,RoleType, Email, KavlingDesc) " + _
    '             "SELECT " + QuotedStr(tbName.Text) + ", " & _
    '             QuotedStr(ddlRoleType.SelectedValue) + ", " + _
    '             QuotedStr(tbEmail.Text) + ", " + _
    '             QuotedStr(tbKavlingDesc.Text)
    '         Else
    '             SqlString = "UPDATE GeneralComplaintPortal SET FullName = " + QuotedStr(tbName.Text) & _
    '                         ", RoleType = " + QuotedStr(ddlRoleType.SelectedValue) & _
    '                         ", KavlingDesc = " + QuotedStr(tbKavlingDesc.Text) & _
    '                         " WHERE Email = " + QuotedStr(tbEmail.Text)
    '         End If
    '         SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
    '         bindDataGrid()
    '         pnlInput.Visible = False
    '         pnlHd.Visible = True
    '     Catch ex As Exception
    '         lstatus.Text = "Btn Save Error : " + ex.ToString
    '     End Try
    ' End Sub

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

    Public Function GetStatusCss(ByVal status As Object) As String
    If status Is Nothing Then
        Return ""
    End If

    Select Case UCase(status.ToString())
        Case "PENDING"
            Return "status-badge status-pending"
        Case "PROSES"
            Return "status-badge status-prosess"
        Case "SELESAI"
            Return "status-badge status-approved"
        Case Else
            Return "status-badge"
    End Select
End Function


Public Sub SendEmailApproval(ByVal toEmail As String, ByVal userName As String)
    Try
        Dim loginUrl As String = "http://10.10.10.152:5134/Login" ' Ganti dengan link portal Anda
        
        Dim message As New MailMessage()
        message.To.Add(New MailAddress(toEmail))
        message.Subject = "Persetujuan Registrasi Akun Portal"
        message.IsBodyHtml = True

        ' Konten HTML dengan Tombol
        Dim htmlBody As String = ""
        htmlBody &= "<div style='font-family: Arial, sans-serif; background:#f9fafb; padding:20px'>"
        htmlBody &= "  <div style='max-width:600px; margin:auto; background:#ffffff; border-radius:8px; border:1px solid #e5e7eb; overflow:hidden'>"
        
        ' HEADER
        htmlBody &= "    <div style='background:#16a34a; padding:16px; color:white'>"
        htmlBody &= "      <h2 style='margin:0; font-size:18px'>Registrasi Disetujui</h2>"
        htmlBody &= "    </div>"

        ' BODY
        htmlBody &= "    <div style='padding:20px; color:#374151'>"
        htmlBody &= "      <p>Halo <b>" & userName & "</b>,</p>"
        htmlBody &= "      <p>Terima kasih telah melakukan registrasi. Akun Anda saat ini telah <b>disetujui dan aktif</b>.</p>"

        ' BOX STATUS
        htmlBody &= "      <div style='background:#ecfdf5; border-left:4px solid #16a34a; padding:12px; margin:16px 0; border-radius:4px; color:#065f46'>"
        htmlBody &= "        <b>Status Akun: Aktif</b><br>"
        htmlBody &= "        Sekarang Anda dapat mengakses layanan portal kami sepenuhnya."
        htmlBody &= "      </div>"

        ' BUTTON LOGIN
        htmlBody &= "      <div style='text-align: center; margin: 25px 0;'>"
        htmlBody &= "        <a href='" & loginUrl & "' style='background-color: #16a34a; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>"
        htmlBody &= "           Login ke Portal Sekarang"
        htmlBody &= "        </a>"
        htmlBody &= "      </div>"

        ' FOOTER
        htmlBody &= "      <p style='margin-top:24px'>Salam,<br><b>Pengelola Kawasan</b></p>"
        htmlBody &= "      <hr style='margin:24px 0; border:none; border-top:1px solid #e5e7eb'>"
        htmlBody &= "      <p style='font-size:12px; color:#6b7280'>Email ini dikirim secara otomatis.<br>Mohon tidak membalas email ini.</p>"
        htmlBody &= "    </div>"
        
        htmlBody &= "  </div>"
        htmlBody &= "</div>"

        message.Body = htmlBody

        ' Konfigurasi SMTP
        Dim smtp As New SmtpClient() ' Membaca dari Web.Config
        smtp.EnableSsl = True
        smtp.Send(message)

    Catch ex As Exception
        lstatus.Text &= " (Gagal kirim email: " & ex.Message & ")"
    End Try
End Sub


Public Sub SendEmailReject(ByVal toEmail As String, ByVal userName As String, ByVal rejectReason As String)
    Try
        Dim contactSupportUrl As String = "https://portal.kawasananda.com/contact.aspx"
        Dim waNumber As String = "+6285159586662" ' Ganti dengan nomor WhatsApp dukungan Anda
        
        Dim message As New MailMessage()
        message.To.Add(New MailAddress(toEmail))
        message.Subject = "Registrasi Akun Ditolak"
        message.IsBodyHtml = True

        ' Konten HTML dengan Tema Merah
        Dim htmlBody As String = ""
        htmlBody &= "<div style='font-family: Arial, sans-serif; background:#f9fafb; padding:20px'>"
        htmlBody &= "  <div style='max-width:600px; margin:auto; background:#ffffff; border-radius:8px; border:1px solid #e5e7eb; overflow:hidden'>"
        
        ' HEADER (Warna Merah)
        htmlBody &= "    <div style='background:#dc2626; padding:16px; color:white'>"
        htmlBody &= "      <h2 style='margin:0; font-size:18px'>Registrasi Ditolak</h2>"
        htmlBody &= "    </div>"

        ' BODY
        htmlBody &= "    <div style='padding:20px; color:#374151'>"
        htmlBody &= "      <p>Halo <b>" & userName & "</b>,</p>"
        htmlBody &= "      <p>Terima kasih telah melakukan registrasi di portal kami. Mohon maaf, saat ini pengajuan akun Anda <b>belum dapat disetujui</b>.</p>"

        ' BOX STATUS (Warna Merah Muda)
        htmlBody &= "      <div style='background:#fef2f2; border-left:4px solid #dc2626; padding:12px; margin:16px 0; border-radius:4px; color:#991b1b'>"
        htmlBody &= "        <b>Alasan Penolakan:</b><br>"
        htmlBody &= "        " & rejectReason & ""
        htmlBody &= "      </div>"

        htmlBody &= "      <p>Silakan melakukan registrasi ulang dengan data yang benar atau hubungi bagian administrasi jika Anda merasa ini adalah kesalahan.</p>"

        ' BUTTON SUPPORT
        htmlBody &= "  <div style='text-align:center; margin:25px 0;'>"
        htmlBody &= "    <a href='https://wa.me/" & waNumber & "' "
        htmlBody &= "       style='background-color:#25D366; color:#ffffff; "
        htmlBody &= "              padding:12px 20px; text-decoration:none; "
        htmlBody &= "              border-radius:6px; font-weight:bold; "
        htmlBody &= "              display:inline-block;'>"

        htmlBody &= "      <img src='https://cdn-icons-png.flaticon.com/512/124/124034.png' "
        htmlBody &= "           width='18' height='18' "
        htmlBody &= "           style='vertical-align:middle; margin-right:8px; border:0;' />"

        htmlBody &= "      Hubungi via WhatsApp"
        htmlBody &= "    </a>"
        htmlBody &= "  </div>"

        ' FOOTER
        htmlBody &= "      <p style='margin-top:24px'>Salam,<br><b>Pengelola Kawasan</b></p>"
        htmlBody &= "      <hr style='margin:24px 0; border:none; border-top:1px solid #e5e7eb'>"
        htmlBody &= "      <p style='font-size:12px; color:#6b7280'>Email ini dikirim secara otomatis.<br>Mohon tidak membalas email ini.</p>"
        htmlBody &= "    </div>"
        
        htmlBody &= "  </div>"
        htmlBody &= "</div>"

        message.Body = htmlBody

        ' Konfigurasi SMTP
        Dim smtp As New SmtpClient()
        smtp.EnableSsl = True ' Sesuaikan dengan hasil test sebelumnya
        smtp.Send(message)

        ' ' Example usage:
        ' SendEmailReject(targetEmail, targetName, "Dokumen tidak lengkap atau tidak sesuai. Silakan hubungi pengelola kawasan untuk informasi lebih lanjut.")
   

    Catch ex As Exception
        lstatus.Text &= " (Gagal kirim email reject: " & ex.Message & ")"
    End Try
End Sub
    
End Class
