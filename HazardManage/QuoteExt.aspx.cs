using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;

using System.Data;
using System.Text;
using GhtnTech.SEP.DBUtility;
using System.Data.OracleClient;
using GhtnTech.SecurityFramework.BLL;

public partial class HazardManage_QuoteExt : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {

                UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
                if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
                {
                    if (!UserHandle.ValidationHandle(PermissionTag.Add))//引用
                    {
                        btn_yy.Visible = false;//引用
                        btn_Sure.Visible = false;//全部引用
                        btn_C.Visible = false;//全部取消
                        btn_cancel.Visible = false;//取消
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Edit))//保存
                    {
                        btn_Save.Visible = false;
                    }

                    BuildTree();

                    Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("SGLX", this)), bmStore1);
                    Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("FXDJ", this)), bmStore2);
                    Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("FXLX", this)), bmStore3);

                    string ren = PublicMethod.ReadXmlReturnNode("REN", this) + "," + PublicMethod.ReadXmlReturnNode("GUAN", this);
                    //bindGLDX(ren, Store1);
                    bindGLDX(ren, Store2);
                    bindGLDX(ren, Store3);
                    bindGLDX(ren, Store4);

                    Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("SWJB", this)), bmStore5);
                    Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("YHJB", this)), bmStore4);

                    GridPanel1.Hide();
                    Panel1.Hide();

                    //首页待办连接处理-周义生
                    if (Request.QueryString["H_YY"] != null)
                    {
                        tpZY.Hide();
                        bindH_YY();
                        GridPanel1.Show();
                        btn_Sure.Hide();
                        btn_C.Hide();
                    }
                }
                else
                {
                    Session["ErrorNum"] = "0";
                    Response.Redirect("~/Error.aspx");
                }
                btn_Save.Disabled = true;
                btn_yy.Disabled = true;
                btn_cancel.Disabled = true;
            }
        }
    }

    //绑定本单位未引用危险源信息-周义生
    private void bindH_YY()
    {
        string oraceltext = "select HAZARDSID,a.infoname ZYNAME,worktasks.worktask GZRWNAME,process.name GXNAME,H_CONTENT,b.infoname FXLX,H_CONSEQUENCES,c.infoname SGLX,HAZARDS.ISPASS,(case nvl(d.id,-1) when -1 then  '通用' else '已引用' end) ISGROUP from HAZARDS inner join process on HAZARDS.PROCESSID=process.processid inner join worktasks on process.worktaskid=worktasks.worktaskid inner join CS_BASEINFOSET a on worktasks.professionalid=a.infoid inner join CS_BASEINFOSET b on HAZARDS.Risk_Typesnumber=b.infoid inner join CS_BASEINFOSET c on HAZARDS.Accident_Typenumber=c.infoid left join (select * from HAZARDS_ORE where HAZARDS_ORE.DEPTNUMBER='{0}') d on HAZARDS.h_Number=d.H_NUMBER where HAZARDSID in (select hazardsid from hazards  where not exists (select hazards_ore.h_number from hazards_ore where hazards.h_number=hazards_ore.h_number and  hazards_ore.deptnumber=" + SessionBox.GetUserSession().DeptNumber + ") )";
        hazardsStore.DataSource = OracleHelper.Query(oraceltext);
        hazardsStore.DataBind();
    }

    private void Bindcombox(int fid, Store store)
    {
        string oracletext = "select * from CS_BASEINFOSET where FID = " + fid + " ";
        store.DataSource = OracleHelper.Query(oracletext);
        store.DataBind();
    }
    //绑定管理对象
    private void bindGLDX(string id, Store store)
    {
        string oracletext = "select * from M_OBJECT where RISK_TYPESID in (" + id + ")";
        store.DataSource = OracleHelper.Query(oracletext);
        store.DataBind();
    }
    #region 绑定树

    private void BuildTree()
    {
        tpZY.Root.Clear();
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
        root.Text = "-1";
        root.NodeID = "辨识单元";
        tpZY.Root.Add(root);
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from CS_BASEINFOSET where FID={0} order by INFOID", PublicMethod.ReadXmlReturnNode("ZY", this)));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            //Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r["INFONAME"].ToString();
            asyncNode.NodeID = "z" + r["INFOID"].ToString();
            //AddGZRW_Temp(asyncNode.Nodes, r["INFOID"].ToString());
            //AddGZRW(asyncNode.Nodes, r["INFOID"].ToString());
            root.Nodes.Add(asyncNode);
        }
    }

    [AjaxMethod]
    public string NodeLoad(string nodeID)
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
        switch (nodeID.Substring(0, 1))
        {
            case "z":
                AddGZRW(nodes, nodeID.Substring(1));
                break;
            case "w":
                AddGX(nodes, nodeID.Substring(1));
                break;
        }

        string a = nodes.ToJson();

        return nodes.ToJson();
    }

    private void AddGZRW(Coolite.Ext.Web.TreeNodeCollection nodes, string p)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from worktasks where professionalid={0}", p));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r["WORKTASK"].ToString();
            asyncNode.NodeID = "w" + r["WORKTASKID"].ToString();
            nodes.Add(asyncNode);
        }
    }

    private void AddGX(Coolite.Ext.Web.TreeNodeCollection nodes, string p)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from process where WORKTASKID={0} order by SERIALNUMBER", p));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r["NAME"].ToString();
            asyncNode.NodeID = "p" + r["PROCESSID"].ToString();
            asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad('{0}','F');", r["PROCESSID"].ToString().Trim());
            asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    #endregion


    protected void RowClick(object sender, AjaxEventArgs e)//单击行事件
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            string json = e.ExtraParams["Values"];
            Dictionary<string, string>[] data = JSON.Deserialize<Dictionary<string, string>[]>(json);
            string HAZARDSID = ((Dictionary<string, string>)data[0])["HAZARDSID"].Trim();
            string ISGROUP = ((Dictionary<string, string>)data[0])["ISGROUP"].Trim();//AHN
            switch (ISGROUP)
            {
                case "已引用":
                    btn_Save.Disabled = false;
                    btn_yy.Disabled = true;
                    btn_cancel.Disabled = false;
                    break;
                case "通用":
                    btn_Save.Disabled = true;
                    btn_yy.Disabled = false;
                    btn_cancel.Disabled = true;
                    break;
            }
            string oracletext = "select RISK_TYPESNUMBER from hazards where HAZARDSID =  " + sm.SelectedRow.RecordID + "";
            int RISK_TYPESNUMBER = int.Parse(OracleHelper.GetSingle(oracletext).ToString().Trim());
            //if (RISK_TYPESNUMBER == int.Parse(PublicMethod.ReadXmlReturnNode("REN", this)) || RISK_TYPESNUMBER == int.Parse(PublicMethod.ReadXmlReturnNode("GUAN", this)))
            //{
            //    Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("SWJB", this)), bmStore4);
            //}
            //else
            //{
            //    Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("YHJB", this)), bmStore4);
            //}
            //--------------新加了风险类型 为拼音检索做准备
            //extRISK_TYPESNUMBER.SelectedItem.Value = RISK_TYPESNUMBER.ToString();
            string oracletext1 = "select * from M_OBJECT where RISK_TYPESID = " + RISK_TYPESNUMBER.ToString();
            Store1.DataSource = OracleHelper.Query(oracletext1);
            Store1.DataBind();
            //--------------
            LoadDetailData(HAZARDSID, ISGROUP);
        }
        else
        {
            btn_Save.Disabled = true;
            btn_yy.Disabled = true;
            btn_cancel.Disabled = true;
        }

    }

    private void LoadDetailData(string ID,string kind)//加载明细信息
    {
        string basesql = "";
        DataTable dt;
        if (kind.Trim() == "已引用")
        {
            basesql = "select h.h_number,h.h_content,h.h_consequences,h.m_standards,h.m_measures,h.regulatorymeasures,h.risk_typesnumber,h.accident_typenumber,ho.risk_evelnumber,ho.m_objectnumber,ho.m_personnumber,ho.directlyresponsiblepersonsnumb,ho.regulatorypartnersnumber,ho.processingmode,ho.punishmentstandard,ho.note,ho.h_bm,ho.levelid,ho.SCORES,ho.sw_bm,ho.sw_levelid,ho.sw_scores,ho.SW_PUNISHMENTSTANDARD from hazards h inner join hazards_ore ho on h.h_number=ho.h_number where ho.h_bjw='引用' and ho.deptnumber='" + SessionBox.GetUserSession().DeptNumber + "'";
            dt = OracleHelper.Query(basesql + " and h.HAZARDSID=" + ID).Tables[0];
        }
        else
        {
            basesql = "select h.h_number,h.h_content,h.h_consequences,h.m_standards,h.m_measures,h.regulatorymeasures,h.risk_typesnumber,h.accident_typenumber,ho.risk_evelnumber,ho.m_objectnumber,ho.m_personnumber,ho.directlyresponsiblepersonsnumb,ho.regulatorypartnersnumber,ho.processingmode,ho.punishmentstandard,ho.note,ho.h_bm,ho.levelid,ho.SCORES,ho.sw_bm,ho.sw_levelid,ho.sw_scores,ho.SW_PUNISHMENTSTANDARD  from hazards h inner join hazards_ore ho on h.h_number=ho.h_number where ho.h_bjw='通用'";
            dt = OracleHelper.Query(basesql + " and h.HAZARDSID=" + ID).Tables[0];
        }
        Panel1.Show();
        SetControlValue(dt);
    }

    private void SetControlValue(DataTable dt)//设置控件值
    {
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            try
            {
                Control c = Panel1.FindControl("ext" + dt.Columns[i].ColumnName.Trim());
                string type = c.GetType().Name.Trim();
                switch (type)
                {
                    case "ComboBox":
                        ComboBox cbb = (ComboBox)c;
                        cbb.SelectedItem.Value = dt.Rows[0][i].ToString().Trim();
                        break;
                    case "TextArea":
                        TextArea ta = (TextArea)c;
                        ta.Text = dt.Rows[0][i].ToString().Trim();
                        break;
                }
            }
            catch
            {
                continue;
            }
        }
    }

    [AjaxMethod]
    public void PYchange(string py, string store)
    {
        string oracletext = "select * from M_OBJECT where RISK_TYPESID = 8 and MO_F_PINYIN like '%" + py.ToUpper() + "%'";
        switch (store)
        {
            case "Store1":
                string oracletext1 = "select * from M_OBJECT where RISK_TYPESID = " + extRISK_TYPESNUMBER.SelectedItem.Value.Trim() + " and MO_F_PINYIN like '%" + py.ToUpper() + "%'";
                Store1.DataSource = OracleHelper.Query(oracletext1);
                Store1.DataBind();
                break;
            case "Store2":
                Store2.DataSource = OracleHelper.Query(oracletext);
                Store2.DataBind();
                break;
            case "Store3":
                Store3.DataSource = OracleHelper.Query(oracletext);
                Store3.DataBind();
                break;
            case "Store4":
                Store4.DataSource = OracleHelper.Query(oracletext);
                Store4.DataBind();
                break;
        }
    }

    [AjaxMethod]
    public void GVLoad(string processid, string kind)
    {
        GhtnTech.SEP.OraclDAL.BaseTableGet btg = new GhtnTech.SEP.OraclDAL.BaseTableGet();
        Hidden1.Value = processid;
        //Hidden2.Value = kind;
        hazardsStore.DataSource = btg.GetHAZARDSForUsingView(" and HAZARDS.PROCESSID='" + processid + "'", SessionBox.GetUserSession().DeptNumber);
        hazardsStore.DataBind();
        GridPanel1.Show();
        Panel1.Hide();
        btn_Save.Disabled = true;
        btn_yy.Disabled = true;
        btn_cancel.Disabled = true;
    }

    [AjaxMethod]
    public void btnSure_Click()//可能缺发布人
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string oracletext = "select * from HAZARDS inner join HAZARDS_ORE on HAZARDS.H_NUMBER=HAZARDS_ORE.H_NUMBER where HAZARDS_ORE.H_BJW='通用' and HAZARDS.PROCESSID = " + Hidden1.Value + " ";
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        int num = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //插入附表
            string sql = "select * from HAZARDS inner join HAZARDS_ORE on HAZARDS.H_NUMBER=HAZARDS_ORE.H_NUMBER where HAZARDS_ORE.H_BJW='引用' and hazards_ore.deptnumber='" + SessionBox.GetUserSession().DeptNumber + "' and hazards.h_number='" + dt.Rows[i]["H_NUMBER"].ToString().Trim() + "'";
            if (OracleHelper.Query(sql).Tables[0].Rows.Count > 0)
            {
                continue;
            }
            string ssql = "insert into HAZARDS_ORE(H_NUMBER,RISK_EVELNUMBER,M_OBJECTNUMBER,M_PERSONNUMBER,DIRECTLYRESPONSIBLEPERSONSNUMB,REGULATORYPARTNERSNUMBER,DATEINPUT,PERSONID,ISPASS,DEPTNUMBER,H_BJW,H_BM,H_F_PINYIN,PROCESSINGMODE,PUNISHMENTSTANDARD,NOTE)";
            ssql += " values ('" + dt.Rows[i]["H_NUMBER"] + "','" + dt.Rows[i]["RISK_EVELNUMBER"].ToString().Trim() + "','" + dt.Rows[i]["M_OBJECTNUMBER"].ToString().Trim() + "','" + dt.Rows[i]["M_PERSONNUMBER"].ToString().Trim() + "','" + dt.Rows[i]["DIRECTLYRESPONSIBLEPERSONSNUMB"].ToString().Trim() + "','" + dt.Rows[i]["REGULATORYPARTNERSNUMBER"].ToString().Trim() + "',sysdate,'','已发布','" + SessionBox.GetUserSession().DeptNumber + "','引用','" + dt.Rows[i]["H_BM"].ToString().Trim() + "','" + dt.Rows[i]["H_F_PINYIN"].ToString().Trim() + "','" + dt.Rows[i]["PROCESSINGMODE"].ToString().Trim() + "','" + dt.Rows[i]["PUNISHMENTSTANDARD"].ToString().Trim() + "','" + dt.Rows[i]["NOTE"].ToString().Trim() + "')";
            OracleHelper.Query(ssql);
            num++;
        }
        GVLoad(Hidden1.Value.ToString().Trim(), "F");
        Ext.Msg.Alert("提示", "共引用" + num + "条!").Show();
    }

    [AjaxMethod]
    public void btnC_Click()//全部取消
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string oracletext = "select * from HAZARDS inner join HAZARDS_ORE on HAZARDS.H_NUMBER=HAZARDS_ORE.H_NUMBER where HAZARDS_ORE.H_BJW='引用' and HAZARDS_ORE.DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "' and HAZARDS.PROCESSID = " + Hidden1.Value + " ";
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //附表删除
            string ssql = "delete HAZARDS_ORE where ID='" + dt.Rows[i]["ID"].ToString().Trim() + "'";
            OracleHelper.Query(ssql);
        }
        GVLoad(Hidden1.Value.ToString().Trim(), "F");
        Ext.Msg.Alert("提示", "共取消" + dt.Rows.Count + "条!").Show();
    }

    [AjaxMethod]
    public void btnSave_Click()//修改引用
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string sql = "select * from HAZARDS inner join HAZARDS_ORE on HAZARDS.H_NUMBER=HAZARDS_ORE.H_NUMBER where HAZARDS_ORE.H_BJW='引用' and HAZARDS_ORE.DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "' and HAZARDS.HAZARDSID=" + sm.SelectedRow.RecordID;
        DataTable dt = OracleHelper.Query(sql).Tables[0];
        string updatesql = "update HAZARDS_ORE set M_OBJECTNUMBER='" + extM_OBJECTNUMBER.SelectedItem.Value + "',M_PERSONNUMBER='" + extM_PERSONNUMBER.SelectedItem.Value + "',RISK_EVELNUMBER='" + extRISK_EVELNUMBER.SelectedItem.Value + "',DIRECTLYRESPONSIBLEPERSONSNUMB='" + extDIRECTLYRESPONSIBLEPERSONSNUMB.SelectedItem.Value + "',REGULATORYPARTNERSNUMBER='" + extREGULATORYPARTNERSNUMBER.SelectedItem.Value + "',PROCESSINGMODE='" + extPROCESSINGMODE.SelectedItem.Value + "',PUNISHMENTSTANDARD='" + extPUNISHMENTSTANDARD.Text.Trim() + "',H_BM='" + extH_BM.Text.Trim() + "',NOTE='" + extNOTE.Text.Trim() + "',H_F_PINYIN=F_PINYIN('" + extH_BM.Text.Trim() + "'),SCORES='" + extSCORES.Text.Trim() + "',LEVELID='" + extLEVELID.SelectedItem.Value + "',SW_BM='" + extSW_BM.Text.Trim() + "',SW_PINYIN=F_PINYIN('" + extSW_BM.Text.Trim() + "'),SW_SCORES='" + extSW_SCORES.Text.Trim() + "',SW_LEVELID='" + extSW_LEVELID.SelectedItem.Value + "',SW_PUNISHMENTSTANDARD='"+extSW_PUNISHMENTSTANDARD.Text.Trim()+"'";
        updatesql += " where H_BJW='引用' and DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "' and H_NUMBER=" + dt.Rows[0]["H_NUMBER"].ToString().Trim();
        OracleHelper.Query(updatesql);
        //OracleHelper.Query("update HAZARDS_ORE set H_F_PINYIN=F_PINYIN('" + extH_BM.Text.Trim() + "') where H_BJW='引用' and DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "' and H_NUMBER=" + dt.Rows[0]["H_NUMBER"].ToString().Trim());//更新别名拼音 助记码
        GVLoad(Hidden1.Value.ToString().Trim(), "F");
        Ext.Msg.Alert("提示", "保存成功!").Show();
    }

    [AjaxMethod]
    public void btnyy_Click()//逐条引用
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string sql = "select * from HAZARDS inner join HAZARDS_ORE on HAZARDS.H_NUMBER=HAZARDS_ORE.H_NUMBER where HAZARDS_ORE.H_BJW='通用' and HAZARDS.HAZARDSID=" + sm.SelectedRow.RecordID;
        DataTable dt = OracleHelper.Query(sql).Tables[0];
        string ssql = "insert into HAZARDS_ORE(H_NUMBER,RISK_EVELNUMBER,M_OBJECTNUMBER,M_PERSONNUMBER,DIRECTLYRESPONSIBLEPERSONSNUMB,REGULATORYPARTNERSNUMBER,DATEINPUT,PERSONID,ISPASS,DEPTNUMBER,H_BJW,H_BM,PROCESSINGMODE,PUNISHMENTSTANDARD,NOTE,SCORES,LEVELID)";
        ssql += " values ('" + dt.Rows[0]["H_NUMBER"] + "','" + extRISK_EVELNUMBER.SelectedItem.Value + "','" + extM_OBJECTNUMBER.SelectedItem.Value + "','" + extM_PERSONNUMBER.SelectedItem.Value + "','" + extDIRECTLYRESPONSIBLEPERSONSNUMB.SelectedItem.Value + "','" + extREGULATORYPARTNERSNUMBER.SelectedItem.Value + "',sysdate,'','已发布','" + SessionBox.GetUserSession().DeptNumber + "','引用','" + extH_BM.Text.Trim() + "','" + extPROCESSINGMODE.SelectedItem.Value + "','" + extPUNISHMENTSTANDARD.Text.Trim() + "','" + extNOTE.Text.Trim() + "','" + extSCORES.Text.Trim() + "','" + extLEVELID.SelectedItem.Value+ "')";
        OracleHelper.Query(ssql);
        OracleHelper.Query("update HAZARDS_ORE set H_F_PINYIN=F_PINYIN('" + extH_BM.Text.Trim() + "') where H_BJW='引用' and DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "' and H_NUMBER=" + dt.Rows[0]["H_NUMBER"]);//更新别名拼音 助记码
        GVLoad(Hidden1.Value.ToString().Trim(), "F");
        Ext.Msg.Alert("提示", "引用成功!").Show();
    }

    [AjaxMethod]
    public void btncancel_Click()//逐条取消
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string sql = "select * from HAZARDS inner join HAZARDS_ORE on HAZARDS.H_NUMBER=HAZARDS_ORE.H_NUMBER where HAZARDS_ORE.H_BJW='引用' and HAZARDS_ORE.DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "' and HAZARDS.HAZARDSID=" + sm.SelectedRow.RecordID;
        DataTable dt = OracleHelper.Query(sql).Tables[0];
        string ssql = "delete HAZARDS_ORE where ID='" + dt.Rows[0]["ID"].ToString().Trim() + "'";
        OracleHelper.Query(ssql);
        GVLoad(Hidden1.Value.ToString().Trim(), "F");
        Ext.Msg.Alert("提示", "取消成功!").Show();
    }
}
