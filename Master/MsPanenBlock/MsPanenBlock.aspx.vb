Imports System.Data
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.Data

Partial Class Master_MsPanenBlock_MsPanenBlock
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                tbDate.SelectedDate = Date.Now
            End If
            dsAssigned.ConnectionString = ViewState("DBConnection")
            dsAvailable.ConnectionString = ViewState("DBConnection") 'Session("DBMaster")
            If Not IsPostBack Then
                ViewState("FromPage") = False
                If Not Request.QueryString("Code") Is Nothing Then
                    FromMasterPage()
                    ViewState("FromPage") = True
                End If
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddAll.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnCopyTo.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y" And ViewState("FromPage") = False
                btnRemove.Visible = ViewState("MenuLevel").Rows(0)("FgDelete") = "Y"
                btnRemoveAll.Visible = ViewState("MenuLevel").Rows(0)("FgDelete") = "Y"
                btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y" And ViewState("FromPage") = False
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSearch" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                    LoadDataGrid()
                ElseIf ViewState("Sender") = "btnSearchFrom" Then
                    tbFromCode.Text = Session("Result")(0).ToString
                    tbFromName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnSearchTo" Then
                    tbToCode.Text = Session("Result")(0).ToString
                    tbToName.Text = Session("Result")(1).ToString
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            lbstatus.Text = ""
        Catch ex As Exception
            lbstatus.Text = "page load Error : " + ex.ToString
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

    Private Sub FromMasterPage()
        Dim param() As String
        Try
            btnCopyTo.Visible = False
            btnPrint.Visible = False
            param = Request.QueryString("Code").ToString.Split("|")
            tbCode.Text = param(0)
            tbName.Text = param(1)
            tbCode.Enabled = False
            btnSearch.Visible = False
            'tbCode_TextChanged(Nothing, Nothing)
            LoadDataGrid()
        Catch ex As Exception
            Throw New Exception("Load Assigned Code Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCopyTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopyTo.Click
        Try
            PnlAssign.Visible = False
            pnlCopy.Visible = True
            btnCopyTo.Enabled = False
            If tbCode.Text.Trim <> "" Then
                tbFromCode.Text = tbCode.Text
                tbFromName.Text = tbName.Text
            Else
                tbFromCode.Text = ""
                tbFromName.Text = ""
            End If
            tbToCode.Text = ""
            tbToName.Text = ""
        Catch ex As Exception
            lbstatus.Text = "btn copy to Error : " + ex.ToString
        End Try
    End Sub

    Private Sub LoadDataGrid()
        Try
            '0 = Format JE, 1 = Account
            LoadDataAssign()
            LoadDataAvailable()
            AssignedGrid.Selection.UnselectAll()
            AvailableGrid.Selection.UnselectAll()
            'AssignedGrid.DataBind()
            'AvailableGrid.DataBind()
        Catch ex As Exception
            Throw New Exception("Assign Grid JE Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ExecSPAddRemove(ByVal GroupValue As String, ByVal StrSelected As String, ByVal Flag As Integer)
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsPanenBlockAddRemove '" + GroupValue + "', '" + StrSelected + "', '" + ViewState("UserId").ToString + "', " + Flag.ToString + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            LoadDataGrid()
        Catch ex As Exception
            Throw New Exception("Execute sp Add Remove Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAll.Click
        Dim StrFilter As String
        Dim P As Integer
        Try
            If tbCode.Text = "" Then Exit Sub
            ' 0 = Warehouse   1 = User
            If AvailableGrid.FilterEnabled = True And AvailableGrid.FilterExpression.Length > 0 Then
                StrFilter = QuotedStr(AvailableGrid.FilterExpression)
                P = StrFilter.Length
                StrFilter = StrFilter.Substring(1, P - 2)
            Else
                StrFilter = ""
            End If
            ExecSPAddRemove(tbCode.Text, StrFilter, 0)
            AvailableGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn add all acc error : " + ex.ToString
        End Try
    End Sub

    Function SelectedGridBlock(ByRef GV As ASPxGridView, ByVal ColumnName As String) As String
        Dim Str1 As String
        Dim First As Boolean
        Dim i As Integer
        Dim Result As List(Of Object)
        Try
            Str1 = ""
            First = True

            Result = GV.GetSelectedFieldValues("BlockCode")

            For i = 0 To Result.Count - 1
                If First Then
                    Str1 = " AND " + ColumnName + " In(''" + Result(i).ToString + "''"
                    First = False
                Else
                    Str1 = Str1 + ",''" + Result(i).ToString + "''"
                End If
            Next

            If Str1.Length > 1 Then
                Str1 = Str1 + ")"
            End If

            If First Then
                Return ""
            Else
                Return Str1
            End If
        Catch ex As Exception
            Throw New Exception("Selected Grid Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Dim StrSelected As String
        Try
            If tbCode.Text = "" Then Exit Sub
            StrSelected = ""
            StrSelected = SelectedGridBlock(AvailableGrid, "BlockCode")
            If StrSelected.Trim <> "" Then
                ExecSPAddRemove(tbCode.Text, StrSelected, 1)
            End If
            AvailableGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn Add Acc Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
        Dim StrSelected As String
        Try
            If tbCode.Text = "" Then Exit Sub
            StrSelected = ""
            StrSelected = SelectedGridBlock(AssignedGrid, "BlockCode")
            If StrSelected.Trim <> "" Then
                ExecSPAddRemove(tbCode.Text, StrSelected, 2)
            End If
            AssignedGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn Remove Acc Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnRemoveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveAll.Click
        Dim StrFilter As String
        Dim P As Integer
        Try
            If tbCode.Text = "" Then Exit Sub
            If AssignedGrid.FilterEnabled = True And AssignedGrid.FilterExpression.Length > 0 Then
                StrFilter = QuotedStr(AssignedGrid.FilterExpression)
                P = StrFilter.Length
                StrFilter = StrFilter.Substring(1, P - 2)
            Else
                StrFilter = ""
            End If
            ExecSPAddRemove(tbCode.Text, StrFilter, 3)
            AssignedGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn Remove All Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try
            pnlCopy.Visible = False
            PnlAssign.Visible = True
            btnCopyTo.Enabled = True
        Catch ex As Exception
            lbstatus.Text = "btn cancel ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopy.Click
        Dim SqlString As String
        Try
            If tbFromCode.Text = tbToCode.Text Then
                lbstatus.Text = "Cannot copy to the same source"
                Exit Sub
            End If
            If tbFromCode.Text.Trim = "" Then
                lbstatus.Text = "Data From cannot empty"
                btnSearchFrom.Focus()
                Exit Sub
            End If
            If tbToCode.Text.Trim = "" Then
                lbstatus.Text = "Data To cannot empty"
                btnSearchTo.Focus()
                Exit Sub
            End If

            SqlString = ""
            SqlString = "EXEC S_MsPanenBlockCopyFrom " + QuotedStr(tbFromCode.Text) + ", " + QuotedStr(tbToCode.Text) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId"))

            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            lbstatus.Text = "Data Copied Successfully"

            pnlCopy.Visible = False
            PnlAssign.Visible = True
            btnCopyTo.Enabled = True
            tbCode.Text = tbToCode.Text
            tbName.Text = tbToName.Text
            LoadDataGrid()
        Catch ex As Exception
            lbstatus.Text = "btn Copy Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * From MsPanen"
            ResultField = "PanenCode, PanenName"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearch"
            Session("Column") = ResultField.Split(",")

            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Search Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnSearchFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchFrom.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * From MsPanen"
            ResultField = "PanenCode, PanenName"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearchFrom"
            Session("Column") = ResultField.Split(",")

            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Search From Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnSearchTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchTo.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * From MsPanen"
            ResultField = "PanenCode, PanenName"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearchTo"
            Session("Column") = ResultField.Split(",")
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Search To Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("select PanenCode, PanenName From MsPanen Where PanenCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr(0).ToString
                tbName.Text = dr(1).ToString
            End If
            LoadDataGrid()
        Catch ex As Exception
            lbstatus.Text = "tb Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFromCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFromCode.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("select PanenCode, PanenName From MsPanen Where PanenCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbFromCode.Text = ""
                tbFromName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbFromCode.Text = dr(0).ToString
                tbFromName.Text = dr(1).ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tb From Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbToCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbToCode.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("select PanenCode, PanenName From MsPanen Where PanenCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbToCode.Text = ""
                tbToName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbToCode.Text = dr(0).ToString
                tbToName.Text = dr(1).ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tb Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsPanenBlockPrint 0, 'Panen - Block " + Format("dd MMM YY", tbDate.SelectedValue) + "', 'Block Code', 'Block Name', " + QuotedStr(ViewState("UserId"))

            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintAssign.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub

    Private Sub LoadDataAssign()
        Try
            If tbCode.Text.Trim <> "" Then

                dsAssigned.SelectCommand = "EXEC S_MsPanenBlockAssign " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            Else
                dsAssigned.SelectCommand = "Select '' as BlockCode, '' as BlockName, '' StartDate, '' EndDate, '' AS DivisionName, '' AS StatusTanam"
            End If
            dsAssigned.ConnectionString = ViewState("DBConnection") 'Session("DBMaster")
            AssignedGrid.DataBind()
        Catch ex As Exception
            Throw New Exception("Assign Grid Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub LoadDataAvailable()
        Try
            If tbCode.Text.Trim <> "" Then
                dsAvailable.SelectCommand = "EXEC S_MsPanenBlockAvailable " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            Else
                dsAvailable.SelectCommand = "Select '' as BlockCode, '' as BlockName, '' LastDate, '' as LastPanen,'' AS DivisionName, '' AS StatusTanam"
            End If
            dsAvailable.ConnectionString = ViewState("DBConnection") 'Session("DBMaster")
            AvailableGrid.DataBind()
        Catch ex As Exception
            Throw New Exception("Assign Grid Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub AssignedGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AssignedGrid.PageIndexChanged
        'New
        Try
            LoadDataAssign()
        Catch ex As Exception
            Throw New Exception("AssignedGrid_PageIndexChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub AssignedGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles AssignedGrid.ProcessColumnAutoFilter
        'New
        Try
            LoadDataAssign()
        Catch ex As Exception
            Throw New Exception("AssignedGrid_ProcessColumnAutoFilter Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub AssignedGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles AssignedGrid.BeforeColumnSortingGrouping
        'New
        Try
            LoadDataAssign()
        Catch ex As Exception
            Throw New Exception("AssignedGrid_BeforeColumnSortingGrouping Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub AvailableGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles AvailableGrid.BeforeColumnSortingGrouping
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            Throw New Exception("AvailableGrid_BeforeColumnSortingGrouping Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub AvailableGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AvailableGrid.PageIndexChanged
        'New
        Try

            LoadDataAvailable()
        Catch ex As Exception
            Throw New Exception("AvailableGrid_PageIndexChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub AvailableGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles AvailableGrid.ProcessColumnAutoFilter
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            Throw New Exception("AvailableGrid_ProcessColumnAutoFilter Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        'New
        Try
            LoadDataGrid()
        Catch ex As Exception
            Throw New Exception("tbDate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
