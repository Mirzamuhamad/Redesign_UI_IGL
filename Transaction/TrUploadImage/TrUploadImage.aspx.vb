Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
'Imports System.Linq
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO

Partial Class TrUploadImage_TrUploadImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        'Dim A As UserControl_MsgBox.YesButtonPressedHandler
        'A = New UserControl_MsgBox.YesButtonPressedHandler(e, omb_YesButtonPressed)
        If Not IsPostBack Then
            InitProperty()
            lbGroup.Text = "Product"
            lbCode.Text = "TT1"
            lbName.Text = "TT Name"
            If Not Request.QueryString("Code") Is Nothing Then
                FromMasterPage()
            End If
            BindDataImage()
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

    Private Sub FromMasterPage()
        Dim param() As String
        Try
            param = Request.QueryString("Code").ToString.Split("|")
            lbCode.Text = param(0)
            lbName.Text = param(1)
            lbGroup.Text = param(2)
        Catch ex As Exception
            Throw New Exception("Load Assigned Code Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If FupMain.HasFile Then
            Dim path, namafile As String
            path = Server.MapPath("~/image/" + lbGroup.Text.Trim + "/") + lbCode.Text.Trim.Replace("/", "") + FupMain.FileName
            namafile = lbCode.Text.Trim.Replace("/", "") + FupMain.FileName
            Dim dtimage As DataTable

            dtimage = SQLExecuteQuery("Select ImageFile from SAUploadImage WHERE ImageGroup = '" + lbGroup.Text + "' and ImageFile = '" + namafile + "' ", ViewState("DBConnection").ToString).Tables(0)
            If dtimage.Rows.Count = 0 Then
                SQLExecuteNonQuery("INSERT INTO SAUploadImage ( ImageGroup, ImageCode, ImageFile, UserId, UserDate ) " + _
                        " VALUES ( '" + lbGroup.Text + "', '" + lbCode.Text + "', '" + namafile + "', '" + ViewState("UserId").ToString + "', GetDate() ) ", ViewState("DBConnection").ToString)
                FupMain.SaveAs(path)
                BindDataImage()
            Else
                FupMain.SaveAs(path)
                BindDataImage()
                lstatus.Text = "File " + namafile + " has already exists directory upload"
                Exit Sub
            End If

            Dim drimage() As DataRow
            drimage = ViewState("DtImage").Select("PictureURL = '" + ResolveUrl("~/image/" + lbGroup.Text.Trim + "/" + namafile) + "'")
            'lstatus.Text = drimage("PictureID").ToString + " - " + drimage("PictureURL").ToString
            'Exit Sub
            If drimage.Count > 0 Then
                Dim dr As DataRow
                dr = drimage(0)
                GrdImage.PageIndex = dr("PictureID").ToString
                GrdImage.DataBind()
            End If

        End If

    End Sub

    Protected Sub GrdImage_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdImage.PageIndexChanging
        GrdImage.PageIndex = e.NewPageIndex
        'If GrdImage.EditIndex <> -1 Then
        '    DataGrid_RowCancelingEdit(Nothing, Nothing)
        'End If
        BindDataImage()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If ViewState("DtImage").rows.Count = 0 Then
            Exit Sub
        End If

        Dim dr As DataRow
        Dim filePath, namafile As String 
        dr = ViewState("DtImage").Select("PictureID = " + GrdImage.PageIndex.ToString)(0)

        filePath = dr("PictureURL").ToString 
        namafile = Path.GetFileName(filePath)
        SQLExecuteNonQuery("DELETE SAUploadImage WHERE ImageGroup = '" + lbGroup.Text + "' and ImageCode = '" + lbCode.Text + "' AND ImageFile = '" + namafile + "' ", ViewState("DBConnection").ToString)
        File.Delete(Server.MapPath("~/image/" + lbGroup.Text.Trim + "/") + namafile)
        'Response.Redirect(Request.Url.AbsoluteUri)
        BindDataImage() 
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        If ViewState("DtImage").rows.Count = 0 Then
            Exit Sub
        End If
        Dim dr As DataRow
        Dim filePath, namafile As String 
        dr = ViewState("DtImage").Select("PictureID = " + GrdImage.PageIndex.ToString)(0)
        filePath = dr("PictureURL").ToString 
        namafile = Path.GetFileName(filePath)
        filePath = Server.MapPath("~/image/" + lbGroup.Text.Trim + "/") + namafile
        Response.ContentType = ContentType
        Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(filePath)))
        Response.WriteFile(filePath)
        Response.End()
        'BindDataImage() 

    End Sub

    'Protected Sub btnOpen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnopen.Click
    '    If ViewState("DtImage").rows.Count = 0 Then
    '        Exit Sub
    '    End If
    '    Dim dr As DataRow
    '    Dim filePath, namafile As String
    '    dr = ViewState("DtImage").Select("PictureID = " + GrdImage.PageIndex.ToString)(0)
    '    filePath = dr("PictureURL").ToString
    '    namafile = Path.GetFileName(filePath)
    '    filePath = Server.MapPath("~/image/" + lbGroup.Text.Trim + "/") + namafile
    '    Response.Redirect(filePath)
    'End Sub

    Protected Sub btnOpen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnopen.Click
        If ViewState("DtImage").rows.Count = 0 Then
            Exit Sub
        End If
        Dim dr As DataRow
        Dim filePath, namafile As String
        dr = ViewState("DtImage").Select("PictureID = " + GrdImage.PageIndex.ToString)(0)
        filePath = dr("PictureURL").ToString
        namafile = path.GetFileName(filePath)
        filePath = Server.MapPath("~/image/" + lbGroup.Text.Trim + "/") + namafile
        'Dim path As String = Server.MapPath("~/image/" + lbGroup.Text.Trim)
        Dim client As New Net.WebClient()
        Dim buffer As [Byte]() = client.DownloadData(filePath)

        If buffer IsNot Nothing Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-length", buffer.Length.ToString())
            Response.BinaryWrite(buffer)
        End If
    End Sub

    Protected Sub BindDataImage()
        Dim filePaths() As String = Directory.GetFiles(Server.MapPath("~/image/" + lbGroup.Text.Trim))
        Dim files As List(Of ListItem) = New List(Of ListItem)
        'For Each filePath As String In filePaths
        '    files.Add(New ListItem("~/image/" + lbGroup.Text.Trim + "/" + Path.GetFileName(filePath), filePath))
        'Next

        Dim dt As New DataTable()
        ' define the table's schema
        dt.Columns.Add(New DataColumn("PictureID", GetType(Integer)))
        dt.Columns.Add(New DataColumn("PictureURL", GetType(String)))

        Dim dtimage As DataTable
        dtimage = SQLExecuteQuery("Select ImageFile from SAUploadImage WHERE ImageGroup = '" + lbGroup.Text + "' and ImageCode = '" + lbCode.Text + "' ", ViewState("DBConnection").ToString).Tables(0)

        Dim No As Integer
        No = 0

        For Each drimage As DataRow In dtimage.Rows
            For Each filePath As String In filePaths
                If (Not (Path.GetFileName(filePath) = "Thumbs.db")) And (Path.GetFileName(filePath) = drimage("ImageFile").ToString) Then
                    Dim dr As DataRow = dt.NewRow()
                    dr("PictureID") = No
                    dr("PictureURL") = ResolveUrl("~/image/" + lbGroup.Text.Trim + "/" + Path.GetFileName(filePath))
                    dt.Rows.Add(dr)
                    No = No + 1
                End If
            Next
        Next
        ViewState("DtImage") = dt
        GrdImage.DataSource = dt
        GrdImage.DataBind()
    End Sub

End Class
