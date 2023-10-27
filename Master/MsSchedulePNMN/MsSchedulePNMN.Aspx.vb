Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Master_MsSchedulePNMN_MsSchedulePNMN
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                'DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                FillCombo(ddlType, "Select distinct ScheduleType from MsSchedulePNMNTemplate", False, "ScheduleType", "ScheduleType", ViewState("DBConnection").ToString)
                FillCombo(ddlCopyScheduleType, "Select distinct ScheduleType from MsSchedulePNMNTemplate", False, "ScheduleType", "ScheduleType", ViewState("DBConnection").ToString)
                'tbDate.SelectedDate = ViewState("ServerDate") 'Date.Today                
                BindData()
            End If
            lbstatus.Text = ""
        Catch ex As Exception
            lbstatus.Text = "Page Load Error : " + ex.ToString
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

    Sub BindData()
        Dim tempDS As New DataSet()
        Dim StrFilter As String
        'Dim QtyConvert, QtyAreal As TextBox
        Try

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If

            dsGetSection.ConnectionString = ViewState("DBConnection")
            tempDS = SQLExecuteQuery("EXEC S_MsSchedulePNMNView " + QuotedStr(ddlType.SelectedValue) + " , " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()
            'For Each GVR In DataGrid.Rows
            '    QtyConvert = GVR.FindControl("QtyConvert")
            '    QtyAreal = GVR.FindControl("QtyAreal")
            '    QtyConvert.Attributes.Add("OnKeyDown", "return PressNumeric();")
            '    QtyAreal.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim SQLString As String
        Dim lbCode, DoneRotasi As Label
        Dim cbW01, cbW02, cbW03, cbW04, cbW05, cbW06, cbW07, cbW08, cbW09, cbW10 As CheckBox
        Dim cbW11, cbW12, cbW13, cbW14, cbW15, cbW16, cbW17, cbW18, cbW19, cbW20 As CheckBox
        Dim cbW21, cbW22, cbW23, cbW24, cbW25, cbW26, cbW27, cbW28, cbW29, cbW30 As CheckBox
        Dim cbW31, cbW32, cbW33, cbW34, cbW35, cbW36, cbW37, cbW38, cbW39, cbW40 As CheckBox
        Dim cbW41, cbW42, cbW43, cbW44, cbW45, cbW46, cbW47, cbW48, cbW49, cbW50 As CheckBox
        Dim cbW51, cbW52, cbW53, cbW54, cbW55, cbW56, cbW57, cbW58, cbW59, cbW60 As CheckBox
        Dim cbW61, cbW62, cbW63, cbW64, cbW65, cbW66, cbW67, cbW68, cbW69, cbW70 As CheckBox
        Dim cbW71, cbW72, cbW73, cbW74, cbW75, cbW76, cbW77, cbW78, cbW79, cbW80 As CheckBox
        Dim cbW81, cbW82, cbW83, cbW84, cbW85, cbW86, cbW87, cbW88, cbW89, cbW90 As CheckBox
        Dim cbW91, cbW92, cbW93, cbW94, cbW95, cbW96, cbW97, cbW98, cbW99, cbW100 As CheckBox
        Dim cbW101, cbW102, cbW103, cbW104 As CheckBox
        Dim VW01, VW02, VW03, VW04, VW05, VW06, VW07, VW08, VW09, VW10 As String
        Dim VW11, VW12, VW13, VW14, VW15, VW16, VW17, VW18, VW19, VW20 As String
        Dim VW21, VW22, VW23, VW24, VW25, VW26, VW27, VW28, VW29, VW30 As String
        Dim VW31, VW32, VW33, VW34, VW35, VW36, VW37, VW38, VW39, VW40 As String
        Dim VW41, VW42, VW43, VW44, VW45, VW46, VW47, VW48, VW49, VW50 As String
        Dim VW51, VW52, VW53, VW54, VW55, VW56, VW57, VW58, VW59, VW60 As String
        Dim VW61, VW62, VW63, VW64, VW65, VW66, VW67, VW68, VW69, VW70 As String
        Dim VW71, VW72, VW73, VW74, VW75, VW76, VW77, VW78, VW79, VW80 As String
        Dim VW81, VW82, VW83, VW84, VW85, VW86, VW87, VW88, VW89, VW90 As String
        Dim VW91, VW92, VW93, VW94, VW95, VW96, VW97, VW98, VW99, VW100 As String
        Dim VW101, VW102, VW103, VW104 As String
        Dim GVR As GridViewRow
        Dim i As Integer
        Try
            For i = 0 To DataGrid.Rows.Count - 1
                GVR = DataGrid.Rows(i)
                lbCode = GVR.FindControl("JobCode")
                DoneRotasi = GVR.FindControl("DoneRotasi")
                cbW01 = GVR.FindControl("cbW01")
                cbW02 = GVR.FindControl("cbW02")
                cbW03 = GVR.FindControl("cbW03")
                cbW04 = GVR.FindControl("cbW04")
                cbW05 = GVR.FindControl("cbW05")
                cbW06 = GVR.FindControl("cbW06")
                cbW07 = GVR.FindControl("cbW07")
                cbW08 = GVR.FindControl("cbW08")
                cbW09 = GVR.FindControl("cbW09")
                cbW10 = GVR.FindControl("cbW10")
                cbW11 = GVR.FindControl("cbW11")
                cbW12 = GVR.FindControl("cbW12")
                cbW13 = GVR.FindControl("cbW13")
                cbW14 = GVR.FindControl("cbW14")
                cbW15 = GVR.FindControl("cbW15")
                cbW16 = GVR.FindControl("cbW16")
                cbW17 = GVR.FindControl("cbW17")
                cbW18 = GVR.FindControl("cbW18")
                cbW19 = GVR.FindControl("cbW19")
                cbW20 = GVR.FindControl("cbW20")
                cbW21 = GVR.FindControl("cbW21")
                cbW22 = GVR.FindControl("cbW22")
                cbW23 = GVR.FindControl("cbW23")
                cbW24 = GVR.FindControl("cbW24")
                cbW25 = GVR.FindControl("cbW25")
                cbW26 = GVR.FindControl("cbW26")
                cbW27 = GVR.FindControl("cbW27")
                cbW28 = GVR.FindControl("cbW28")
                cbW29 = GVR.FindControl("cbW29")
                cbW30 = GVR.FindControl("cbW30")
                cbW31 = GVR.FindControl("cbW31")
                cbW32 = GVR.FindControl("cbW32")
                cbW33 = GVR.FindControl("cbW33")
                cbW34 = GVR.FindControl("cbW34")
                cbW35 = GVR.FindControl("cbW35")
                cbW36 = GVR.FindControl("cbW36")
                cbW37 = GVR.FindControl("cbW37")
                cbW38 = GVR.FindControl("cbW38")
                cbW39 = GVR.FindControl("cbW39")
                cbW40 = GVR.FindControl("cbW40")
                cbW41 = GVR.FindControl("cbW41")
                cbW42 = GVR.FindControl("cbW42")
                cbW43 = GVR.FindControl("cbW43")
                cbW44 = GVR.FindControl("cbW44")
                cbW45 = GVR.FindControl("cbW45")
                cbW46 = GVR.FindControl("cbW46")
                cbW47 = GVR.FindControl("cbW47")
                cbW48 = GVR.FindControl("cbW48")
                cbW49 = GVR.FindControl("cbW49")
                cbW50 = GVR.FindControl("cbW50")
                cbW51 = GVR.FindControl("cbW51")
                cbW52 = GVR.FindControl("cbW52")
                cbW53 = GVR.FindControl("cbW53")
                cbW54 = GVR.FindControl("cbW54")
                cbW55 = GVR.FindControl("cbW55")
                cbW56 = GVR.FindControl("cbW56")
                cbW57 = GVR.FindControl("cbW57")
                cbW58 = GVR.FindControl("cbW58")
                cbW59 = GVR.FindControl("cbW59")
                cbW60 = GVR.FindControl("cbW60")
                cbW61 = GVR.FindControl("cbW61")
                cbW62 = GVR.FindControl("cbW62")
                cbW63 = GVR.FindControl("cbW63")
                cbW64 = GVR.FindControl("cbW64")
                cbW65 = GVR.FindControl("cbW65")
                cbW66 = GVR.FindControl("cbW66")
                cbW67 = GVR.FindControl("cbW67")
                cbW68 = GVR.FindControl("cbW68")
                cbW69 = GVR.FindControl("cbW69")
                cbW70 = GVR.FindControl("cbW70")
                cbW71 = GVR.FindControl("cbW71")
                cbW72 = GVR.FindControl("cbW72")
                cbW73 = GVR.FindControl("cbW73")
                cbW74 = GVR.FindControl("cbW74")
                cbW75 = GVR.FindControl("cbW75")
                cbW76 = GVR.FindControl("cbW76")
                cbW77 = GVR.FindControl("cbW77")
                cbW78 = GVR.FindControl("cbW78")
                cbW79 = GVR.FindControl("cbW79")
                cbW80 = GVR.FindControl("cbW80")
                cbW81 = GVR.FindControl("cbW81")
                cbW82 = GVR.FindControl("cbW82")
                cbW83 = GVR.FindControl("cbW83")
                cbW84 = GVR.FindControl("cbW84")
                cbW85 = GVR.FindControl("cbW85")
                cbW86 = GVR.FindControl("cbW86")
                cbW87 = GVR.FindControl("cbW87")
                cbW88 = GVR.FindControl("cbW88")
                cbW89 = GVR.FindControl("cbW89")
                cbW90 = GVR.FindControl("cbW90")
                cbW91 = GVR.FindControl("cbW91")
                cbW92 = GVR.FindControl("cbW92")
                cbW93 = GVR.FindControl("cbW93")
                cbW94 = GVR.FindControl("cbW94")
                cbW95 = GVR.FindControl("cbW95")
                cbW96 = GVR.FindControl("cbW96")
                cbW97 = GVR.FindControl("cbW97")
                cbW98 = GVR.FindControl("cbW98")
                cbW99 = GVR.FindControl("cbW99")
                cbW100 = GVR.FindControl("cbW100")
                cbW101 = GVR.FindControl("cbW101")
                cbW102 = GVR.FindControl("cbW102")
                cbW103 = GVR.FindControl("cbW103")
                cbW104 = GVR.FindControl("cbW104")
                If cbW01.Checked = True Then
                    VW01 = "1"
                Else
                    VW01 = "0"
                End If
                If cbW02.Checked = True Then
                    VW02 = "1"
                Else
                    VW02 = "0"
                End If
                If cbW03.Checked = True Then
                    VW03 = "1"
                Else
                    VW03 = "0"
                End If
                If cbW04.Checked = True Then
                    VW04 = "1"
                Else
                    VW04 = "0"
                End If
                If cbW05.Checked = True Then
                    VW05 = "1"
                Else
                    VW05 = "0"
                End If
                If cbW06.Checked = True Then
                    VW06 = "1"
                Else
                    VW06 = "0"
                End If
                If cbW07.Checked = True Then
                    VW07 = "1"
                Else
                    VW07 = "0"
                End If
                If cbW08.Checked = True Then
                    VW08 = "1"
                Else
                    VW08 = "0"
                End If
                If cbW09.Checked = True Then
                    VW09 = "1"
                Else
                    VW09 = "0"
                End If
                If cbW10.Checked = True Then
                    VW10 = "1"
                Else
                    VW10 = "0"
                End If
                If cbW01.Checked = True Then
                    VW01 = "1"
                Else
                    VW01 = "0"
                End If
                If cbW02.Checked = True Then
                    VW02 = "1"
                Else
                    VW02 = "0"
                End If
                If cbW03.Checked = True Then
                    VW03 = "1"
                Else
                    VW03 = "0"
                End If
                If cbW04.Checked = True Then
                    VW04 = "1"
                Else
                    VW04 = "0"
                End If
                If cbW05.Checked = True Then
                    VW05 = "1"
                Else
                    VW05 = "0"
                End If
                If cbW06.Checked = True Then
                    VW06 = "1"
                Else
                    VW06 = "0"
                End If
                If cbW07.Checked = True Then
                    VW07 = "1"
                Else
                    VW07 = "0"
                End If
                If cbW08.Checked = True Then
                    VW08 = "1"
                Else
                    VW08 = "0"
                End If
                If cbW09.Checked = True Then
                    VW09 = "1"
                Else
                    VW09 = "0"
                End If
                If cbW10.Checked = True Then
                    VW10 = "1"
                Else
                    VW10 = "0"
                End If
                If cbW11.Checked = True Then
                    VW11 = "1"
                Else
                    VW11 = "0"
                End If
                If cbW12.Checked = True Then
                    VW12 = "1"
                Else
                    VW12 = "0"
                End If
                If cbW13.Checked = True Then
                    VW13 = "1"
                Else
                    VW13 = "0"
                End If
                If cbW14.Checked = True Then
                    VW14 = "1"
                Else
                    VW14 = "0"
                End If
                If cbW15.Checked = True Then
                    VW15 = "1"
                Else
                    VW15 = "0"
                End If
                If cbW16.Checked = True Then
                    VW16 = "1"
                Else
                    VW16 = "0"
                End If
                If cbW17.Checked = True Then
                    VW17 = "1"
                Else
                    VW17 = "0"
                End If
                If cbW18.Checked = True Then
                    VW18 = "1"
                Else
                    VW18 = "0"
                End If
                If cbW19.Checked = True Then
                    VW19 = "1"
                Else
                    VW19 = "0"
                End If
                If cbW20.Checked = True Then
                    VW20 = "1"
                Else
                    VW20 = "0"
                End If
                If cbW21.Checked = True Then
                    VW21 = "1"
                Else
                    VW21 = "0"
                End If
                If cbW22.Checked = True Then
                    VW22 = "1"
                Else
                    VW22 = "0"
                End If
                If cbW23.Checked = True Then
                    VW23 = "1"
                Else
                    VW23 = "0"
                End If
                If cbW24.Checked = True Then
                    VW24 = "1"
                Else
                    VW24 = "0"
                End If
                If cbW25.Checked = True Then
                    VW25 = "1"
                Else
                    VW25 = "0"
                End If
                If cbW26.Checked = True Then
                    VW26 = "1"
                Else
                    VW26 = "0"
                End If
                If cbW27.Checked = True Then
                    VW27 = "1"
                Else
                    VW27 = "0"
                End If
                If cbW28.Checked = True Then
                    VW28 = "1"
                Else
                    VW28 = "0"
                End If
                If cbW29.Checked = True Then
                    VW29 = "1"
                Else
                    VW29 = "0"
                End If
                If cbW30.Checked = True Then
                    VW30 = "1"
                Else
                    VW30 = "0"
                End If
                If cbW31.Checked = True Then
                    VW31 = "1"
                Else
                    VW31 = "0"
                End If
                If cbW32.Checked = True Then
                    VW32 = "1"
                Else
                    VW32 = "0"
                End If
                If cbW33.Checked = True Then
                    VW33 = "1"
                Else
                    VW33 = "0"
                End If
                If cbW34.Checked = True Then
                    VW34 = "1"
                Else
                    VW34 = "0"
                End If
                If cbW35.Checked = True Then
                    VW35 = "1"
                Else
                    VW35 = "0"
                End If
                If cbW36.Checked = True Then
                    VW36 = "1"
                Else
                    VW36 = "0"
                End If
                If cbW37.Checked = True Then
                    VW37 = "1"
                Else
                    VW37 = "0"
                End If
                If cbW38.Checked = True Then
                    VW38 = "1"
                Else
                    VW38 = "0"
                End If
                If cbW39.Checked = True Then
                    VW39 = "1"
                Else
                    VW39 = "0"
                End If
                If cbW40.Checked = True Then
                    VW40 = "1"
                Else
                    VW40 = "0"
                End If
                If cbW41.Checked = True Then
                    VW41 = "1"
                Else
                    VW41 = "0"
                End If
                If cbW42.Checked = True Then
                    VW42 = "1"
                Else
                    VW42 = "0"
                End If
                If cbW43.Checked = True Then
                    VW43 = "1"
                Else
                    VW43 = "0"
                End If
                If cbW44.Checked = True Then
                    VW44 = "1"
                Else
                    VW44 = "0"
                End If
                If cbW45.Checked = True Then
                    VW45 = "1"
                Else
                    VW45 = "0"
                End If
                If cbW46.Checked = True Then
                    VW46 = "1"
                Else
                    VW46 = "0"
                End If
                If cbW47.Checked = True Then
                    VW47 = "1"
                Else
                    VW47 = "0"
                End If
                If cbW48.Checked = True Then
                    VW48 = "1"
                Else
                    VW48 = "0"
                End If
                If cbW49.Checked = True Then
                    VW49 = "1"
                Else
                    VW49 = "0"
                End If
                If cbW50.Checked = True Then
                    VW50 = "1"
                Else
                    VW50 = "0"
                End If
                If cbW51.Checked = True Then
                    VW51 = "1"
                Else
                    VW51 = "0"
                End If
                If cbW52.Checked = True Then
                    VW52 = "1"
                Else
                    VW52 = "0"
                End If
                If cbW53.Checked = True Then
                    VW53 = "1"
                Else
                    VW53 = "0"
                End If
                If cbW54.Checked = True Then
                    VW54 = "1"
                Else
                    VW54 = "0"
                End If
                If cbW55.Checked = True Then
                    VW55 = "1"
                Else
                    VW55 = "0"
                End If
                If cbW56.Checked = True Then
                    VW56 = "1"
                Else
                    VW56 = "0"
                End If
                If cbW57.Checked = True Then
                    VW57 = "1"
                Else
                    VW57 = "0"
                End If
                If cbW58.Checked = True Then
                    VW58 = "1"
                Else
                    VW58 = "0"
                End If
                If cbW59.Checked = True Then
                    VW59 = "1"
                Else
                    VW59 = "0"
                End If
                If cbW60.Checked = True Then
                    VW60 = "1"
                Else
                    VW60 = "0"
                End If
                If cbW61.Checked = True Then
                    VW61 = "1"
                Else
                    VW61 = "0"
                End If
                If cbW62.Checked = True Then
                    VW62 = "1"
                Else
                    VW62 = "0"
                End If
                If cbW63.Checked = True Then
                    VW63 = "1"
                Else
                    VW63 = "0"
                End If
                If cbW64.Checked = True Then
                    VW64 = "1"
                Else
                    VW64 = "0"
                End If
                If cbW65.Checked = True Then
                    VW65 = "1"
                Else
                    VW65 = "0"
                End If
                If cbW66.Checked = True Then
                    VW66 = "1"
                Else
                    VW66 = "0"
                End If
                If cbW67.Checked = True Then
                    VW67 = "1"
                Else
                    VW67 = "0"
                End If
                If cbW68.Checked = True Then
                    VW68 = "1"
                Else
                    VW68 = "0"
                End If
                If cbW69.Checked = True Then
                    VW69 = "1"
                Else
                    VW69 = "0"
                End If
                If cbW70.Checked = True Then
                    VW70 = "1"
                Else
                    VW70 = "0"
                End If
                If cbW71.Checked = True Then
                    VW71 = "1"
                Else
                    VW71 = "0"
                End If
                If cbW72.Checked = True Then
                    VW72 = "1"
                Else
                    VW72 = "0"
                End If
                If cbW73.Checked = True Then
                    VW73 = "1"
                Else
                    VW73 = "0"
                End If
                If cbW74.Checked = True Then
                    VW74 = "1"
                Else
                    VW74 = "0"
                End If
                If cbW75.Checked = True Then
                    VW75 = "1"
                Else
                    VW75 = "0"
                End If
                If cbW76.Checked = True Then
                    VW76 = "1"
                Else
                    VW76 = "0"
                End If
                If cbW77.Checked = True Then
                    VW77 = "1"
                Else
                    VW77 = "0"
                End If
                If cbW78.Checked = True Then
                    VW78 = "1"
                Else
                    VW78 = "0"
                End If
                If cbW79.Checked = True Then
                    VW79 = "1"
                Else
                    VW79 = "0"
                End If
                If cbW80.Checked = True Then
                    VW80 = "1"
                Else
                    VW80 = "0"
                End If
                If cbW81.Checked = True Then
                    VW81 = "1"
                Else
                    VW81 = "0"
                End If
                If cbW82.Checked = True Then
                    VW82 = "1"
                Else
                    VW82 = "0"
                End If
                If cbW83.Checked = True Then
                    VW83 = "1"
                Else
                    VW83 = "0"
                End If
                If cbW84.Checked = True Then
                    VW84 = "1"
                Else
                    VW84 = "0"
                End If
                If cbW85.Checked = True Then
                    VW85 = "1"
                Else
                    VW85 = "0"
                End If
                If cbW86.Checked = True Then
                    VW86 = "1"
                Else
                    VW86 = "0"
                End If
                If cbW87.Checked = True Then
                    VW87 = "1"
                Else
                    VW87 = "0"
                End If
                If cbW88.Checked = True Then
                    VW88 = "1"
                Else
                    VW88 = "0"
                End If
                If cbW89.Checked = True Then
                    VW89 = "1"
                Else
                    VW89 = "0"
                End If
                If cbW90.Checked = True Then
                    VW90 = "1"
                Else
                    VW90 = "0"
                End If
                If cbW91.Checked = True Then
                    VW91 = "1"
                Else
                    VW91 = "0"
                End If
                If cbW92.Checked = True Then
                    VW92 = "1"
                Else
                    VW92 = "0"
                End If
                If cbW93.Checked = True Then
                    VW93 = "1"
                Else
                    VW93 = "0"
                End If
                If cbW94.Checked = True Then
                    VW94 = "1"
                Else
                    VW94 = "0"
                End If
                If cbW95.Checked = True Then
                    VW95 = "1"
                Else
                    VW95 = "0"
                End If
                If cbW96.Checked = True Then
                    VW96 = "1"
                Else
                    VW96 = "0"
                End If
                If cbW97.Checked = True Then
                    VW97 = "1"
                Else
                    VW97 = "0"
                End If
                If cbW98.Checked = True Then
                    VW98 = "1"
                Else
                    VW98 = "0"
                End If
                If cbW99.Checked = True Then
                    VW99 = "1"
                Else
                    VW99 = "0"
                End If
                If cbW100.Checked = True Then
                    VW100 = "1"
                Else
                    VW100 = "0"
                End If
                If cbW101.Checked = True Then
                    VW101 = "1"
                Else
                    VW101 = "0"
                End If
                If cbW102.Checked = True Then
                    VW102 = "1"
                Else
                    VW102 = "0"
                End If
                If cbW103.Checked = True Then
                    VW103 = "1"
                Else
                    VW103 = "0"
                End If
                If cbW104.Checked = True Then
                    VW104 = "1"
                Else
                    VW104 = "0"
                End If
                'If Not ((Rotation.Text = "" Or DoneRotasi.Text = "0")) Then ' And ((QtyConvert.Text <> lbLastPriceEmp.Text) Or (QtyAreal.Text <> lbLastPriceContractor.Text)) Then
                SQLString = "EXEC S_MsSchedulePNMNUpdate " + QuotedStr(ddlType.SelectedValue) + ", " + QuotedStr(lbCode.Text) + ", " + QuotedStr(DoneRotasi.Text) + "," + _
                VW01 + ", " + VW02 + ", " + VW03 + ", " + VW04 + ", " + VW05 + ", " + VW06 + ", " + VW07 + ", " + VW08 + ", " + VW09 + ", " + VW10 + "," + _
                VW11 + ", " + VW12 + ", " + VW13 + ", " + VW14 + ", " + VW15 + ", " + VW16 + ", " + VW17 + ", " + VW18 + ", " + VW19 + ", " + VW20 + "," + _
                VW21 + ", " + VW22 + ", " + VW23 + ", " + VW24 + ", " + VW25 + ", " + VW26 + ", " + VW27 + ", " + VW28 + ", " + VW29 + ", " + VW30 + "," + _
                VW31 + ", " + VW32 + ", " + VW33 + ", " + VW34 + ", " + VW35 + ", " + VW36 + ", " + VW37 + ", " + VW38 + ", " + VW39 + ", " + VW40 + "," + _
                VW41 + ", " + VW42 + ", " + VW43 + ", " + VW44 + ", " + VW45 + ", " + VW46 + ", " + VW47 + ", " + VW48 + ", " + VW49 + ", " + VW50 + "," + _
                VW51 + ", " + VW52 + ", " + VW53 + ", " + VW54 + ", " + VW55 + ", " + VW56 + ", " + VW57 + ", " + VW58 + ", " + VW59 + ", " + VW60 + "," + _
                VW61 + ", " + VW62 + ", " + VW63 + ", " + VW64 + ", " + VW65 + ", " + VW66 + ", " + VW67 + ", " + VW68 + ", " + VW69 + ", " + VW70 + "," + _
                VW71 + ", " + VW72 + ", " + VW73 + ", " + VW74 + ", " + VW75 + ", " + VW76 + ", " + VW77 + ", " + VW78 + ", " + VW79 + ", " + VW80 + "," + _
                VW81 + ", " + VW82 + ", " + VW83 + ", " + VW84 + ", " + VW85 + ", " + VW86 + ", " + VW87 + ", " + VW88 + ", " + VW89 + ", " + VW90 + "," + _
                VW91 + ", " + VW92 + ", " + VW93 + ", " + VW94 + ", " + VW95 + ", " + VW96 + ", " + VW97 + ", " + VW98 + ", " + VW99 + ", " + VW100 + "," + _
                VW101 + ", " + VW102 + ", " + VW103 + ", " + VW104 + "," + _
                QuotedStr(ViewState("UserId").ToString)
                SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            Next
            BindData()
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
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
            lbstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Public Sub ExportGridToExcel(ByVal filenamevalue As String)
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname As String
        worksheetname = Left(filenamevalue, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + filenamevalue + ".xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        GridExport.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Protected Sub UpdateWeek(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbx As CheckBox = CType(sender, CheckBox)
        Dim rowIndex As Integer = Convert.ToInt32(cbx.Attributes("RowIndex"))
        Dim GVR As GridViewRow
        Dim DoneRotasi As Label
        Dim cbW01, cbW02, cbW03, cbW04, cbW05, cbW06, cbW07, cbW08, cbW09, cbW10 As CheckBox
        Dim cbW11, cbW12, cbW13, cbW14, cbW15, cbW16, cbW17, cbW18, cbW19, cbW20 As CheckBox
        Dim cbW21, cbW22, cbW23, cbW24, cbW25, cbW26, cbW27, cbW28, cbW29, cbW30 As CheckBox
        Dim cbW31, cbW32, cbW33, cbW34, cbW35, cbW36, cbW37, cbW38, cbW39, cbW40 As CheckBox
        Dim cbW41, cbW42, cbW43, cbW44, cbW45, cbW46, cbW47, cbW48, cbW49, cbW50 As CheckBox
        Dim cbW51, cbW52, cbW53, cbW54, cbW55, cbW56, cbW57, cbW58, cbW59, cbW60 As CheckBox
        Dim cbW61, cbW62, cbW63, cbW64, cbW65, cbW66, cbW67, cbW68, cbW69, cbW70 As CheckBox
        Dim cbW71, cbW72, cbW73, cbW74, cbW75, cbW76, cbW77, cbW78, cbW79, cbW80 As CheckBox
        Dim cbW81, cbW82, cbW83, cbW84, cbW85, cbW86, cbW87, cbW88, cbW89, cbW90 As CheckBox
        Dim cbW91, cbW92, cbW93, cbW94, cbW95, cbW96, cbW97, cbW98, cbW99, cbW100 As CheckBox
        Dim cbW101, cbW102, cbW103, cbW104 As CheckBox
        Dim VW01, VW02, VW03, VW04, VW05, VW06, VW07, VW08, VW09, VW10 As Integer
        Dim VW11, VW12, VW13, VW14, VW15, VW16, VW17, VW18, VW19, VW20 As Integer
        Dim VW21, VW22, VW23, VW24, VW25, VW26, VW27, VW28, VW29, VW30 As Integer
        Dim VW31, VW32, VW33, VW34, VW35, VW36, VW37, VW38, VW39, VW40 As Integer
        Dim VW41, VW42, VW43, VW44, VW45, VW46, VW47, VW48, VW49, VW50 As Integer
        Dim VW51, VW52, VW53, VW54, VW55, VW56, VW57, VW58, VW59, VW60 As Integer
        Dim VW61, VW62, VW63, VW64, VW65, VW66, VW67, VW68, VW69, VW70 As Integer
        Dim VW71, VW72, VW73, VW74, VW75, VW76, VW77, VW78, VW79, VW80 As Integer
        Dim VW81, VW82, VW83, VW84, VW85, VW86, VW87, VW88, VW89, VW90 As Integer
        Dim VW91, VW92, VW93, VW94, VW95, VW96, VW97, VW98, VW99, VW100 As Integer
        Dim VW101, VW102, VW103, VW104, Total As Integer
        GVR = DataGrid.Rows(rowIndex)
        'lbCode = GVR.FindControl("JobCode")
        DoneRotasi = GVR.FindControl("DoneRotasi")
        cbW01 = GVR.FindControl("cbW01")
        cbW02 = GVR.FindControl("cbW02")
        cbW03 = GVR.FindControl("cbW03")
        cbW04 = GVR.FindControl("cbW04")
        cbW05 = GVR.FindControl("cbW05")
        cbW06 = GVR.FindControl("cbW06")
        cbW07 = GVR.FindControl("cbW07")
        cbW08 = GVR.FindControl("cbW08")
        cbW09 = GVR.FindControl("cbW09")
        cbW10 = GVR.FindControl("cbW10")
        cbW11 = GVR.FindControl("cbW11")
        cbW12 = GVR.FindControl("cbW12")
        cbW13 = GVR.FindControl("cbW13")
        cbW14 = GVR.FindControl("cbW14")
        cbW15 = GVR.FindControl("cbW15")
        cbW16 = GVR.FindControl("cbW16")
        cbW17 = GVR.FindControl("cbW17")
        cbW18 = GVR.FindControl("cbW18")
        cbW19 = GVR.FindControl("cbW19")
        cbW20 = GVR.FindControl("cbW20")
        cbW21 = GVR.FindControl("cbW21")
        cbW22 = GVR.FindControl("cbW22")
        cbW23 = GVR.FindControl("cbW23")
        cbW24 = GVR.FindControl("cbW24")
        cbW25 = GVR.FindControl("cbW25")
        cbW26 = GVR.FindControl("cbW26")
        cbW27 = GVR.FindControl("cbW27")
        cbW28 = GVR.FindControl("cbW28")
        cbW29 = GVR.FindControl("cbW29")
        cbW30 = GVR.FindControl("cbW30")
        cbW31 = GVR.FindControl("cbW31")
        cbW32 = GVR.FindControl("cbW32")
        cbW33 = GVR.FindControl("cbW33")
        cbW34 = GVR.FindControl("cbW34")
        cbW35 = GVR.FindControl("cbW35")
        cbW36 = GVR.FindControl("cbW36")
        cbW37 = GVR.FindControl("cbW37")
        cbW38 = GVR.FindControl("cbW38")
        cbW39 = GVR.FindControl("cbW39")
        cbW40 = GVR.FindControl("cbW40")
        cbW41 = GVR.FindControl("cbW41")
        cbW42 = GVR.FindControl("cbW42")
        cbW43 = GVR.FindControl("cbW43")
        cbW44 = GVR.FindControl("cbW44")
        cbW45 = GVR.FindControl("cbW45")
        cbW46 = GVR.FindControl("cbW46")
        cbW47 = GVR.FindControl("cbW47")
        cbW48 = GVR.FindControl("cbW48")
        cbW49 = GVR.FindControl("cbW49")
        cbW50 = GVR.FindControl("cbW50")
        cbW51 = GVR.FindControl("cbW51")
        cbW52 = GVR.FindControl("cbW52")
        cbW53 = GVR.FindControl("cbW53")
        cbW54 = GVR.FindControl("cbW54")
        cbW55 = GVR.FindControl("cbW55")
        cbW56 = GVR.FindControl("cbW56")
        cbW57 = GVR.FindControl("cbW57")
        cbW58 = GVR.FindControl("cbW58")
        cbW59 = GVR.FindControl("cbW59")
        cbW60 = GVR.FindControl("cbW60")
        cbW61 = GVR.FindControl("cbW61")
        cbW62 = GVR.FindControl("cbW62")
        cbW63 = GVR.FindControl("cbW63")
        cbW64 = GVR.FindControl("cbW64")
        cbW65 = GVR.FindControl("cbW65")
        cbW66 = GVR.FindControl("cbW66")
        cbW67 = GVR.FindControl("cbW67")
        cbW68 = GVR.FindControl("cbW68")
        cbW69 = GVR.FindControl("cbW69")
        cbW70 = GVR.FindControl("cbW70")
        cbW71 = GVR.FindControl("cbW71")
        cbW72 = GVR.FindControl("cbW72")
        cbW73 = GVR.FindControl("cbW73")
        cbW74 = GVR.FindControl("cbW74")
        cbW75 = GVR.FindControl("cbW75")
        cbW76 = GVR.FindControl("cbW76")
        cbW77 = GVR.FindControl("cbW77")
        cbW78 = GVR.FindControl("cbW78")
        cbW79 = GVR.FindControl("cbW79")
        cbW80 = GVR.FindControl("cbW80")
        cbW81 = GVR.FindControl("cbW81")
        cbW82 = GVR.FindControl("cbW82")
        cbW83 = GVR.FindControl("cbW83")
        cbW84 = GVR.FindControl("cbW84")
        cbW85 = GVR.FindControl("cbW85")
        cbW86 = GVR.FindControl("cbW86")
        cbW87 = GVR.FindControl("cbW87")
        cbW88 = GVR.FindControl("cbW88")
        cbW89 = GVR.FindControl("cbW89")
        cbW90 = GVR.FindControl("cbW90")
        cbW91 = GVR.FindControl("cbW91")
        cbW92 = GVR.FindControl("cbW92")
        cbW93 = GVR.FindControl("cbW93")
        cbW94 = GVR.FindControl("cbW94")
        cbW95 = GVR.FindControl("cbW95")
        cbW96 = GVR.FindControl("cbW96")
        cbW97 = GVR.FindControl("cbW97")
        cbW98 = GVR.FindControl("cbW98")
        cbW99 = GVR.FindControl("cbW99")
        cbW100 = GVR.FindControl("cbW100")
        cbW101 = GVR.FindControl("cbW101")
        cbW102 = GVR.FindControl("cbW102")
        cbW103 = GVR.FindControl("cbW103")
        cbW104 = GVR.FindControl("cbW104")
        If cbW01.Checked Then
            VW01 = 1
        Else
            VW01 = 0
        End If
        If cbW02.Checked Then
            VW02 = 1
        Else
            VW02 = 0
        End If
        If cbW03.Checked Then
            VW03 = 1
        Else
            VW03 = 0
        End If
        If cbW04.Checked Then
            VW04 = 1
        Else
            VW04 = 0
        End If
        If cbW05.Checked Then
            VW05 = 1
        Else
            VW05 = 0
        End If
        If cbW06.Checked Then
            VW06 = 1
        Else
            VW06 = 0
        End If
        If cbW07.Checked Then
            VW07 = 1
        Else
            VW07 = 0
        End If
        If cbW08.Checked Then
            VW08 = 1
        Else
            VW08 = 0
        End If
        If cbW09.Checked Then
            VW09 = 1
        Else
            VW09 = 0
        End If
        If cbW10.Checked Then
            VW10 = 1
        Else
            VW10 = 0
        End If
        If cbW11.Checked Then
            VW11 = 1
        Else
            VW11 = 0
        End If
        If cbW12.Checked Then
            VW12 = 1
        Else
            VW12 = 0
        End If
        If cbW13.Checked Then
            VW13 = 1
        Else
            VW13 = 0
        End If
        If cbW14.Checked Then
            VW14 = 1
        Else
            VW14 = 0
        End If
        If cbW15.Checked Then
            VW15 = 1
        Else
            VW15 = 0
        End If
        If cbW16.Checked Then
            VW16 = 1
        Else
            VW16 = 0
        End If
        If cbW17.Checked Then
            VW17 = 1
        Else
            VW17 = 0
        End If
        If cbW18.Checked Then
            VW18 = 1
        Else
            VW18 = 0
        End If
        If cbW19.Checked Then
            VW19 = 1
        Else
            VW19 = 0
        End If
        If cbW20.Checked Then
            VW20 = 1
        Else
            VW20 = 0
        End If
        If cbW21.Checked Then
            VW21 = 1
        Else
            VW21 = 0
        End If
        If cbW22.Checked Then
            VW22 = 1
        Else
            VW22 = 0
        End If
        If cbW23.Checked Then
            VW23 = 1
        Else
            VW23 = 0
        End If
        If cbW24.Checked Then
            VW24 = 1
        Else
            VW24 = 0
        End If
        If cbW25.Checked Then
            VW25 = 1
        Else
            VW25 = 0
        End If
        If cbW26.Checked Then
            VW26 = 1
        Else
            VW26 = 0
        End If
        If cbW27.Checked Then
            VW27 = 1
        Else
            VW27 = 0
        End If
        If cbW28.Checked Then
            VW28 = 1
        Else
            VW28 = 0
        End If
        If cbW29.Checked Then
            VW29 = 1
        Else
            VW29 = 0
        End If
        If cbW30.Checked Then
            VW30 = 1
        Else
            VW30 = 0
        End If
        If cbW31.Checked Then
            VW31 = 1
        Else
            VW31 = 0
        End If
        If cbW32.Checked Then
            VW32 = 1
        Else
            VW32 = 0
        End If
        If cbW33.Checked Then
            VW33 = 1
        Else
            VW33 = 0
        End If
        If cbW34.Checked Then
            VW34 = 1
        Else
            VW34 = 0
        End If
        If cbW35.Checked Then
            VW35 = 1
        Else
            VW35 = 0
        End If
        If cbW36.Checked Then
            VW36 = 1
        Else
            VW36 = 0
        End If
        If cbW37.Checked Then
            VW37 = 1
        Else
            VW37 = 0
        End If
        If cbW38.Checked Then
            VW38 = 1
        Else
            VW38 = 0
        End If
        If cbW39.Checked Then
            VW39 = 1
        Else
            VW39 = 0
        End If
        If cbW40.Checked Then
            VW40 = 1
        Else
            VW40 = 0
        End If
        If cbW41.Checked Then
            VW41 = 1
        Else
            VW41 = 0
        End If
        If cbW42.Checked Then
            VW42 = 1
        Else
            VW42 = 0
        End If
        If cbW43.Checked Then
            VW43 = 1
        Else
            VW43 = 0
        End If
        If cbW44.Checked Then
            VW44 = 1
        Else
            VW44 = 0
        End If
        If cbW45.Checked Then
            VW45 = 1
        Else
            VW45 = 0
        End If
        If cbW46.Checked Then
            VW46 = 1
        Else
            VW46 = 0
        End If
        If cbW47.Checked Then
            VW47 = 1
        Else
            VW47 = 0
        End If
        If cbW48.Checked Then
            VW48 = 1
        Else
            VW48 = 0
        End If
        If cbW49.Checked Then
            VW49 = 1
        Else
            VW49 = 0
        End If
        If cbW50.Checked Then
            VW50 = 1
        Else
            VW50 = 0
        End If
        If cbW51.Checked Then
            VW51 = 1
        Else
            VW51 = 0
        End If
        If cbW52.Checked Then
            VW52 = 1
        Else
            VW52 = 0
        End If
        If cbW53.Checked Then
            VW53 = 1
        Else
            VW53 = 0
        End If
        If cbW54.Checked Then
            VW54 = 1
        Else
            VW54 = 0
        End If
        If cbW55.Checked Then
            VW55 = 1
        Else
            VW55 = 0
        End If
        If cbW56.Checked Then
            VW56 = 1
        Else
            VW56 = 0
        End If
        If cbW57.Checked Then
            VW57 = 1
        Else
            VW57 = 0
        End If
        If cbW58.Checked Then
            VW58 = 1
        Else
            VW58 = 0
        End If
        If cbW59.Checked Then
            VW59 = 1
        Else
            VW59 = 0
        End If
        If cbW60.Checked Then
            VW60 = 1
        Else
            VW60 = 0
        End If
        If cbW61.Checked Then
            VW61 = 1
        Else
            VW61 = 0
        End If
        If cbW62.Checked Then
            VW62 = 1
        Else
            VW62 = 0
        End If
        If cbW63.Checked Then
            VW63 = 1
        Else
            VW63 = 0
        End If
        If cbW64.Checked Then
            VW64 = 1
        Else
            VW64 = 0
        End If
        If cbW65.Checked Then
            VW65 = 1
        Else
            VW65 = 0
        End If
        If cbW66.Checked Then
            VW66 = 1
        Else
            VW66 = 0
        End If
        If cbW67.Checked Then
            VW67 = 1
        Else
            VW67 = 0
        End If
        If cbW68.Checked Then
            VW68 = 1
        Else
            VW68 = 0
        End If
        If cbW69.Checked Then
            VW69 = 1
        Else
            VW69 = 0
        End If
        If cbW70.Checked Then
            VW70 = 1
        Else
            VW70 = 0
        End If
        If cbW71.Checked Then
            VW71 = 1
        Else
            VW71 = 0
        End If
        If cbW72.Checked Then
            VW72 = 1
        Else
            VW72 = 0
        End If
        If cbW73.Checked Then
            VW73 = 1
        Else
            VW73 = 0
        End If
        If cbW74.Checked Then
            VW74 = 1
        Else
            VW74 = 0
        End If
        If cbW75.Checked Then
            VW75 = 1
        Else
            VW75 = 0
        End If
        If cbW76.Checked Then
            VW76 = 1
        Else
            VW76 = 0
        End If
        If cbW77.Checked Then
            VW77 = 1
        Else
            VW77 = 0
        End If
        If cbW78.Checked Then
            VW78 = 1
        Else
            VW78 = 0
        End If
        If cbW79.Checked Then
            VW79 = 1
        Else
            VW79 = 0
        End If
        If cbW80.Checked Then
            VW80 = 1
        Else
            VW80 = 0
        End If
        If cbW81.Checked Then
            VW81 = 1
        Else
            VW81 = 0
        End If
        If cbW82.Checked Then
            VW82 = 1
        Else
            VW82 = 0
        End If
        If cbW83.Checked Then
            VW83 = 1
        Else
            VW83 = 0
        End If
        If cbW84.Checked Then
            VW84 = 1
        Else
            VW84 = 0
        End If
        If cbW85.Checked Then
            VW85 = 1
        Else
            VW85 = 0
        End If
        If cbW86.Checked Then
            VW86 = 1
        Else
            VW86 = 0
        End If
        If cbW87.Checked Then
            VW87 = 1
        Else
            VW87 = 0
        End If
        If cbW88.Checked Then
            VW88 = 1
        Else
            VW88 = 0
        End If
        If cbW89.Checked Then
            VW89 = 1
        Else
            VW89 = 0
        End If
        If cbW90.Checked Then
            VW90 = 1
        Else
            VW90 = 0
        End If
        If cbW91.Checked Then
            VW91 = 1
        Else
            VW91 = 0
        End If
        If cbW92.Checked Then
            VW92 = 1
        Else
            VW92 = 0
        End If
        If cbW93.Checked Then
            VW93 = 1
        Else
            VW93 = 0
        End If
        If cbW94.Checked Then
            VW94 = 1
        Else
            VW94 = 0
        End If
        If cbW95.Checked Then
            VW95 = 1
        Else
            VW95 = 0
        End If
        If cbW96.Checked Then
            VW96 = 1
        Else
            VW96 = 0
        End If
        If cbW97.Checked Then
            VW97 = 1
        Else
            VW97 = 0
        End If
        If cbW98.Checked Then
            VW98 = 1
        Else
            VW98 = 0
        End If
        If cbW99.Checked Then
            VW99 = 1
        Else
            VW99 = 0
        End If
        If cbW100.Checked Then
            VW100 = 1
        Else
            VW100 = 0
        End If
        If cbW101.Checked Then
            VW101 = 1
        Else
            VW101 = 0
        End If
        If cbW102.Checked Then
            VW102 = 1
        Else
            VW102 = 0
        End If
        If cbW103.Checked Then
            VW103 = 1
        Else
            VW103 = 0
        End If
        If cbW104.Checked Then
            VW104 = 1
        Else
            VW104 = 0
        End If
        Total = VW01 + VW02 + VW03 + VW04 + VW05 + VW06 + VW07 + VW08 + VW09 + VW10 + VW11 + VW12 + VW13 + VW14 + VW15 + VW16 + VW17 + VW18 + VW19 + VW20 + VW21 + VW22 + VW23 + VW24 + VW25 + VW26 + VW27 + VW28 + VW29 + VW30 + VW31 + VW32 + VW33 + VW34 + VW35 + VW36 + VW37 + VW38 + VW39 + VW40 + VW41 + VW42 + VW43 + VW44 + VW45 + VW46 + VW47 + VW48 + VW49 + VW50 + VW51 + VW52 + VW53 + VW54 + VW55 + VW56 + VW57 + VW58 + VW59 + VW60 + VW61 + VW62 + VW63 + VW64 + VW65 + VW66 + VW67 + VW68 + VW69 + VW70 + VW71 + VW72 + VW73 + VW74 + VW75 + VW76 + VW77 + VW78 + VW79 + VW80 + VW81 + VW82 + VW83 + VW84 + VW85 + VW86 + VW87 + VW88 + VW89 + VW90 + VW91 + VW92 + VW93 + VW94 + VW95 + VW96 + VW97 + VW98 + VW99 + VW100 + VW101 + VW102 + VW103 + VW104
        DoneRotasi.Text = Total.ToString
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim tempDS As New DataSet()
            Dim StrFilter As String
            Try
                StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
                If StrFilter.Length > 5 Then
                    StrFilter = StrFilter.Remove(1, 5)
                    StrFilter = " And " + StrFilter
                End If
                tempDS = SQLExecuteQuery("EXEC S_MsSchedulePNMNExport " + QuotedStr(ddlType.SelectedValue) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
                GridExport.DataSource = tempDS.Tables(0)
                GridExport.DataBind()
                'GridExport.Visible = True
                ExportGridToExcel("SchedulePNMN")
            Catch ex As Exception
                Throw New Exception("Bind Data Error : " + ex.ToString)
            End Try

        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
            'lstatus.Text = "ddlCategory_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
  
    Protected Sub btnGoAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoAdd.Click
        Dim tempDS As New DataSet()
        Try

            dsGetSection.ConnectionString = ViewState("DBConnection")
            tempDS = SQLExecuteQuery("EXEC S_MsSchedulePNMNView " + QuotedStr(tbAddSchedule.Text.Trim) + " , ''", ViewState("DBConnection").ToString)
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()
            FillCombo(ddlType, "Select distinct ScheduleType from MsSchedulePNMNTemplate", False, "ScheduleType", "ScheduleType", ViewState("DBConnection").ToString)
            ddlType.SelectedValue = tbAddSchedule.Text.Trim
            FillCombo(ddlCopyScheduleType, "Select distinct ScheduleType from MsSchedulePNMNTemplate", False, "ScheduleType", "ScheduleType", ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("btnGoAdd_Click Error : " + ex.ToString)
        End Try

        'DataGrid.PageIndex = 0
        'DataGrid.EditIndex = -1
        'BindData()

    End Sub

    Protected Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelete.Click
        Dim SQLDelete As String
        Try
            SQLDelete = "EXEC S_MsSchedulePNMNDelete " + QuotedStr(ddlType.SelectedValue)
            SQLExecuteNonQuery(SQLDelete, ViewState("DBConnection").ToString)
            FillCombo(ddlType, "Select distinct ScheduleType from MsSchedulePNMNTemplate", False, "ScheduleType", "ScheduleType", ViewState("DBConnection").ToString)
            BindData()
            FillCombo(ddlCopyScheduleType, "Select distinct ScheduleType from MsSchedulePNMNTemplate", False, "ScheduleType", "ScheduleType", ViewState("DBConnection").ToString)
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub btnGoCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoCopy.Click
        Dim SQLCopy As String
        Try
            If ddlCopyScheduleType.SelectedValue = ddlType.SelectedValue Then
                lbstatus.Text = "Schedule Type to copy cannot select " + QuotedStr(ddlType.SelectedValue)
                Exit Sub
            End If
            If ddlCopyType.SelectedValue = "From" Then
                SQLCopy = "EXEC S_MsSchedulePNMNCopy " + QuotedStr(ddlCopyScheduleType.SelectedValue) + ", " + QuotedStr(ddlType.SelectedValue)
            Else
                SQLCopy = "EXEC S_MsSchedulePNMNCopy " + QuotedStr(ddlType.SelectedValue) + ", " + QuotedStr(ddlCopyScheduleType.SelectedValue)
            End If

            SQLExecuteNonQuery(SQLCopy, ViewState("DBConnection").ToString)
            BindData()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
End Class
