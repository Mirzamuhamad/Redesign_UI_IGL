Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Master_MsChangeProduct_MsChangeProduct
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then

            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCurrent" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString + " " + TrimStr(Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnNewProduct" Then
                    tbCodeNew.Text = Session("Result")(0).ToString
                    tbNewName.Text = Session("Result")(1).ToString + " " + TrimStr(Session("Result")(2).ToString)
                End If

                Session("Result") = Nothing

            End If
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "page Load Error : " + ex.ToString
        End Try
    End Sub


    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result As String
        Try
            If tbCode.Text = "" Or tbName.Text = "" Then
                lbStatus.Text = MessageDlg("Current Product must have value")
                tbCode.Focus()
                Exit Sub
            End If

            If tbCodeNew.Text = "" Or tbNewName.Text = "" Then
                lbStatus.Text = MessageDlg("New Product must have value")
                tbNewName.Focus()
                Exit Sub
            End If

            result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_STProductChange " + _
            QuotedStr(tbCode.Text) + ", " + QuotedStr(tbCodeNew.Text) + ", @A OUT " + _
            "SELECT @A", ViewState("DBConnection"))

            If result.Length > 2 Then
                lbStatus.Text = MessageDlg(result)
            Else
                lbStatus.Text = MessageDlg("Save Change Product Success!")
            End If

        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCurrent_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCurrent.Click
        Dim ResultField As String
        Try
            If ddlType.SelectedIndex = 0 Then
                Session("filter") = "SELECT * FROM VMsProductFG Where Product_Code <> " + QuotedStr(tbCodeNew.Text)
            Else
                Session("filter") = "SELECT * FROM VMsProductNFG Where Product_Code <> " + QuotedStr(tbCodeNew.Text)
            End If
            ResultField = "Product_Code, Product_Name, Specification"
            ViewState("Sender") = "btnCurrent"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Current Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnNewProduct_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNewProduct.Click
        Dim ResultField As String
        Try
            If ddlType.SelectedIndex = 0 Then
                Session("filter") = "SELECT * FROM VMsProductFG Where Product_Code <> " + QuotedStr(tbCode.Text)
            Else
                Session("filter") = "SELECT * FROM VMsProductNFG Where Product_Code <> " + QuotedStr(tbCode.Text)
            End If
            ResultField = "Product_Code, Product_Name, Specification"
            ViewState("Sender") = "btnNewProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Current Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            If ddlType.SelectedIndex = 0 Then
                SQLString = "SELECT Product_Code, Product_Name, Specification FROM VMsProductFG Where Product_Code = " + QuotedStr(tbCode.Text) + " and Product_Code <> " + QuotedStr(tbCodeNew.Text)
            Else
                SQLString = "SELECT Product_Code, Product_Name, Specification FROM VMsProductNFG Where Product_Code = " + QuotedStr(tbCode.Text) + " and Product_Code <> " + QuotedStr(tbCodeNew.Text)
            End If
            Dt = SQLExecuteQuery(SQLString, Session("DBCOnnection")).Tables(0)
            tbCode.Text = ""
            tbName.Text = ""
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCode.Text = Dr("Product_Code").ToString
                tbName.Text = Dr("Product_Name").ToString + " " + TrimStr(Dr("Specification").ToString)
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
            If ddlType.SelectedIndex = 0 Then
                SQLString = "SELECT Product_Code, Product_Name, Specification FROM VMsProductFG Where Product_Code = " + QuotedStr(tbCodeNew.Text) + " and Product_Code <> " + QuotedStr(tbCode.Text)
            Else
                SQLString = "SELECT Product_Code, Product_Name, Specification FROM VMsProductNFG Where Product_Code = " + QuotedStr(tbCodeNew.Text) + " and Product_Code <> " + QuotedStr(tbCode.Text)
            End If
            Dt = SQLExecuteQuery(SQLString, Session("DBCOnnection")).Tables(0)
            tbCodeNew.Text = ""
            tbNewName.Text = ""
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCodeNew.Text = Dr("Product_Code").ToString
                tbNewName.Text = Dr("Product_Name").ToString + " " + TrimStr(Dr("Specification").ToString)
            End If
        Catch ex As Exception
            lbStatus.Text = "btn tbCodeNew_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
