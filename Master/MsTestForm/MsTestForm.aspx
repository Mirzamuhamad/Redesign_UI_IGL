<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsTestForm.aspx.vb" Inherits="MsForm" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
        <%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
            TagPrefix="BDP" %>

            <!DOCTYPE html
                PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

            <html xmlns="http://www.w3.org/1999/xhtml">

            <head runat="server">
                <title>Seller File</title>


                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
                <link rel="stylesheet"
                    href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
                <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap"
                    rel="stylesheet">
                <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css" />
                <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
                <link href="../../Styles/StyleNew.css" rel="stylesheet" type="text/css" />
                <!-- untuk loading -->
                <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

                <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
                <script src="../../Function/Function.JS" type="text/javascript"></script>
                <!-- <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" /> -->
                <!-- untuk date picker -->
                <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
                <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">

                <script type="text/javascript">
                    $(document).ready(function () {
                        flatpickr(".datepicker", {
                            dateFormat: "d M Y",//"m/d/Y", //"d M Y",  // hasil: 23 Oct 2025
                            allowInput: true
                        });
                    });
                </script>
                <!-- ----------------- -->

            </head>

            <body>
                <form id="form1" runat="server">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <div class="Content">
                        <div class="card bg-white sticky-top shadow-sm border-0 rounded-0">
                            <div class="card-body py-2">
                                <h6 class="mb-0">Sample Form Master Boostrap</h6>
                            </div>
                        </div>


                        <!-- <hr class="my-4 border-primary" /> -->
                        <asp:Panel runat="server" ID="pnlHd">
                            <div class="container-fluid mt-2">
                                <div class="row mb-2 align-items-center">
                                    <!-- Kolom kiri: filter & tombol -->
                                    <div class="col d-flex gap-2 flex-wrap">
                                        <!-- Textbox dengan placeholder -->
                                        <asp:TextBox runat="server" ID="tbFilter"
                                            CssClass="form-control form-control-sm w-auto" placeholder="Cari data...">
                                        </asp:TextBox>

                                        <!-- Dropdown -->
                                        <asp:DropDownList runat="server" ID="ddlField"
                                            CssClass="form-select form-select-sm w-auto">
                                            <asp:ListItem Selected="true" Text="Seller Code" Value="SellCode">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Seller Name" Value="SellName"></asp:ListItem>
                                            <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
                                            <asp:ListItem Text="Type ID" Value="TypeID"></asp:ListItem>
                                            <asp:ListItem Text="Seller ID" Value="SellID"></asp:ListItem>
                                            <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>
                                            <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>
                                            <asp:ListItem Text="City" Value="City"></asp:ListItem>
                                            <asp:ListItem Text="ZipCode" Value="ZipCode"></asp:ListItem>
                                            <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                                            <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                            <asp:ListItem Text="NPWP" Value="NPWP"></asp:ListItem>
                                        </asp:DropDownList>

                                        <!-- Tombol dengan ikon -->
                                        <asp:LinkButton runat="server" ID="btnSearch" CssClass="btn btn-primary btn-sm">
                                            <i class="fa fa-search"></i> Search
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="btnExpand"
                                            CssClass="btn btn-secondary btn-sm">
                                            <i class="fa fa-ellipsis-h"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="btnPrint" CssClass="btn btn-success btn-sm">
                                            <i class="fa fa-print"></i> Print
                                        </asp:LinkButton>
                                    </div>

                                    <!-- Kolom kanan: tombol Add -->
                                    <div class="col-auto d-flex justify-content-end">
                                        <asp:LinkButton ID="btnAdd" runat="server"
                                            CssClass="btn btn-primary btn-sm px-5">
                                            <i class="fa fa-plus"></i> Add
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>


                            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                                <div class="container-fluid">
                                    <div class="row mb-2">
                                        <div class="col d-flex gap-2 flex-wrap">
                                            <!-- Dropdown Notasi -->
                                            <asp:DropDownList CssClass="form-select form-select-sm w-auto"
                                                runat="server" ID="ddlNotasi">
                                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                            </asp:DropDownList>

                                            <!-- Textbox dengan placeholder -->
                                            <asp:TextBox runat="server" ID="tbfilter2"
                                                CssClass="form-control form-control-sm w-auto"
                                                placeholder="Cari data...">
                                            </asp:TextBox>

                                            <!-- Dropdown Field -->
                                            <asp:DropDownList runat="server" ID="ddlField2"
                                                CssClass="form-select form-select-sm w-auto">
                                                <asp:ListItem Selected="true" Text="Seller Code" Value="SellCode">
                                                </asp:ListItem>
                                                <asp:ListItem Text="Seller Name" Value="SellName"></asp:ListItem>
                                                <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
                                                <asp:ListItem Text="Type ID" Value="TypeID"></asp:ListItem>
                                                <asp:ListItem Text="Seller ID" Value="SellID"></asp:ListItem>
                                                <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>
                                                <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>
                                                <asp:ListItem Text="City" Value="City"></asp:ListItem>
                                                <asp:ListItem Text="ZipCode" Value="ZipCode"></asp:ListItem>
                                                <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                                                <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                                <asp:ListItem Text="NPWP" Value="NPWP"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>


                            <div class="container-fluid mt-0">
                                <div class="card shadow-sm">
                                    <div class="card-body p-2">
                                        <!-- wrapper biar tabel bisa scroll kanan & vertikal -->
                                        <div class="table-responsive" style="max-height:100vh; overflow-y:auto;">
                                        <asp:GridView ID="DataGrid" runat="server" ShowFooter="false"
                                            AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True"
                                            PageSize="10"
                                            CssClass="table table-striped table-bordered table-hover table-sm  text-nowrap table-soft-text">
                                            <HeaderStyle CssClass="table-soft-dark text-white sticky-top" />
                                            <FooterStyle CssClass="table-light" />
                                            <PagerStyle CssClass="grid-pager text-center bg-light"
                                                HorizontalAlign="Left" />


                                                <Columns>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:DropDownList
                                                                CssClass="form-select form-select-sm d-inline w-auto"
                                                                ID="ddl" runat="server">
                                                                <asp:ListItem Selected="True" Text="View" />
                                                                <asp:ListItem Text="Edit" />
                                                                <asp:ListItem Text="Delete" />
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="btn btn-primary btn-sm ms-1"
                                                                runat="server" ID="btnGo" Text="Go"
                                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                                CommandName="Go" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="110px" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="SellCode" HeaderText="Seller Code"
                                                        SortExpression="SellCode" />
                                                    <asp:BoundField DataField="SellName" HeaderText="Seller Name"
                                                        SortExpression="SellName" />
                                                    <asp:BoundField DataField="Gender" HeaderText="Gender"
                                                        SortExpression="Gender" />
                                                    <asp:BoundField DataField="TypeID" HeaderText="Type ID "
                                                        SortExpression="TypeID" />
                                                    <asp:BoundField DataField="SellID" HeaderText="ID No"
                                                        SortExpression="SellID" />
                                                    <asp:BoundField DataField="NoKK" HeaderText="KK No "
                                                        SortExpression="NoKK" />
                                                    <asp:BoundField DataField="Address1" HeaderText="Address"
                                                        SortExpression="Address1" />
                                                    <asp:BoundField DataField="Address2" HeaderText="Address 2"
                                                        SortExpression="Address2" />
                                                    <asp:BoundField DataField="Desa" HeaderText="Desa"
                                                        SortExpression="Desa" />
                                                    <asp:BoundField DataField="Kec" HeaderText="Kecamatan"
                                                        SortExpression="Kec" />
                                                    <asp:BoundField DataField="Kab" HeaderText="Kabupaten"
                                                        SortExpression="Kab" />
                                                    <asp:BoundField DataField="City" HeaderText="City"
                                                        SortExpression="City" />
                                                    <asp:BoundField DataField="ZipCode" HeaderText="ZipCode"
                                                        SortExpression="ZipCode" />
                                                    <asp:BoundField DataField="Phone" HeaderText="Phone"
                                                        SortExpression="Phone" />
                                                    <asp:BoundField DataField="Email" HeaderText="Email"
                                                        SortExpression="Email" />
                                                    <asp:BoundField DataField="NPWP" HeaderText="NPWP"
                                                        SortExpression="NPWP" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <div class="d-flex justify-content-between align-items-center mt-2">
                                            <label class="me-2 ">Rows Page:</label>
                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"
                                                CssClass="form-select form-select-sm form-select-sm w-auto">
                                                <asp:ListItem Text="10" Value="10" Selected="True" />
                                                <asp:ListItem Text="25" Value="25" />
                                                <asp:ListItem Text="50" Value="50" />
                                                <asp:ListItem Text="100" Value="100" />
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                </div>
                            </div>


                            <!-- <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible="False" /> -->
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlInput" Visible="false">
                            <br>
                            <div class="container-fluid">
                                <div class="row mb-2">
                                    <div class="col-md-2">
                                        <label class="form-label">Nomor PO</label>
                                        <asp:TextBox ID="txtNomorPO" runat="server"
                                            CssClass="form-control form-control-sm">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <label class="form-label">Tanggal datepicker Fatpicker</label>
                                        <asp:TextBox ID="tbDatePO" runat="server"
                                            CssClass="form-control form-control-sm datepicker" ReadOnly="true"
                                            ValidationGroup="Input" />

                                    </div>

                                    <div class="col-md-2">
                                        <label class="form-label">Tanggal Basic date picker css</label>
                                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy"
                                            ReadOnly="True" ValidationGroup="Input" ButtonImageHeight="19px"
                                            ButtonImageWidth="20px" DisplayType="TextBox" AutoPostBack="True"
                                            ShowNoneButton="False">
                                            <TextBoxStyle CssClass="form-control form-control-sm" />
                                        </BDP:BasicDatePicker>
                                    </div>

                                    <div class="col-md-3">
                                        <label class="form-label">Supplier</label>
                                        <asp:TextBox ID="txtSupplier" runat="server"
                                            CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label">Type PO</label>
                                        <asp:DropDownList ID="ddlTypePO" runat="server"
                                            CssClass="form-select form-select-sm">
                                            <asp:ListItem Value="">-- Pilih Type --</asp:ListItem>
                                            <asp:ListItem Value="Lokal">Lokal</asp:ListItem>
                                            <asp:ListItem Value="Import">Import</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>


                                <div class="row mb-2">

                                    <div class="col-md-3">
                                        <label class="form-label">Seller Code</label>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm"
                                            AutoPostBack="true" ID="tbCode" MaxLength="5" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label">Seller Name</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="tbName"
                                            ValidationGroup="Input" />
                                    </div>


                                    <div class="col-md-3">
                                        <label class="form-label">Gender</label>
                                        <asp:DropDownList runat="server" CssClass="form-select form-select-sm"
                                            ID="ddlGender" ValidationGroup="Input">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Laki - Laki</asp:ListItem>
                                            <asp:ListItem>Perempuan</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label">Type ID</label>
                                        <asp:DropDownList runat="server" CssClass="form-select form-select-sm"
                                            ID="ddlTypeID" ValidationGroup="Input">
                                            <asp:ListItem Selected="True">KTP</asp:ListItem>
                                            <asp:ListItem>SIM C</asp:ListItem>
                                            <asp:ListItem>SIM A</asp:ListItem>
                                            <asp:ListItem>SIM B</asp:ListItem>
                                            <asp:ListItem>Passport</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row mb-2">
                                    <div class="col-md-3">
                                        <label class="form-label">ID No</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="tbSellerID"
                                            ValidationGroup="Input" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label">KK No</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="TbKk" ValidationGroup="Input" />
                                    </div>

                                    <div class="col-md-3">
                                        <label class="form-label">Address</label>
                                        <asp:TextBox runat="server" MaxLength="200"
                                            CssClass="form-control form-control-sm" ID="tbAddress"
                                            ValidationGroup="Input" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label">Address 2</label>
                                        <asp:TextBox runat="server" MaxLength="200"
                                            CssClass="form-control form-control-sm" ID="tbAddress2"
                                            ValidationGroup="Input" />
                                    </div>
                                </div>


                                <div class="row mb-2">
                                    <div class="col-md-2">
                                        <label class="form-label">Kota</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="tbCity"
                                            ValidationGroup="Input" />
                                    </div>

                                    <div class="col-md-2">
                                        <label class="form-label">Kabupaten</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="TbKab"
                                            ValidationGroup="Input" />
                                    </div>

                                    <div class="col-md-2">
                                        <label class="form-label">Kecamatan</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="tbKec"
                                            ValidationGroup="Input" />
                                    </div>

                                    <div class="col-md-5">
                                        <label class="form-label">Desa (Rt/Rw)</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="tbDesa"
                                            ValidationGroup="Input" />
                                    </div>


                                    <div class="col-md-1">
                                        <label class="form-label">ZipCode</label>
                                        <asp:TextBox runat="server" MaxLength="5"
                                            CssClass="form-control form-control-sm" ID="tbZipCode"
                                            ValidationGroup="Input" />
                                    </div>


                                </div>
                                <div class="row mb-2">

                                    <div class="col-md-3">
                                        <label class="form-label">Phone</label>
                                        <asp:TextBox runat="server" MaxLength="13"
                                            CssClass="form-control form-control-sm" ID="tbPhone"
                                            ValidationGroup="Input" />
                                    </div>

                                    <div class="col-md-3">
                                        <label class="form-label">Email</label>
                                        <asp:TextBox runat="server" MaxLength="60"
                                            CssClass="form-control form-control-sm" ID="tbEmail"
                                            ValidationGroup="Input" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label">NPWP</label>
                                        <asp:TextBox runat="server" MaxLength="20"
                                            CssClass="form-control form-control-sm" ID="tbNpwp"
                                            ValidationGroup="Input" />
                                    </div>

                                    <div class="col-md-3">
                                        <label class="form-label">INVOICE Reg</label>
                                        <asp:DropDownList CssClass="form-select form-select-sm" ValidationGroup="Input"
                                            runat="server" ID="ddlFgPT">
                                            <asp:ListItem Selected="True">Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>




                    </div>

                    <div class="container-fluid">
                        <div class="d-flex gap-2 mt-3">
                            <!-- Save -->
                            <asp:LinkButton ID="BtnSave" runat="server" CssClass="btn btn-success btn-sm"
                                CommandName="Update">
                                <i class="fa fa-save"></i> Save
                            </asp:LinkButton>

                            <!-- Cancel -->
                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-danger btn-sm"
                                CommandName="Cancel">
                                <i class="fa fa-times"></i> Cancel
                            </asp:LinkButton>

                            <!-- Reset -->
                            <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-secondary btn-sm"
                                CommandName="Reset">
                                <i class="fa fa-undo"></i> Reset
                            </asp:LinkButton>

                            <!-- Home -->
                            <asp:LinkButton ID="btnHome" runat="server" CssClass="btn btn-primary btn-sm">
                                <i class="fa fa-home"></i> Home
                            </asp:LinkButton>
                        </div>
                    </div>
                    <br>
                    <br>

                    </asp:Panel>

                    <!-- Modal Popup Status -->
                    <div class="modal fade" id="statusModal" tabindex="-1" aria-labelledby="statusModalLabel"
                        aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content border-0 shadow">
                                <!-- <div class="modal-header bg-primary text-white py-2">
                                    <h6 class="modal-title mb-0" id="statusModalLabel">
                                        <i class="fa fa-info-circle me-2"></i>Alert Status
                                    </h6>
                                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                                        aria-label="Close"></button>
                                </div> -->

                                <div class="modal-body text-center" id="statusModalBody">
                                    <div class="container-fluid">
                                        <asp:Label runat="server" ID="lstatus"
                                            CssClass="badge d-block text-center fs-6 p-2" EnableViewState="false" />
                                    </div>

                                </div>
                                <!-- 
                                <div class="modal-footer justify-content-center py-2">
                                    <button type="button" class="btn btn-primary btn-sm"
                                        data-bs-dismiss="modal">OK</button>
                                </div> -->
                            </div>
                        </div>
                    </div>


                    <div class="loading-form">
                        <div class="spinner-border text-primary spinner-style" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                    </div>

                </form>
            </body>

            </html>