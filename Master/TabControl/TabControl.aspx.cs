using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class TabControl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TabContainer1_ActiveTabChanged(TabContainer1, null);
        }
    }
    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        DataTable dTable_rgstrtn = null;
        DataTable dTable_btch = null;
        DataTable dTable_crs = null;

        try
        {       
            if (TabContainer1.ActiveTabIndex == 0)
            {
                dTable_rgstrtn = new DataTable();

                dTable_rgstrtn.Columns.Add("stdnt_cd");
                dTable_rgstrtn.Columns.Add("rgstrtn_cd");
                dTable_rgstrtn.Columns.Add("sbjct_chsn");
                dTable_rgstrtn.Columns.Add("stdnt_stts");

                dTable_rgstrtn.Rows.Add(dTable_rgstrtn.NewRow());

                dTable_rgstrtn.Rows[0]["stdnt_cd"] = "S0080032003023";
                dTable_rgstrtn.Rows[0]["rgstrtn_cd"] = "R0080032003023";
                dTable_rgstrtn.Rows[0]["sbjct_chsn"] = "ASP.NET 3.0, SQL 2005, XML";
                dTable_rgstrtn.Rows[0]["stdnt_stts"] = "VALID";

                GridView1.DataSource = dTable_rgstrtn;
                GridView1.DataBind();
                GridView1.Visible = true;
            }

            if (TabContainer1.ActiveTabIndex == 1)
            {
                dTable_btch = new DataTable();

                dTable_btch.Columns.Add("btch_cd");
                dTable_btch.Columns.Add("smstr_cd");
                dTable_btch.Columns.Add("smstr_vrsn");
                dTable_btch.Columns.Add("mx_nmbr_stdnt");

                dTable_btch.Rows.Add(dTable_btch.NewRow());

                dTable_btch.Rows[0]["btch_cd"] = "B0001";
                dTable_btch.Rows[0]["smstr_cd"] = "SM100";
                dTable_btch.Rows[0]["smstr_vrsn"] = "1.00";
                dTable_btch.Rows[0]["mx_nmbr_stdnt"] = "20";

                GridView2.DataSource = dTable_btch;
                GridView2.DataBind();
                GridView2.Visible = true;
            }

            if (TabContainer1.ActiveTabIndex == 2)
            {
                dTable_crs = new DataTable();

                dTable_crs.Columns.Add("crs_ttl");
                dTable_crs.Columns.Add("crs_drtn");
                dTable_crs.Columns.Add("smrt_crd_rqrd");

                dTable_crs.Rows.Add(dTable_crs.NewRow());

                dTable_crs.Rows[0]["crs_ttl"] = "Introducing ASP.NET 3.5";
                dTable_crs.Rows[0]["crs_drtn"] = "48 Hrs";
                dTable_crs.Rows[0]["smrt_crd_rqrd"] = "Yes";

                GridView3.DataSource = dTable_crs;
                GridView3.DataBind();
                GridView3.Visible = true;
            }
            
        }
        catch
        {
            throw;
        }
        finally
        {
            
        }

    }
}
