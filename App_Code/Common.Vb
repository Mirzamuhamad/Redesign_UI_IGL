Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.Data

Public Module Common
    Function QuotedStr(ByVal Str As String) As String
        Try
            Return "'" & Str.Replace("'", "''") & "'"
        Catch ex As Exception
            Throw New Exception("query str error : " & ex.ToString)
        End Try
    End Function

    Function SelectedGrid(ByRef GV As GridView, ByVal cbName As String, ByVal lbName As String, ByVal ColumnName As String) As String
        Dim cb As CheckBox
        Dim lb As Label
        Dim GVR As GridViewRow
        Dim Str1 As String
        Dim First As Boolean
        Try
            Str1 = ""
            First = True

            For Each GVR In GV.Rows
                cb = GVR.FindControl(cbName)
                lb = GVR.FindControl(lbName)
                If cb.Checked Then
                    If Not lb Is Nothing Then
                        If First Then
                            Str1 = " AND " + ColumnName + " In(''" + lb.Text + "''"
                            First = False
                        Else
                            Str1 = Str1 + ",''" + lb.Text + "''"
                        End If
                    End If
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

    Function SelectedGridDev(ByRef GV As ASPxGridView, ByVal ColumnName As String) As String
        Dim Str1 As String
        Dim First As Boolean
        Dim i As Integer
        Dim Result As List(Of Object)
        Try
            Str1 = ""
            First = True

            Result = GV.GetSelectedFieldValues("CODE")

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

    Function CFloat(ByVal Str As String) As Double
        Try
            If IsNumeric(Str.Replace(",", "")) Then
                Return CDbl(Str.Replace(",", ""))
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("query str error : " & ex.ToString)
        End Try
    End Function

    Function GetTotalSum(ByVal dt As DataTable, ByVal FieldName As String) As Double
        Dim result As Double
        Try
            result = 0
            For Each dr As DataRow In dt.Rows()
                If Not (dr.RowState = DataRowState.Deleted) Then
                    result = result + CFloat(dr(FieldName).ToString)
                End If
            Next
            Return result
        Catch ex As Exception
            Throw New Exception("query str error : " & ex.ToString)
        End Try
    End Function

    Function GetCountRecord(ByVal dt As DataTable, Optional ByVal FieldName As String = "") As Integer
        Dim result As Double
        Try
            result = 0
            For Each dr As DataRow In dt.Rows()
                If Not (dr.RowState = DataRowState.Deleted) Then
                    If FieldName = "" Then
                        result = result + 1
                    Else
                        If TrimStr(dr(FieldName).ToString) <> "" Then
                            result = result + 1
                        End If
                    End If
                End If
            Next
            Return result
        Catch ex As Exception
            Throw New Exception("GetCountRecord error : " & ex.ToString)
        End Try
    End Function

    Sub ShowGridViewIfEmpty(ByVal source As DataTable, ByVal gv As GridView)
        Dim columnsCount As Integer
        'Dim S As String = ""
        Try
            source.Rows.Add(source.NewRow()) ' create a new blank row to the DataTable
            'Bind the DataTable which contain a blank row to the GridView
            gv.DataSource = source
            gv.DataBind()
            'Get the total number of columns in the GridView to know what the Column Span should be
            columnsCount = gv.Columns.Count
            'gv.Rows(0).Cells.Clear() 'clear all the cells in the row
            'gv.Rows(0).Cells.Add(New TableCell()) 'add a new blank cell
            'gv.Rows(0).Cells(0).ColumnSpan = columnsCount 'set the column span to the new added cell

            'gv.Rows(0).Cells.clear
            gv.Rows(0).Cells(columnsCount - 1).Visible = False

            'You can set the styles here
            'gv.Rows(0).Cells(0).HorizontalAlign = HorizontalAlign.Center
            'gv.Rows(0).Cells(0).ForeColor = System.Drawing.Color.Red
            'gv.Rows(0).Cells(0).Font.Bold = True
            'set No Results found to the new added cell
            'gv.Rows(0).Cells(0).Text = "NO RESULT FOUND!"
        Catch ex As Exception
            Throw New Exception("  ShowGridViewIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindGridDt(ByVal source As DataTable, ByVal gv As GridView)
        Dim IsEmpty As Boolean
        Dim DtTemp As DataTable
        Dim dr As DataRow()
        Try
            IsEmpty = False
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                gv.DataSource = DtTemp
            Else
                gv.DataSource = source
            End If
            gv.DataBind()
            gv.Columns(0).Visible = Not IsEmpty
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Function MessageDlg(ByVal Message As String) As String
        Dim Result As String
        Try
            Message = Message.Replace("<br />", "\n")
            Message = Message.Replace("'", """")
            Result = "<script language='javascript'> {alert('" + Message + "')}</script>"
            Return Result
        Catch ex As Exception
            Throw New Exception("Message Dlg Erro : " + ex.ToString)
        End Try
    End Function

    'Function ConfirmDlg(ByVal Message As String) As String
    '    Dim Result As String
    '    Try
    '        Message = Message.Replace("'", """")
    '        'Result = "<script language='javascript'> {alert('" + Message + "')}</script>"
    '        Result = "<script language='javascript'> { var r=confirm('" + Message + "')}</script>"
    '        Return Result
    '    Catch ex As Exception
    '        Throw New Exception("Message Dlg Erro : " + ex.ToString)
    '    End Try
    'End Function

    Function ChangeQuoteNull(ByVal SQLString As String) As String
        Try
            SQLString = SQLString.Replace(",'',", ", NULL ,")
            SQLString = SQLString.Replace(", '',", ", NULL ,")
            SQLString = SQLString.Replace(",'' ,", ", NULL ,")
            SQLString = SQLString.Replace(", '' ,", ", NULL ,")
            SQLString = SQLString.Replace("=''", "= NULL")
            SQLString = SQLString.Replace("= ''", "= NULL")
            Return SQLString
        Catch ex As Exception
            Throw New Exception("ChangeQuoteNull Error : " + ex.ToString)
        End Try
    End Function

    ' dipakai di assign pilihan group by
    Sub SelectGroupBy(ByRef PrmGroupBy As DropDownList, ByVal PrmAssign As Label, ByVal PrmAvail As Label)
        Dim str1 As String = PrmGroupBy.Items(0).Text
        Dim str2 As String = PrmGroupBy.Items(1).Text
        If (PrmGroupBy.Text = str1) Then
            PrmAssign.Text = str2 + " Assigned"
            PrmAvail.Text = str2 + " Available"
        Else
            PrmAssign.Text = str1 + " Assigned"
            PrmAvail.Text = str1 + " Available"
        End If
    End Sub

    Public Function IsTime(ByVal Input As String) As Boolean
        Dim Hour, Mins, DoubleDot As String
        Try
            If Input.Length <> 5 Then
                Return False
            End If

            Hour = Input.Substring(0, 2)
            Mins = Input.Substring(3, 2)
            DoubleDot = Input.Substring(2, 1)
            If DoubleDot <> ":" Then
                Return False
            End If
            If IsNumeric(Hour) = False Then
                Return False
            End If
            If IsNumeric(Mins) = False Then
                Return False
            End If
            If CInt(Hour) > 24 Then
                Return False
            End If
            If CInt(Mins) > 59 Then
                Return False
            End If

            Return True

        Catch ex As Exception
            Throw New Exception("IsTime function Error : " + ex.ToString)
        End Try
    End Function

    'Public Sub FillCombo(ByRef DDL As DropDownList, ByVal SqlString As String, ByVal Nullable As Boolean, ByVal SelectValue As String, ByVal SelectText As String, Optional ByVal DBConnection As String = "Nothing")
    '    Dim DS As DataSet
    '    Dim Dr As DataRow
    '    Dim LI As ListItem
    '    Try
    '        DS = SQLExecuteQuery(SqlString, DBConnection)

    '        If DDL.Items.Count > 0 Then
    '            DDL.Items.Clear()
    '        End If

    '        If Nullable Then
    '            LI = New ListItem("Choose One", "")
    '            DDL.Items.Add(LI)
    '        End If
    '        For Each Dr In DS.Tables(0).Rows
    '            LI = New ListItem(Dr(SelectText).ToString, Dr(SelectValue).ToString)
    '            DDL.Items.Add(LI)
    '        Next
    '    Catch ex As Exception
    '        Throw New Exception("Fill Combo Error : " + ex.ToString)
    '    End Try
    'End Sub

    Public Sub FillCombo(ByRef DDL As DropDownList, ByVal SqlString As String, ByVal Nullable As Boolean, ByVal SelectValue As String, ByVal SelectText As String, ByVal DBConnection As String)
        Dim DS As SqlDataReader
        Dim LI As ListItem
        Try
            DS = SQLExecuteReader(SqlString, DBConnection)
            If DDL.Items.Count > 0 Then
                DDL.Items.Clear()
            End If
            If Nullable Then
                LI = New ListItem("Choose One", "")
                DDL.Items.Add(LI)
            End If
            While DS.Read
                LI = New ListItem(DS(SelectText).ToString, DS(SelectValue).ToString)
                DDL.Items.Add(LI)
            End While
            DS.Close()
        Catch ex As Exception
            Throw New Exception("Fill Combo Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub FillComboActive(ByRef DDL As DropDownList, ByVal SqlString As String, ByVal Nullable As Boolean, ByVal SelectValue As String, ByVal SelectText As String, ByVal DBConnection As String)
        Dim DS As SqlDataReader
        Dim LI As ListItem
        Try
            DS = SQLExecuteReader(SqlString, DBConnection)
            If DDL.Items.Count > 0 Then
                DDL.Items.Clear()
            End If
            If Nullable Then
                LI = New ListItem("Choose One", "")
                DDL.Items.Add(LI)
            End If
            While DS.Read
                If DS("FgActive").ToString = "Y" Then
                    LI = New ListItem(DS(SelectText).ToString, DS(SelectValue).ToString)
                    DDL.Items.Add(LI)
                End If
            End While
            DS.Close()
        Catch ex As Exception
            Throw New Exception("Fill Combo Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub FillComboAll(ByRef DDL As DropDownList, ByVal SqlString As String, ByVal Nullable As Boolean, ByVal SelectValue As String, ByVal SelectText As String, ByVal DBConnection As String)
        Dim DS As SqlDataReader
        Dim LI, L2 As ListItem
        Try
            DS = SQLExecuteReader(SqlString, DBConnection)
            If DDL.Items.Count > 0 Then
                DDL.Items.Clear()
            End If
            If Nullable Then
                LI = New ListItem("Choose One", "")
                DDL.Items.Add(LI)
            End If
            L2 = New ListItem("All", "All")
            DDL.Items.Add(L2)

            While DS.Read
                LI = New ListItem(DS(SelectText).ToString, DS(SelectValue).ToString)
                DDL.Items.Add(LI)
            End While
            DS.Close()
        Catch ex As Exception
            Throw New Exception("Fill Combo Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub FillComboBlank(ByRef DDL As DropDownList)
        Dim LI As ListItem
        Try
            LI = New ListItem("Choose One", "")
            DDL.Items.Add(LI)
        Catch ex As Exception
            Throw New Exception("Fill Combo Blank Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub FillRange(ByRef DDL As DropDownList)
        Dim LI, L2 As ListItem
        Try
            DDL.Items.Clear()

            LI = New ListItem("All", "0")
            DDL.Items.Add(LI)

            L2 = New ListItem("This Month", "1")
            DDL.Items.Add(L2)

            LI = New ListItem("3 Month", "3")
            DDL.Items.Add(LI)

            LI = New ListItem("6 Month", "6")
            DDL.Items.Add(LI)

            LI = New ListItem("1 Year", "12")
            DDL.Items.Add(LI)
            DDL.SelectedIndex = DDL.Items.IndexOf(L2)
        Catch ex As Exception
            Throw New Exception("Fill Combo Error : " + ex.ToString)
        End Try
    End Sub

    'dipake di filter Ms
    Public Function GenerateFilterMs(ByVal Field1 As String, ByVal Field2 As String, ByVal Filter1 As String, ByVal Filter2 As String, ByVal Notasi As String) As String
        Dim StrFilter As String
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                If Filter2.Trim.Length > 0 Then
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%' " + _
                    Notasi + " " + Field2.Replace(" ", "_") + " like '%" + Filter2 + "%'"
                Else
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%'"
                End If
            Else
                StrFilter = ""
            End If

            If StrFilter <> "" Then
                StrFilter = " Where " + StrFilter
            End If
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterMs Error : " + ex.ToString)
        End Try
    End Function

    'dipake difilter Search Dialog dan transaksi
    Public Function GenerateFilterDlg(ByVal Field1 As String, ByVal Field2 As String, ByVal Filter1 As String, ByVal Filter2 As String, ByVal Notasi As String, Optional ByVal RangeData As String = "0", Optional ByVal FieldDate As String = "Transdate") As String
        Dim StrFilter As String
        Dim StrRangeDate As String
        Dim YY, MM, H As Integer
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                If Filter2.Trim.Length > 0 Then
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%' " + _
                    Notasi + " " + Field2.Replace(" ", "_") + " like '%" + Filter2 + "%'"
                Else
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%'"
                End If
            Else
                StrFilter = ""
            End If
            If RangeData = "0" Then
                StrRangeDate = ""
            Else
                YY = Year(Now())
                MM = Month(Now())
                H = (YY * 12) + MM - CInt(RangeData) + 1
                YY = Math.Floor(H / 12)
                MM = H - (YY * 12)
                If MM = 0 Then
                    MM = 12
                    YY = YY - 1
                End If
                StrRangeDate = FieldDate + " >= '" + YY.ToString.Trim + Right("0" + MM.ToString.Trim, 2) + "01'"
            End If
            If StrFilter.Length > 5 Then
                If StrRangeDate.Length > 5 Then
                    StrFilter = StrRangeDate + " AND ( " + StrFilter + " ) "
                Else
                    StrFilter = "( " + StrFilter + " )"
                End If
            Else
                StrFilter = StrRangeDate
            End If
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterDlg Error : " + ex.ToString)
        End Try
    End Function

    Public Function Generate5FilterDlg(ByVal Field1 As String, ByVal Field2 As String, ByVal Field3 As String, ByVal Field4 As String, ByVal Field5 As String, ByVal Filter1 As String, ByVal Filter2 As String, ByVal Filter3 As String, ByVal Filter4 As String, ByVal Filter5 As String, ByVal Notasi As String, ByVal Notasi3 As String, ByVal Notasi4 As String, ByVal Notasi5 As String) As String
        Dim StrFilter As String
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%'"
                If Filter2.Trim.Length > 0 Then
                    StrFilter = StrFilter + Notasi + " " + Field2.Replace(" ", "_") + " like '%" + Filter2 + "%'"
                End If
                If Filter3.Trim.Length > 0 Then
                    StrFilter = StrFilter + Notasi3 + " " + Field3.Replace(" ", "_") + " like '%" + Filter3 + "%'"
                End If
                If Filter4.Trim.Length > 0 Then
                    StrFilter = StrFilter + Notasi4 + " " + Field4.Replace(" ", "_") + " like '%" + Filter4 + "%'"
                End If
                If Filter5.Trim.Length > 0 Then
                    StrFilter = StrFilter + Notasi5 + " " + Field5.Replace(" ", "_") + " like '%" + Filter5 + "%'"
                End If
            Else
                StrFilter = ""
            End If
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterDlg Error : " + ex.ToString)
        End Try
    End Function

    ' dipake di form master
    Public Sub BindDataMaster(ByVal SqlString As String, ByRef GV As GridView, ByVal SortExpression As String, ByVal Connection As String)
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            'If Connection = "Nothing" Then
            '    tempDS = SQLExecuteQuery(SqlString)
            'Else
            tempDS = SQLExecuteQuery(SqlString, Connection)
            'End If
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GV)
            Else
                DV.Sort = SortExpression
                GV.DataSource = DV
                GV.DataBind()
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Master Error : " + ex.ToString)
        End Try
    End Sub

    ' dipake di form transaksi
    Public Function BindDataView(ByVal tabelname As String, ByVal SortBy As String, ByVal StrFilter As String, ByVal pagenum As Integer, ByVal pagesize As Integer, ByVal Connection As String) As DataTable
        Dim DT As DataTable

        Try
            Dim SQLString As String
            'DS = SQLExecuteQuery("Select * from V_FNBeginSI" + StrFilter, Session("DBConnection").ToString)
            If StrFilter.Length > 6 Then
                If Not StrFilter.ToUpper.Contains("WHERE ") Then
                    StrFilter = " WHERE " + StrFilter
                End If
            End If
            SQLString = "EXEC S_GetDataView " + QuotedStr(tabelname) + ", " + QuotedStr(SortBy) + ", " + pagenum.ToString + ", " + pagesize.ToString + ", " + QuotedStr(StrFilter)
            DT = SQLExecuteQuery(SQLString, Connection).Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString + " Query data view : " + tabelname)
        End Try
    End Function

    Public Sub BindDataPage(ByVal tabelname As String, ByVal StrFilter As String, ByVal pagesize As Integer, ByVal ddlPage As DropDownList, ByVal Connection As String)
        Dim Pages, SQLString As String
        Dim Li As ListItem
        Try
            'DS = SQLExecuteQuery("Select * from V_FNBeginSI" + StrFilter, Session("DBConnection").ToString)
            If StrFilter.Length > 6 Then
                If Not StrFilter.ToUpper.Contains("WHERE ") Then
                    StrFilter = " WHERE " + StrFilter
                End If
            End If
            SQLString = "Declare @Page Int EXEC S_GetDataPage " + QuotedStr(tabelname) + ", " + pagesize.ToString + ", " + QuotedStr(StrFilter) + ", @Page Out SELECT @Page AS Page "
            Pages = SQLExecuteScalar(SQLString, Connection)
            ddlPage.Items.Clear()
            For i = 1 To Int(Pages)
                Li = New ListItem(i.ToString, i.ToString)
                ddlPage.Items.Add(Li)
            Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString + " Query S_GetDataPage table : " + tabelname)
        End Try
    End Sub

    Public Function GetDataPageMax(ByVal tabelname As String, ByVal StrFilter As String, ByVal pagesize As Integer, ByVal Connection As String) As Integer
        Dim Pages, SQLString As String
        Try
            'DS = SQLExecuteQuery("Select * from V_FNBeginSI" + StrFilter, Session("DBConnection").ToString)
            If StrFilter.Length > 6 Then
                If Not StrFilter.ToUpper.Contains("WHERE ") Then
                    StrFilter = " WHERE " + StrFilter
                End If
            End If
            SQLString = "Declare @Page Int EXEC S_GetDataPage " + QuotedStr(tabelname) + ", " + pagesize.ToString + ", " + QuotedStr(StrFilter) + ", @Page Out SELECT @Page AS Page "
            Pages = SQLExecuteScalar(SQLString, Connection)
            Return CInt(Pages)
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString + " Query S_GetDataPage table : " + tabelname)
        End Try
    End Function

    Public Function BindDataTransaction(ByVal SQLString As String, ByVal StrFilter As String, ByVal Connection As String) As DataTable
        Dim DT As DataTable
        Try
            'DS = SQLExecuteQuery("Select * from V_FNBeginSI" + StrFilter, Session("DBConnection").ToString)
            If SQLString.ToUpper.Contains("WHERE ") Then
                If StrFilter.Length > 0 Then
                    SQLString = SQLString + " AND " + StrFilter
                Else
                    SQLString = SQLString + StrFilter
                End If
            Else
                If StrFilter.Length > 0 Then
                    SQLString = SQLString + " WHERE " + StrFilter
                End If
            End If
            DT = SQLExecuteQuery(SQLString, Connection).Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString + " Query : " + SQLString)
        End Try
    End Function


    ' "Product, Location", "AAA|LO"
    Public Function CekExistData(ByVal dt As DataTable, ByVal KeyField As String, ByVal keyvalue As String) As Boolean
        Dim key(), value(), criteria As String
        Dim dr As DataRow()
        key = KeyField.Split(",")
        value = keyvalue.Split("|")
        criteria = ""
        For I = 0 To key.Length - 1
            If I = 0 Then
                criteria = criteria + " " + key(I) + " = " + QuotedStr(value(I))
            Else
                criteria = criteria + " and " + key(I) + " = " + QuotedStr(value(I))
            End If
        Next
        dr = dt.Select(criteria)
        Return dr.Length > 0
    End Function

    Public Function GetMonthName(ByVal monthNum As Integer) As String
        Try
            Dim strDate As New DateTime(1, monthNum, 1)
            Return strDate.ToString("MMMM")
        Catch ex As Exception
            Throw New Exception("GetMonthName Error : " + ex.ToString)
        End Try
    End Function

    Public Function FormatFloat(ByVal Value As Object, ByVal Digit As Integer) As String
        Dim FValue As Double
        Dim SValue As String
        Try
            SValue = CStr(Value)
            If IsNumeric(SValue.Replace(",", "")) = True Then
                FValue = CDbl(SValue.Replace(",", ""))
            Else
                FValue = 0
            End If
            If Digit >= 0 Then
                Return FormatNumber(FValue, Digit)
            Else
                If FValue = 0 Then
                    Return "0"
                Else
                    Return Format(FValue, "#,##0.########")
                End If
            End If
        Catch ex As Exception
            Throw New Exception("FormatQty Error : " + ex.ToString)
        End Try

    End Function
    Public Function FloorKoma(ByVal Value As Double, ByVal Digit As Integer) As Double
        Try
            Dim Kali As Double
            Dim hasil As Double

            If Digit = 1 Then
                Kali = 10
            ElseIf Digit = 2 Then
                Kali = 100
            ElseIf Digit = 3 Then
                Kali = 1000
            ElseIf Digit = 4 Then
                Kali = 10000
            ElseIf Digit = 5 Then
                Kali = 100000
            ElseIf Digit = 6 Then
                Kali = 1000000
            ElseIf Digit = 7 Then
                Kali = 10000000
            Else
                Kali = 1
            End If
            hasil = Math.Floor(Value * Kali)
            Return (hasil / Kali)

        Catch ex As Exception
            Throw New Exception("FloorKoma Error : " + ex.ToString)
        End Try
    End Function

End Module
