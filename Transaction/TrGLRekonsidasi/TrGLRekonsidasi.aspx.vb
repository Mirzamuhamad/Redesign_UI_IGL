Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_TrGLRekonsidasi_TrGLRekonsidasi
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Session("AdvanceFilter") = Nothing
                InitProperty()
                SetInit()
                tbAccCode.Text = ""
                tbDate.SelectedDate = ViewState("ServerDate")
                tbDateE.SelectedDate = ViewState("ServerDate")
            End If
            lbStatus.Text = ""

            'If Not Session("AdvanceFilter") Is Nothing Then
            '    BindData()
            '    Session("AdvanceFilter") = Nothing
            'End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnAcc" Then
                    tbAccCode.Text = Session("Result")(0).ToString
                    tbAccName.Text = Session("Result")(1).ToString
                    BindData()
                    BindGridDt(ViewState("Dt2"), GridView1)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If

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

    Private Sub SetInit()
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = ViewState("DigitHome")
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        'tbRef.Attributes.Add("ReadOnly", "True")
    End Sub

    'Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
    'Dim DT As DataTable
    'Dim DV As DataView
    'Dim StrQuery As String
    'Dim StrFilter As String
    '    Try
    '        If AdvanceFilter.Length > 1 Then
    '            StrFilter = AdvanceFilter
    '            StrQuery = "EXEC S_GLRekonsilidasiBank " + QuotedStr(tbDate.SelectedDate) + " , " + QuotedStr(tbDateE.Text) + ", " + QuotedStr(tbAccCode.Text) + ", ' order by " + StrFilter + "'"

    '        Else
    '            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '            If StrFilter.Length > 3 Then
    '                StrFilter = " And " + StrFilter
    '            End If
    '            StrQuery = "EXEC S_GLRekonsilidasiBank " + QuotedStr(tbDate.SelectedDate) + " , " + QuotedStr(tbDateE.Text) + ", " + QuotedStr(tbAccCode.Text) + ", " + QuotedStr(StrFilter)
    '        End If

    '        lbStatus.Text = StrQuery
    '        Exit Sub
    '        DT = BindDataTransaction(StrQuery, "", ViewState("DBConnection").ToString)
    '        If DT.Rows.Count = 0 Then
    '            lbStatus.Text = "No Data"
    '            pnlNav.Visible = False
    ''GridView1.Visible = True
    '        End If
    '        DV = DT.DefaultView
    '        DV.Sort = ViewState("SortExpression")
    '        GridView1.DataSource = DV
    '        GridView1.DataBind()
    '        GridView1.Visible = True
    ''GridView1.Columns.Item(8).ItemStyle.HorizontalAlign = HorizontalAlign.Right
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Private Function GetStringDt(ByVal Nmbr As String) As String
    '    Return "EXEC S_GLRekonsilidasiBank " + QuotedStr(Date.Today) + ", " + QuotedStr(Date.Today) + ", '',''"
    '    lbStatus.Text = Nmbr
    '    Exit Function
    'End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            'BindDataDt2(tbAccCode.Text)
            BindData()
            pnlNav.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, EffectiveDate, Remark"
            FilterValue = "Reference, Effective_Date, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        'Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcc.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Account, Description, Currency, Class_Account, Sub_Group_Account, Group_Account from VMsAccountRpt WHERE GroupRpt = 'KAS & BANK' "
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAcc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub
    'Private Function GetStringDt2(ByVal Nmbr As String) As String
    'Return "EXEC S_GLRekonsilidasiBank " + QuotedStr(tbDate.SelectedDate) + " , " + QuotedStr(tbDateE.Text) + ", " + QuotedStr(Nmbr) + ", ''"
    'End Function

    Private Sub BindData()
        Dim SQLString As String
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 3 Then
                StrFilter = " And " + StrFilter
            End If
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            SQLString = "EXEC S_GLRekonsilidasiBank " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbDateE.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbAccCode.Text.Trim) + "," + QuotedStr(StrFilter)
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridView1)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbAccCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccCode.TextChanged
        Dim Dr As DataRow
        Dim dt As DataTable
        Try
            dt = SQLExecuteQuery("Select Account, Description FROM VMsAccountRpt WHERE GroupRpt = 'KAS & BANK' AND Account = " + QuotedStr(tbAccCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Not dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbAccCode.Text = Dr("Account")
                tbAccName.Text = Dr("Description")
                btnAcc.Focus()
                SQLExecuteNonQuery("EXEC S_GLRekonsilidasiBank " + QuotedStr(tbDate.SelectedDate) + " , " + QuotedStr(tbDateE.Text) + ", " + QuotedStr(tbAccCode.Text) + ", ''", ViewState("DBConnection").ToString)
                BindData()
            Else
                tbAccCode.Text = ""
                tbAccName.Text = ""
                tbAccCode.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        'Dim obj As GridViewRow
        'Dim txt As TextBox
        'Try
        '    GridView1.EditIndex = e.NewEditIndex
        '    GridView1.ShowFooter = False
        '    BindData()
        '    obj = GridView1.Rows(e.NewEditIndex)
        '    txt = obj.FindControl("RekNo")
        '    txt.Focus()
        'Catch ex As Exception
        '    lbStatus.Text = "GridView_Edit exception : " + ex.ToString
        'End Try
    End Sub

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGetDt.Click
        Try
            SQLExecuteNonQuery("EXEC S_GLRekonsilidasiBank " + QuotedStr(tbDate.SelectedDate) + " , " + QuotedStr(tbDateE.Text) + ", " + QuotedStr(tbAccCode.Text) + ", ''", ViewState("DBConnection").ToString)
            BindData()
        Catch ex As Exception
            lbStatus.Text = "BtnGetDt_Click exception : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim tbppn As TextBox
        Dim SQLString As String
        Dim Percent, PPn As Double
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True
            Percent = 0
            PPn = 0
            If tbAccCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Accouct must have value")
                tbAccCode.Focus()
                Exit Sub
            End If
            For i = 0 To GridView1.Rows.Count - 1
                GVR = GridView1.Rows(i)
                tbppn = GVR.FindControl("RekNo")

                PPn = PPn + CFloat(tbppn.Text)
            Next
            If exe Then
                ' simpan ke database
                For i = 0 To GridView1.Rows.Count - 1
                    GVR = GridView1.Rows(i)
                    tbppn = GVR.FindControl("RekNo")

                    SQLString = "UPDATE GLJournalDt SET RekNo = " + QuotedStr(tbppn.Text) + " from GLJournalDt WHERE Reference = " + QuotedStr(GVR.Cells(3).Text) + " AND ItemNo = " + QuotedStr(FormatFloat(GVR.Cells(7).Text, ViewState("DigitQty")))
                    SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                Next
            End If
        Catch ex As Exception
            lbStatus.Text = " btn apply error : " + ex.ToString
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
            BindData()
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged, tbDateE.SelectionChanged
        Try
            BindData()
            BindGridDt(ViewState("Dt2"), GridView1)
        Catch ex As Exception
            lbStatus.Text = "tbDate_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
