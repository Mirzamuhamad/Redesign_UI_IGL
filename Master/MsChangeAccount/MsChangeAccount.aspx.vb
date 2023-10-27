Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Master_MsChangeAccount_MsChangeAccount
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SortExpression") = Nothing
                '  DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                '  DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                '  btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCurrent" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnNewProduct" Then
                    tbCodeNew.Text = Session("Result")(0).ToString
                    tbNewName.Text = Session("Result")(1).ToString
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                '  Session("Criteria") = Nothing
                Session("Column") = Nothing

            End If
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "page Load Error : " + ex.ToString
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

    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result As String
        Try
            If tbCode.Text = "" Or tbName.Text = "" Then
                lbStatus.Text = MessageDlg("Current Account must have value")
                tbCode.Focus()
                Exit Sub
            End If

            If tbCodeNew.Text = "" Or tbNewName.Text = "" Then
                lbStatus.Text = MessageDlg("New Account must have value")
                tbNewName.Focus()
                Exit Sub
            End If

            result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_GLAccountChange " + _
            QuotedStr(tbCode.Text) + ", " + QuotedStr(tbCodeNew.Text) + ", @A OUT " + _
            "SELECT @A", ViewState("DBConnection").ToString)

            If result.Length > 2 Then
                lbStatus.Text = MessageDlg(result)
            Else
                lbStatus.Text = MessageDlg("Save Change Account Success!")
            End If

        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrent.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Account, Description FROM VMsAccount Where Account <> " + QuotedStr(tbCodeNew.Text)
            ResultField = "Account, Description"
            ViewState("Sender") = "btnCurrent"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Current Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnNewProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Account, Description FROM VMsAccount Where Account <> " + QuotedStr(tbCode.Text)
            ResultField = "Account, Description"
            ViewState("Sender") = "btnNewProduct"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn New Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT Account, Description FROM VMsAccount Where Account = " + QuotedStr(tbCode.Text) + " and Account <> " + QuotedStr(tbCodeNew.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            tbCode.Text = ""
            tbName.Text = ""
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCode.Text = Dr("Account").ToString
                tbName.Text = Dr("Description").ToString 
            End If
        Catch ex As Exception
            lbStatus.Text = "tbCode_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCodeNew_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCodeNew.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT Account, Description FROM VMsAccount Where Account = " + QuotedStr(tbCodeNew.Text) + " and Account <> " + QuotedStr(tbCode.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            tbCodeNew.Text = ""
            tbNewName.Text = ""
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCodeNew.Text = Dr("Account").ToString
                tbNewName.Text = Dr("Description").ToString 
            End If
        Catch ex As Exception
            lbStatus.Text = "btn tbCodeNew_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
