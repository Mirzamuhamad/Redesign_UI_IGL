Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
'Imports System.Linq
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO

Partial Class Execute_Master_Testing_Testing
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
            'BindDataImage()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If

        'If ViewState("Sender") = "btnCust" Then
        '    lbl01.Text = "Find Data " + Session("Result")(0).ToString
        '    lbl02.Text = "Find Data " + Session("Result")(1).ToString
        'End If

        dsAccType.ConnectionString = ViewState("DBConnection")
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

    'Sub omb_YesButtonPressed(ByVal sender As Object, ByVal args As System.EventArgs)
    '    lbl01.Text = "Status OK"
    'End Sub

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
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function
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
            SqlString = "Select A.*, B.AccTypeName from MsAccGroup A INNER JOIN MsAccType B ON A.AccType = B.AccTypeCode " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "AccGroupCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            'Session("SelectCommand") = "EXEC S_FormMsAccGroup"
            Session("SelectCommand") = "S_FormPrintMaster3 'VMsAccGroup A','AccGroupCode','AccGroupName','AccTypeName','Account Group File','Account Group Code','Account Group Name','Type' ," + QuotedStr(StrFilter)
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("DBConnection") = ViewState("DBConnection")
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScriptAJAX("openprintdlg();", Page, Me.GetType)
            'AttachScript("openprintdlg();", Page, Me.GetType)


            'Dim ReportGw As New ReportDocument
            'Dim Ds As DataSet
            'Dim crParameterFieldDefinitnions As ParameterFieldDefinitions
            'Dim crparameter1, crprmColumn As ParameterFieldDefinition
            'Dim crparameter1values, crprmColumnvalues As ParameterValues
            'Dim crDiscrete1Value As New ParameterDiscreteValue
            'Try
            'Ds = SQLExecuteQuery("Select A.AccGroupCode AS Code, A.AccGroupName AS Name, B.AccTypeName AS Col3 from MsAccGroup A INNER JOIN MsAccType B ON A.AccType = B.AccTypeCode ")
            'ReportGw.Load(Server.MapPath("~\Rpt\PrintMaster3.Rpt"))

            'ReportGw.SetDataSource(Ds.Tables(0))
            'crParameterFieldDefinitnions = ReportGw.DataDefinition.ParameterFields
            'crparameter1 = crParameterFieldDefinitnions.Item("Title")
            'crparameter1values = crparameter1.CurrentValues
            'crDiscrete1Value.Value = "Account Group File"
            'crparameter1values.Add(crDiscrete1Value)
            'crparameter1.ApplyCurrentValues(crparameter1values)

            'crprmColumn = crParameterFieldDefinitnions.Item("Col3Title")
            'crprmColumnvalues = crparameter1.CurrentValues
            'crDiscrete1Value.Value = "Type"
            'crprmColumnvalues.Add(crDiscrete1Value)
            'crprmColumn.ApplyCurrentValues(crprmColumnvalues)

            'Session("Report") = ReportGw
            'Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")
            Session("DBConnection") = ViewState("DBConnection")
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
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
        Dim SQLString As String
        Dim dbCode, dbName As TextBox
        Dim cbxAccType As DropDownList

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("AccGroupCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("AccGroupNameAdd")
                cbxAccType = DataGrid.FooterRow.FindControl("AccTypeAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Account Group Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Account Group Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT AccGroupCode From VMsAccGroup WHERE AccGroupCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Account Group " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsAccGroup (AccGroupCode, AccGroupName, AccType, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(cbxAccType.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()

            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("AccGroupCode")

            SQLExecuteNonQuery("Delete from MsAccGroup where AccGroupCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("AccGroupNameEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim CbxAccType As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("AccGroupCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("AccGroupNameEdit")
            CbxAccType = DataGrid.Rows(e.RowIndex).FindControl("AccTypeEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Account Group Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsAccGroup set AccGroupName= " + QuotedStr(dbName.Text) + "," & _
            "AccType = " + QuotedStr(CbxAccType.SelectedValue) + " where AccGroupCode = " & QuotedStr(lbCode.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    'Protected Sub ClearLabel()
    '    lbl01.Text = ""
    '    lbl02.Text = ""
    '    lbl03.Text = ""
    'End Sub

    'Protected Sub btn1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn1.Click
    '    ClearLabel()
    '    lbl01.Text = "Klik dari Panel 1"
    'End Sub

    'Protected Sub btn2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn2.Click
    '    ClearLabel()
    '    lbl01.Text = "Klik dari Panel 1"
    '    lbl02.Text = "Klik dari Panel 1 ( Cross )"
    'End Sub

    'Protected Sub btn1b_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn1b.Click
    '    ClearLabel()
    '    lbl02.Text = "Klik dari Panel 2"
    'End Sub

    'Protected Sub btn2b_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn2b.Click
    '    ClearLabel()
    '    lbl01.Text = "Klik dari Panel 2 ( Cross )"
    '    lbl02.Text = "Klik dari Panel 2"
    'End Sub

    'Protected Sub btn3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn3.Click
    '    ClearLabel()
    '    lbl01.Text = "Klik dari Panel 1 (btn3)"
    '    lbl03.Text = "Klik dari Panel 1 ( Cross )"
    'End Sub

    'Protected Sub pnlFind_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlFind.Load
    '    lblFind.Text = "test"
    'End Sub

    'Protected Sub btnFind2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind2.Click
    '    FindDlgCity.Visible = True
    '    FindDlgCity.EnableModelDialog(True)
    'End Sub


    'Protected Sub FindDlgCity_LoginStatus(ByVal sender As Object, ByVal e As System.EventArgs) Handles FindDlgCity.LoginStatus
    '    'If (PopupLoginControl1.IsLogin) Then
    '    '{
    '    '    divComments.Visible = true;
    '    '    //lnkWriteMessage.Visible = false;
    '    '}
    '    'Else
    '    '{
    '    '    divComments.Visible = false;
    '    '    //lnkWriteMessage.Visible = true;
    '    '}
    'End Sub

    Protected Sub btnFindDlg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindDlg.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsCustomer"
            ResultField = "Customer_Code, Customer_Name, Currency, Contact_Person, Term"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            'Dim Msg2 As String
            'Msg2 = "alert('Welcome')"
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), Msg2, True)
            Session("DBConnection") = ViewState("DBConnection")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenSearchDlg();", True)
            'ScriptManager.RegisterStartupScript(sender, Me.GetType(), "Find", "OpenSearchDlg();", False)
            'lbl03.Text = "TEST Find Dialog"
            'Exit Sub
        Catch ex As Exception
            lstatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMsg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMsg.Click
        'MessageDlg("Test")
        MsgBoxOpen.MsgBox("Confirmation", "Yakin ?")
        'If MsgBoxOpen.GetResult = "Y" Then
        '    lbl01.Text = "Yes"
        'Else
        '    lbl01.Text = "Non"
        'End If
        'If MsgBoxOpen.GetResult = True Then
        'MessageDlg("OK")
        'Else
        'MessageDlg("NO")
        'End If
    End Sub

    Protected Sub btnLoading_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoading.Click
        WaitOpen.Show("Compute Analyzing")
        Timer1.Enabled = True
    End Sub

    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        WaitOpen.Hide()
        Timer1.Enabled = False
    End Sub

    'Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
    '    If FupMain.HasFile Then
    '        Dim path As String
    '        path = Server.MapPath("~/image/Product/") + FupMain.FileName
    '        FupMain.SaveAs(path)
    '        'imgviewer.ImageUrl = "~/image/Product/" + FupMain.FileName
    '        BindDataImage()
    '    End If
    'End Sub

    'Protected Sub GrdImage_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdImage.PageIndexChanging
    '    GrdImage.PageIndex = e.NewPageIndex
    '    'If GrdImage.EditIndex <> -1 Then
    '    '    DataGrid_RowCancelingEdit(Nothing, Nothing)
    '    'End If
    '    BindDataImage()
    'End Sub

    'Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    '    Dim dr As DataRow 
    '    Dim filePath, namafile As String 
    '    dr = ViewState("DtImage").Select("PictureID = " + GrdImage.PageIndex.ToString)(0)
    '    filePath = dr("PictureURL").ToString 
    '    namafile = Path.GetFileName(filePath)
    '    '"~/image/Product/" + 
    '    File.Delete(Server.MapPath("~/image/Product/") +namafile)
    '    'Response.Redirect(Request.Url.AbsoluteUri)
    '    BindDataImage() 
    'End Sub

    'Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
    '    Dim dr As DataRow 
    '    Dim filePath, namafile As String 
    '    dr = ViewState("DtImage").Select("PictureID = " + GrdImage.PageIndex.ToString)(0)
    '    filePath = dr("PictureURL").ToString 
    '    namafile = Path.GetFileName(filePath)
    '    '"~/image/Product/" + 
    '    filePath = Server.MapPath("~/image/Product/") +namafile
    '    Response.ContentType = ContentType
    '    Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(filePath)))
    '    Response.WriteFile(filePath)
    '    Response.End()
    '    'BindDataImage() 
    'End Sub

    'Protected Sub BindDataImage()
    '    Dim filePaths() As String = Directory.GetFiles(Server.MapPath("~/image/product"))
    '    Dim files As List(Of ListItem) = New List(Of ListItem)
    '    'For Each filePath As String In filePaths
    '    '    files.Add(New ListItem("~/image/product/" + Path.GetFileName(filePath), filePath))
    '    'Next

    '    Dim dt As New DataTable()
    '    ' define the table's schema
    '    dt.Columns.Add(New DataColumn("PictureID", GetType(Integer)))
    '    dt.Columns.Add(New DataColumn("PictureURL", GetType(String)))    

    '    Dim No As Integer
    '    No=0
    '    For Each filePath As String In filePaths
    '        if not (Path.GetFileName(filePath) = "Thumbs.db") then
    '            Dim dr As DataRow = dt.NewRow()
    '            dr("PictureID") = No
    '            dr("PictureURL") = ResolveUrl("~/image/product/"+Path.GetFileName(filePath))
    '            dt.Rows.Add(dr)
    '            No=No+1
    '        End if
    '    Next    
    '    ViewState("DtImage") = dt   
    '    GrdImage.DataSource = dt
    '    GrdImage.DataBind()
    'End Sub
End Class
