Imports System.IO
Imports System.Data

Partial Class FormCekPostPR
    Inherits System.Web.UI.Page

    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Session("DTPost") = Nothing
    End Sub
    Private Sub SetInit()
        Dim DV As DataView
        Dim DT As DataTable
        Try
            DT = Session("DTPost")
            DV = DT.DefaultView
            DV.Sort = ViewState("SortExpression")
            Gridcek.DataSource = Session("DTPost")
            Gridcek.DataBind()
            If DT.Rows(0)("FgAdmin").ToString = "N" Then
                lbstatus.Text = "Please contact Administrator to make BAP"
                btnOK2.Visible = False
                btnCancel2.Visible = False
                btnBack2.Visible = True
                pnlEditDt.Visible = False
            Else
                lbstatus.Text = ""
                btnOK2.Visible = True
                btnCancel2.Visible = True
                btnBack2.Visible = False
                pnlEditDt.Visible = True
                tbBAPDate.SelectedDate = Now
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            SetInit()
        End If
    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel2.Click, btnBack2.Click
        If Session("FgCekPOST") = "PR" Then
            Response.Redirect("..\..\Transaction\TrPRReq\TrPRReq.aspx?ContainerId=PRRequestID")
        ElseIf Session("FgCekPOST") = "POService" Then
            Response.Redirect("..\..\Transaction\TrPOService\TrPOService.aspx?ContainerId=POServiceID")
        ElseIf Session("FgCekPOST") = "POGoods" Then
            Response.Redirect("..\..\Transaction\TrPOGoods\TrPOGoods.aspx?ContainerId=POGoodsID")
        End If
    End Sub

    Protected Sub btnOK2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnOK2.Click
        Dim StrError As String
        Dim SQLString As String
        Dim PrimaryKey() As String
        PrimaryKey = Session("PostNmbr").ToString.Trim.Split("|")

        If Trim(tbBAPNo.Text) = "" Then
            lbstatus.Text = "BAP No. must have value"
            tbBAPNo.Focus()
            Exit Sub
        End If

        If tbBAPDate.IsNull Then
            lbstatus.Text = "BAP Date must have value"
            tbBAPDate.Focus()
            Exit Sub
        End If
        If Session("FgCekPOST") = "PR" Then
            StrError = ExecSPCommandGo("Post", "S_PRRequest", Session("PostNmbr").ToString, CInt(Session("GLYear")), CInt(Session("GLPeriod")), Session("UserID").ToString, Session("DBConnection").ToString)
            If Trim(StrError) <> "" Then
                lbstatus.Text = lbstatus.Text + lbstatus.Text + " <br/>"
                Exit Sub
            Else
                SQLString = "UPDATE PRCRequestHd SET BAPNo = '" + tbBAPNo.Text + "', BAPDate = '" + Format(tbBAPDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                " WHERE TransNmbr = '" + Session("PostNmbr").ToString + "'"
                SQLString = ChangeQuoteNull(SQLString)
                SQLExecuteNonQuery(SQLString, Session("DBConnection").ToString)
            End If
            Response.Redirect("..\..\Transaction\TrPRReq\TrPRReq.aspx?ContainerId=PRRequestID")
        ElseIf (Session("FgCekPOST") = "POGoods") Then
            StrError = ExecSPCommandGo("Post", "S_PRPO", Session("PostNmbr").ToString, CInt(Session("GLYear")), CInt(Session("GLPeriod")), Session("UserID").ToString, Session("DBConnection").ToString)
            If Trim(StrError) <> "" Then
                lbstatus.Text = lbstatus.Text + lbstatus.Text + " <br/>"
                Exit Sub
            Else
                SQLString = "UPDATE PRCPOHd SET BAPNo = '" + tbBAPNo.Text + "', BAPDate = '" + Format(tbBAPDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                " WHERE TransNmbr = '" + PrimaryKey(0).ToString + "' AND Revisi = " + PrimaryKey(1).ToString
                SQLString = ChangeQuoteNull(SQLString)
                SQLExecuteNonQuery(SQLString, Session("DBConnection").ToString)
            End If
            Response.Redirect("..\..\Transaction\TrPOGoods\TrPOGoods.aspx?ContainerId=POGoodsID")
        ElseIf (Session("FgCekPOST") = "POGoodsImp") Then
            StrError = ExecSPCommandGo("Post", "S_PRPO", Session("PostNmbr").ToString, CInt(Session("GLYear")), CInt(Session("GLPeriod")), Session("UserID").ToString, Session("DBConnection").ToString)
            If Trim(StrError) <> "" Then
                lbstatus.Text = lbstatus.Text + lbstatus.Text + " <br/>"
                Exit Sub
            Else
                SQLString = "UPDATE PRCPOHd SET BAPNo = '" + tbBAPNo.Text + "', BAPDate = '" + Format(tbBAPDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                " WHERE TransNmbr = '" + PrimaryKey(0).ToString + "' AND Revisi = " + PrimaryKey(1).ToString
                SQLString = ChangeQuoteNull(SQLString)
                SQLExecuteNonQuery(SQLString, Session("DBConnection").ToString)
            End If
            Response.Redirect("..\..\Transaction\TrPOGoods\TrPOGoods.aspx?ContainerId=POGoodsImpID")
        ElseIf (Session("FgCekPOST") = "POService") Then
            StrError = ExecSPCommandGo("Post", "S_PRPO", Session("PostNmbr").ToString, CInt(Session("GLYear")), CInt(Session("GLPeriod")), Session("UserID").ToString, Session("DBConnection").ToString)
            If Trim(StrError) <> "" Then
                lbstatus.Text = lbstatus.Text + lbstatus.Text + " <br/>"
                Exit Sub
            Else
                SQLString = "UPDATE PRCPOHd SET BAPNo = '" + tbBAPNo.Text + "', BAPDate = '" + Format(tbBAPDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                " WHERE TransNmbr = '" + PrimaryKey(0).ToString + "' AND Revisi = " + PrimaryKey(1).ToString
                SQLString = ChangeQuoteNull(SQLString)
                SQLExecuteNonQuery(SQLString, Session("DBConnection").ToString)
            End If
            Response.Redirect("..\..\Transaction\TrPOService\TrPOService.aspx?ContainerId=POServiceID")
        End If

    End Sub
End Class
