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

using GhtnTech.SEP.DAL;


public partial class HazardManage_Inputext : BasePage
{
    protected void Page_Load(object sender, EventArgs e)//初始化权限已添加
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
                    if (!UserHandle.ValidationHandle(PermissionTag.Add))//新增
                    {
                        btnAdd.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Edit))//编辑
                    {
                        btn_Save.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Submit))//提交
                    {
                        btnApprove.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Delete))//删除
                    {
                        btnDelete.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Verify))//审批
                    {
                        btnPYes.Visible = false;
                        ts1.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Publish))//发布
                    {
                        btnPublish.Visible = false;
                        ts2.Visible = false;
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

                    TJDXset();
                    GridPanel1.Hide();
                    Panel1.Hide();

                    //首页连接处理-周
                    if (Request.QueryString["SH_hazardstempid"] != null)
                    {
                        string oracletext = "select hazards_temp.hazardstempid HAZARDSID,hazards_temp.h_content,a.infoname FXLX,hazards_temp.h_consequences,b.infoname SGLX,hazards_temp.ispass,'H' ISFROM from hazards_temp inner join cs_baseinfoset a on hazards_temp.risk_typesnumber=a.infoid inner join cs_baseinfoset b on hazards_temp.accident_typenumber=b.infoid inner join audithazard on hazards_temp.hazardstempid=audithazard.hazardsid where hazardstempid in (" + Request["SH_hazardstempid"] + ")";
                        hazardsStore.DataSource = OracleHelper.Query(oracletext);
                        hazardsStore.DataBind();
                        GridPanel1.Show();
                        tpZY.Hide();
                    }
                    if (Request.QueryString["FB_hazardstempid"] != null)
                    {
                        string oracletext = "select hazards_temp.hazardstempid HAZARDSID,hazards_temp.h_content,a.infoname FXLX,hazards_temp.h_consequences,b.infoname SGLX,hazards_temp.ispass,'H' ISFROM from hazards_temp inner join cs_baseinfoset a on hazards_temp.risk_typesnumber=a.infoid inner join cs_baseinfoset b on hazards_temp.accident_typenumber=b.infoid where hazardstempid in (" + Request["FB_hazardstempid"] + ")";
                        hazardsStore.DataSource = OracleHelper.Query(oracletext);
                        hazardsStore.DataBind();
                        GridPanel1.Show();
                        tpZY.Hide();
                    }

                    if (Request.QueryString["TempId"] != null)//隐患三违提交转危险源--xl
                    {
                        GridPanel1.Show();
                        Panel1.Show();
                        loadTempData(Request.QueryString["TempId"]);
                    }
                }
                else
                {
                    Session["ErrorNum"] = "0";
                    Response.Redirect("~/Error.aspx");
                }
            }


        }
    }

    private void loadTempData(string TempId)//xl
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        btn_Save.Disabled = false;
        //DataTable dt = OracleHelper.Query("select * from yscollect t where t.cid='" + TempId + "'").Tables[0];
        //各基层单位查询本矿新增隐患信息
        var q = dc.Yscollect.First(p => p.Cid == decimal.Parse(TempId));
        extH_CONTENT.Text = q.Yhcontent.Trim();
        extNOTE.Text = q.Remark;
    }

    private void TJDXset()//提交对象绑定
    {
        TJDXStore.DataSource = UserHandle.GetProcessRole2(this.PageTag, Button3.ID);
        TJDXStore.DataBind();
    }
    [AjaxMethod]
    public void TJ()//提交
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            RowSelectionModel sm1 = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            if (sm1.SelectedRows.Count == 0)
            {
                Ext.Msg.Alert("提示", "请选择提交对象!").Show();
                return;
            }
            else
            {
                for (int i = 0; i < sm1.SelectedRows.Count; i++)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into AUDITHAZARD ");
                    strSql.Append("(HAZARDSID,DEPT)");
                    strSql.Append(" values(");
                    strSql.Append(":HAZARDSID,:DEPT)");
                    OracleParameter[] parameters = {
					new OracleParameter(":HAZARDSID", OracleType.VarChar,9),//ID
                    new OracleParameter(":DEPT", OracleType.VarChar,9)
                                           };
                    parameters[0].Value = sm.SelectedRow.RecordID.Trim();
                    parameters[1].Value = sm1.SelectedRows[i].RecordID.Trim();
                    OracleHelper.ExecuteSql(strSql.ToString(), parameters);
                }
            }

            //更新临时表状态
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("update HAZARDS_TEMP set ");
            strSql1.Append("ISPASS=:ISPASS ");
            strSql1.Append(" where HAZARDSTEMPID=:HAZARDSTEMPID");
            OracleParameter[] parameters1 = {
					new OracleParameter(":HAZARDSTEMPID", OracleType.VarChar,9),//ID
                    new OracleParameter(":ISPASS", OracleType.VarChar,10)
                                           };
            parameters1[0].Value = sm.SelectedRow.RecordID.Trim();
            parameters1[1].Value = "提交";
            OracleHelper.ExecuteSql(strSql1.ToString(), parameters1);

            Ext.Msg.Alert("提示", string.Format("提交成功，共提交给{0}个对象!", sm1.SelectedRows.Count)).Show();
            Window1.Hide();
            GVLoad(Hidden1.Value.ToString().Trim(), Hidden2.Value.ToString().Trim());
            btn_Save.Disabled = true;
            btnApprove.Disabled = true;
            btnDelete.Disabled = true;
            btnPYes.Disabled = false;
            btnPublish.Disabled = true;
        }
    }

    [AjaxMethod]
    public void SP(string pass)//审批--已添加登陆部门人员
    {
        if (taSPYJ.Text.Trim() == "")
        {
            Ext.Msg.Alert("提示", "请填写审批意见!").Show();
            return;
        }
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            List<string> li = SessionBox.GetUserSession().Role;//当前用户角色列表
            DataTable dt = UserHandle.GetProcessRole2(this.PageTag, Button3.ID);//拥有审批权限角色列表
            string rolelist = "";
            foreach (var row in dt.Select())//获取当前用户拥有审批权限角色列表
            {
                foreach (var s in li)
                {
                    if (s.Remove(s.IndexOf(',')).Trim() == row["ROLEID"].ToString().Trim())
                    {
                        rolelist += row["ROLEID"].ToString().Trim() + ",";
                    }
                }

            }
            if (rolelist.Length > 0)
            {
                rolelist = rolelist.Substring(0, rolelist.Length - 1);
            }
            if (pass == "T")
            {
                GhtnTech.SEP.OraclDAL.DALAUDITupdate du = new GhtnTech.SEP.OraclDAL.DALAUDITupdate();
                du.UpdateAUDIT(taSPYJ.Text.Trim(), SessionBox.GetUserSession().PersonNumber, "已审核", sm.SelectedRow.RecordID.Trim(), rolelist);


                string oracletext = "select * from AUDITHAZARD where AUDITHAZARD.PASS is null and HAZARDSID = " + sm.SelectedRow.RecordID.Trim() + "";

                if (OracleHelper.Query(oracletext).Tables[0].Rows.Count == 0)
                {
                    //更新临时表状态
                    StringBuilder strSql1 = new StringBuilder();
                    strSql1.Append("update HAZARDS_TEMP set ");
                    strSql1.Append("ISPASS=:ISPASS ");
                    strSql1.Append(" where HAZARDSTEMPID=:HAZARDSTEMPID");
                    OracleParameter[] parameters1 = {
					new OracleParameter(":HAZARDSTEMPID", OracleType.Number,9),//ID
                    new OracleParameter(":ISPASS", OracleType.VarChar,10)
                                           };
                    parameters1[0].Value = sm.SelectedRow.RecordID.Trim();
                    parameters1[1].Value = "已审核";
                    OracleHelper.ExecuteSql(strSql1.ToString(), parameters1);
                    btn_Save.Disabled = true;
                    btnApprove.Disabled = true;
                    btnDelete.Disabled = true;
                    btnPYes.Disabled = true;
                    btnPublish.Disabled = false;
                }
            }
            else
            {
                GhtnTech.SEP.OraclDAL.DALAUDITupdate du = new GhtnTech.SEP.OraclDAL.DALAUDITupdate();
                du.UpdateAUDIT(taSPYJ.Text.Trim(), SessionBox.GetUserSession().PersonNumber, "不通过", sm.SelectedRow.RecordID.Trim(), rolelist);

                //更新临时表状态
                StringBuilder strSql1 = new StringBuilder();
                strSql1.Append("update HAZARDS_TEMP set ");
                strSql1.Append("ISPASS=:ISPASS ");
                strSql1.Append(" where HAZARDSTEMPID=:HAZARDSTEMPID");
                OracleParameter[] parameters1 = {
					new OracleParameter(":HAZARDSTEMPID", OracleType.Number,9),//ID
                    new OracleParameter(":ISPASS", OracleType.VarChar,10)
                                           };
                parameters1[0].Value = sm.SelectedRow.RecordID.Trim();
                parameters1[1].Value = "不通过";
                OracleHelper.ExecuteSql(strSql1.ToString(), parameters1);
                btn_Save.Disabled = false;
                btnApprove.Disabled = false;
                btnDelete.Disabled = false;
                btnPYes.Disabled = true;
                btnPublish.Disabled = true;
            }
            GVLoad(Hidden1.Value.ToString().Trim(), Hidden2.Value.ToString().Trim());
            Ext.Msg.Alert("提示", "审批完成!").Show();
            SPWindow.Hide();
        }
    }

    [AjaxMethod]
    public void btnSH()//点击审核按钮
    {
        SPWindow.Show();
        taSPYJ.Text = "同意";
    }

    [AjaxMethod]
    public void Publish()//已发布--已添加部门
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            DataTable dt = OracleHelper.Query("select * from HAZARDS_TEMP where HAZARDSTEMPID='" + sm.SelectedRow.RecordID + "'").Tables[0];
            string[] h_number = GetLiushuiNo(sm.SelectedRow.RecordID.ToString());
            string number = h_number[0];
            string processid = h_number[1];
            //插入主表
            string sql = "insert into HAZARDS(PROCESSID,H_NUMBER,H_CONTENT,H_CONSEQUENCES,M_STANDARDS,M_MEASURES,REGULATORYMEASURES,DATEINPUT,PERSONID,ISPASS,RISK_TYPESNUMBER,ACCIDENT_TYPENUMBER,DEPTNUMBER,OPENINGTIME,RELEASEDEPT)";
            sql += " values ('" + processid + "','" + number + "','" + dt.Rows[0]["H_CONTENT"].ToString().Trim() + "','" + dt.Rows[0]["H_CONSEQUENCES"].ToString().Trim() + "','" + dt.Rows[0]["M_STANDARDS"].ToString().Trim() + "','" + dt.Rows[0]["M_MEASURES"].ToString().Trim() + "','" + dt.Rows[0]["REGULATORYMEASURES"].ToString().Trim() + "',sysdate,'','已发布','" + dt.Rows[0]["RISK_TYPESNUMBER"].ToString().Trim() + "','" + dt.Rows[0]["ACCIDENT_TYPENUMBER"].ToString().Trim() + "','" + dt.Rows[0]["DEPTNUMBER"].ToString().Trim() + "',sysdate,'" + SessionBox.GetUserSession().DeptNumber + "')";
            OracleHelper.Query(sql);
            //插入附表
            string ssql = "insert into HAZARDS_ORE(H_NUMBER,RISK_EVELNUMBER,M_OBJECTNUMBER,M_PERSONNUMBER,DIRECTLYRESPONSIBLEPERSONSNUMB,REGULATORYPARTNERSNUMBER,DATEINPUT,PERSONID,ISPASS,H_BJW,H_BM,PROCESSINGMODE,PUNISHMENTSTANDARD,NOTE)";
            ssql += " values ('" + number + "','" + dt.Rows[0]["RISK_EVELNUMBER"].ToString().Trim() + "','" + dt.Rows[0]["M_OBJECTNUMBER"].ToString().Trim() + "','" + dt.Rows[0]["M_PERSONNUMBER"].ToString().Trim() + "','" + dt.Rows[0]["DIRECTLYRESPONSIBLEPERSONSNUMB"].ToString().Trim() + "','" + dt.Rows[0]["REGULATORYPARTNERSNUMBER"].ToString().Trim() + "',sysdate,'','已发布','通用','" + dt.Rows[0]["H_CONTENT"].ToString().Trim() + "','" + dt.Rows[0]["PROCESSINGMODE"].ToString().Trim() + "','" + dt.Rows[0]["PUNISHMENTSTANDARD"].ToString().Trim() + "','" + dt.Rows[0]["NOTE"].ToString().Trim() + "')";
            OracleHelper.Query(ssql);
            OracleHelper.Query("update HAZARDS_ORE set H_F_PINYIN=F_PINYIN('" + dt.Rows[0]["H_CONTENT"].ToString().Trim() + "') where H_NUMBER=" + number);//更新别名拼音 助记码
            //更新临时表状态
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("update HAZARDS_TEMP set ");
            strSql2.Append("ISPASS=:ISPASS ");
            strSql2.Append(" where HAZARDSTEMPID=:HAZARDSTEMPID");
            OracleParameter[] parameters2 = {
					new OracleParameter(":HAZARDSTEMPID", OracleType.Number,9),//ID
                    new OracleParameter(":ISPASS", OracleType.VarChar,10)
                                           };
            parameters2[0].Value = sm.SelectedRow.RecordID;
            parameters2[1].Value = "已发布";
            OracleHelper.ExecuteSql(strSql2.ToString(), parameters2);

            Ext.Msg.Alert("提示", "发布成功！").Show();
            GVLoad(Hidden1.Value.ToString().Trim(), Hidden2.Value.ToString().Trim());
        }
    }

    [AjaxMethod]
    public void Delete()//删除
    {
        //删除操作 以后要补存入历史和禁用方法
        //审核表也没删
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HAZARDS_TEMP ");
            strSql.Append(" where HAZARDSTEMPID=:HAZARDSTEMPID");
            OracleParameter[] parameters = {
					new OracleParameter(":HAZARDSTEMPID", OracleType.Number,9)//ID
                                           };
            parameters[0].Value = sm.SelectedRow.RecordID.Trim();
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            Ext.Msg.Alert("提示", "删除成功!").Show();
            GVLoad(Hidden1.Value.ToString().Trim(), Hidden2.Value.ToString().Trim());
            btn_Save.Disabled = true;
            btnApprove.Disabled = true;
            btnDelete.Disabled = true;
            btnPYes.Disabled = true;
            btnPublish.Disabled = true;
        }

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

    private void Publishworktasks_temp(string ID)//发布工作任务临时表
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update worktasks_temp set STATUS=:STATUS ");
        strSql.Append("where WORKTASKID=:WORKTASKID");
        OracleParameter[] parameters = {
                    new OracleParameter(":STATUS", OracleType.VarChar,10),//ID
                    new OracleParameter(":WORKTASKID", OracleType.Number,9)
                                           };
        parameters[0].Value = "已发布";
        parameters[1].Value = ID;
        OracleHelper.ExecuteSql(strSql.ToString(), parameters);
    }

    private void PublishPROCESS_TEMP(string ID)//发布工序临时表
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update PROCESS_TEMP set STATUS=:STATUS ");
        strSql.Append("where PROCESSID=:PROCESSID");
        OracleParameter[] parameters = {
                    new OracleParameter(":STATUS", OracleType.VarChar,10),//ID
                    new OracleParameter(":PROCESSID", OracleType.Number,9)
                                           };
        parameters[0].Value = "已发布";
        parameters[1].Value = ID;
        OracleHelper.ExecuteSql(strSql.ToString(), parameters);
    }

    private string[] GetLiushuiNo(string ID)//危险源流水号获取准备工作
    {
        string oracletext = "select * from HAZARDS_TEMP where HAZARDSTEMPID=" + ID;
        DataTable dttemp = OracleHelper.Query(oracletext).Tables[0];
        if (dttemp.Rows[0]["ISFROMTEMP"].ToString().Trim() == "T")//上级来自临时表
        {
            //发布工作任务
            oracletext = "select worktasks_temp.* from worktasks_temp inner join process_temp on worktasks_temp.worktaskid=process_temp.worktaskid where process_temp.PROCESSID=" + dttemp.Rows[0]["PROCESSNUMBER"].ToString().Trim();
            DataTable dtwork = OracleHelper.Query(oracletext).Tables[0];
            oracletext = "select * from WORKTASKSNO where ZYID=" + dtwork.Rows[0]["PROFESSIONALID"].ToString().Trim();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WORKTASKS (WORKTASK,PROFESSIONALID,TASK_NUMBER) values( ");
            strSql.Append(":WORKTASK,:PROFESSIONALID,:TASK_NUMBER)");
            OracleParameter[] parameters = {
                    new OracleParameter(":WORKTASK", OracleType.VarChar,200),
                    new OracleParameter(":PROFESSIONALID", OracleType.Number,9),
                    new OracleParameter(":TASK_NUMBER", OracleType.VarChar,8)
                                           };
            parameters[0].Value = dtwork.Rows[0]["WORKTASK"].ToString().Trim();
            parameters[1].Value = int.Parse(dtwork.Rows[0]["PROFESSIONALID"].ToString());
            string wtno = PublicMethod.GetWORKTASKSNO(dtwork.Rows[0]["PROFESSIONALID"].ToString().Trim());
            parameters[2].Value = wtno;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            Publishworktasks_temp(dtwork.Rows[0]["WORKTASKID"].ToString().Trim());//变更临时表状态

            //发布工序
            oracletext = "select WORKTASKID from WORKTASKS where TASK_NUMBER='" + wtno + "' and PROFESSIONALID=" + dtwork.Rows[0]["PROFESSIONALID"].ToString().Trim();
            string WORKTASKID = OracleHelper.Query(oracletext).Tables[0].Rows[0]["WORKTASKID"].ToString();//获取刚才发布工作任务的ID
            oracletext = "select process_temp.* from process_temp where process_temp.PROCESSID=" + dttemp.Rows[0]["PROCESSNUMBER"].ToString().Trim();
            DataTable dtprocess = OracleHelper.Query(oracletext).Tables[0];
            string prono = PublicMethod.GetPROCESSNO(WORKTASKID);

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("insert into PROCESS (NAME,WORKTASKID,PROFESSIONALID,DANWEIID,ISOMUX,SERIALNUMBER) values( ");
            strSql1.Append(":NAME,:WORKTASKID,:PROFESSIONALID,:DANWEIID,:ISOMUX,:SERIALNUMBER)");
            OracleParameter[] parameters1 = {
                    new OracleParameter(":NAME", OracleType.VarChar,200),
                    new OracleParameter(":WORKTASKID", OracleType.Number,9),
                    new OracleParameter(":PROFESSIONALID", OracleType.Number,9),
                    new OracleParameter(":DANWEIID", OracleType.VarChar,8),
                    new OracleParameter(":ISOMUX", OracleType.VarChar,8),
                    new OracleParameter(":SERIALNUMBER", OracleType.VarChar,8)
                                           };
            parameters1[0].Value = dtprocess.Rows[0]["NAME"].ToString();
            parameters1[1].Value = int.Parse(WORKTASKID);
            parameters1[2].Value = int.Parse(dtwork.Rows[0]["PROFESSIONALID"].ToString());
            parameters1[3].Value = dtprocess.Rows[0]["DEPTNUMBER"].ToString();
            parameters1[4].Value = prono;
            oracletext = "select nvl(max(SERIALNUMBER),0) A from PROCESS where WORKTASKID=" + WORKTASKID;
            parameters1[5].Value = int.Parse(OracleHelper.Query(oracletext).Tables[0].Rows[0]["A"].ToString()) + 1;
            OracleHelper.ExecuteSql(strSql1.ToString(), parameters1);
            PublishPROCESS_TEMP(dtprocess.Rows[0]["PROCESSID"].ToString().Trim());//变更临时表状态

            oracletext = "select * from PROCESS where ISOMUX='" + prono + "' and WORKTASKID=" + WORKTASKID;
            string processid = OracleHelper.Query(oracletext).Tables[0].Rows[0]["PROCESSID"].ToString();//获取刚才发布工序的ID

            return new string[] { PublicMethod.GetHAZARDSNO(processid), processid };
        }
        else
        {
            return new string[] { PublicMethod.GetHAZARDSNO(dttemp.Rows[0]["PROCESSNUMBER"].ToString().Trim()), dttemp.Rows[0]["PROCESSNUMBER"].ToString().Trim() };
        }

    }


    #region 绑定树 --已添加绑定部门

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
                AddGZRW_Temp(nodes, nodeID.Substring(1));
                AddGZRW(nodes, nodeID.Substring(1));
                break;
            case "w":
                AddGX(nodes, nodeID.Substring(1));
                break;
            case "a":
                AddGX_Temp(nodes, nodeID.Substring(1));
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
    private void AddGZRW_Temp(Coolite.Ext.Web.TreeNodeCollection nodes, string p)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from WORKTASKS_TEMP where PROFESSIONALID={0}  and WORKTASKS_TEMP.STATUS!='已发布' and DEPTNUMBER='{1}'", p, SessionBox.GetUserSession().DeptNumber));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r["WORKTASK"].ToString() + "<span>&nbsp;</span>";
            asyncNode.NodeID = "a" + r["WORKTASKID"].ToString();
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

    private void AddGX_Temp(Coolite.Ext.Web.TreeNodeCollection nodes, string p)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from PROCESS_TEMP where WORKTASKID={0} and STATUS!='已发布' and DEPTNUMBER='{1}'", p, SessionBox.GetUserSession().DeptNumber));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r["NAME"].ToString() + "<span>&nbsp;</span>";
            asyncNode.NodeID = "b" + r["PROCESSID"].ToString();
            asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad('{0}','T');", r["PROCESSID"].ToString().Trim());
            asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    #endregion

    #region 树右键

    #region 新增工作任务
    [AjaxMethod]
    public void maddGZRW(string id)
    {
        if (id == "-1")
        {
            return;
        }
        Ext.Msg.Show(new MessageBox.Config
        {
            Title = "新增工作任务",
            Message = "请在下面录入工作任务名称：",
            Width = 300,
            ButtonsConfig = new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.SaveGZRW(text,'" + id + "')",
                    Text = "保存"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取消"
                }
            },
            Buttons = MessageBox.Button.YESNO,
            Multiline = true,
            AnimEl = miAddGZRW.ClientID,
            //Fn = new JFunction { Fn = "showResultText" }
        });

    }
    #endregion

    #region 修改工作任务
    [AjaxMethod]
    public void meditGZRW(string id)
    {
        if (id == "-1")
        {
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        Ext.Msg.Show(new MessageBox.Config
        {
            Title = "修改工作任务",
            Message = "工作任务名称：",
            Width = 300,
            Value = dc.WorktasksTemp.First(p => p.Worktaskid == decimal.Parse(id.Trim().Substring(1))).Worktask,
            ButtonsConfig = new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.SaveGZRW(text,'" + id + "')",
                    Text = "保存"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取消"
                }
            },
            Buttons = MessageBox.Button.YESNO,
            Multiline = true,
            AnimEl = miEditGZRW.ClientID,
        });

    }
    #endregion

    #region 删除工作任务
    [AjaxMethod]
    public void mdelGZRW(string id)
    {
        if (id == "-1")
        {
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        if (id.Trim().Substring(0, 1) == "a")
        {
            if (dc.ProcessTemp.Where(p => p.Worktaskid == decimal.Parse(id.Trim().Substring(1))).Count() > 0)
            {
                Ext.Msg.Alert("提示", "工作任务下已有工序，不能删除!").Show();
                return;
            }
            Ext.Msg.Confirm("提示", "是否确定删除?", new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.DelGZRW(" + id.Trim().Substring(1) + ");",
                    Text = "确 定"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取 消"
                }
            }).Show();
        }
    }

    [AjaxMethod]
    public void DelGZRW(int id)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var wt = dc.WorktasksTemp.First(p => p.Worktaskid == id);
        dc.WorktasksTemp.DeleteOnSubmit(wt);
        dc.SubmitChanges();
        fatherReload();
        Ext.Msg.Alert("提示", "删除成功!").Show();
    }
    #endregion

    #region 工作任务 数据保存

    [AjaxMethod]
    public void SaveGZRW(string WORKTASK, string id)
    {
        if (WORKTASK.Trim() == "")
        {
            Ext.Msg.Alert("提示", "工作任务内容不能为空!").Show();
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        if (id.Trim().Substring(0, 1) == "z")
        {
            if (dc.WorktasksTemp.Where(p => p.Worktask == WORKTASK && p.Status == "保存" && p.Professionalid == decimal.Parse(id.Trim().Substring(1))).Count() > 0)
            {
                Ext.Msg.Alert("提示", "已经添加的工作任务!").Show();
                return;
            }
            if (dc.Worktasks.Where(p => p.Worktask == WORKTASK && p.Professionalid == decimal.Parse(id.Trim().Substring(1))).Count() > 0)
            {
                Ext.Msg.Alert("提示", "标准库已经存在的工作任务!").Show();
                return;
            }
            WorktasksTemp wt = new WorktasksTemp
            {
                Worktask = WORKTASK,
                Dateinput = System.DateTime.Today,
                Deptnumber = SessionBox.GetUserSession().DeptNumber,
                Personid = SessionBox.GetUserSession().PersonNumber,
                Professionalid = decimal.Parse(id.Trim().Substring(1)),
                Status = "保存",
            };
            dc.WorktasksTemp.InsertOnSubmit(wt);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "保存成功!").Show();
        }
        if (id.Trim().Substring(0, 1) == "a")
        {
            var wt = dc.WorktasksTemp.First(p => p.Worktaskid == decimal.Parse(id.Trim().Substring(1)));
            wt.Worktask = WORKTASK;
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "保存成功!").Show();
        }

        if (id.Trim().Substring(0, 1) == "a")
        {
            fatherReload();
        }
        else
        {
            nodeReload();
        }

    }

    #endregion

    #region 树节点刷新
    private void nodeReload()
    {
        string js = "var selNode = #{tpZY}.getSelectionModel().getSelectedNode();";
        js += "selNode.reload();";
        Ext.DoScript(js);
    }

    private void fatherReload()
    {
        string js = "var selNode = #{tpZY}.getSelectionModel().getSelectedNode();";
        js += "selNode = selNode.parentNode;";
        js += "selNode.reload();";//保存成功后重载父节点下的节点
        Ext.DoScript(js);
    }
    #endregion

    #region 新增工序
    [AjaxMethod]
    public void maddGX(string id)
    {
        if (id == "-1")
        {
            return;
        }
        Ext.Msg.Show(new MessageBox.Config
        {
            Title = "新增工序",
            Message = "请在下面录入工序名称：",
            Width = 300,
            ButtonsConfig = new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.SaveGX(text,'" + id + "')",
                    Text = "保存"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取消"
                }
            },
            Buttons = MessageBox.Button.YESNO,
            Multiline = true,
            AnimEl = miAddGX.ClientID
        });
    }
    #endregion

    #region 修改工序
    [AjaxMethod]
    public void meditGX(string id)
    {
        if (id == "-1")
        {
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        Ext.Msg.Show(new MessageBox.Config
        {
            Title = "修改工序",
            Message = "工序名称：",
            Width = 300,
            Value = dc.ProcessTemp.First(p => p.Processid == decimal.Parse(id.Trim().Substring(1))).Name,
            ButtonsConfig = new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.SaveGX(text,'" + id + "')",
                    Text = "保存"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取消"
                }
            },
            Buttons = MessageBox.Button.YESNO,
            Multiline = true,
            AnimEl = miEditGX.ClientID,
        });

    }
    #endregion

    #region 删除工作任务
    [AjaxMethod]
    public void mdelGX(string id)
    {
        if (id == "-1")
        {
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        if (id.Trim().Substring(0, 1) == "b")
        {
            if (dc.HazardsTemp.Where(p => p.Processnumber == id.Trim().Substring(1) && p.Ispass=="保存").Count() > 0)
            {
                Ext.Msg.Alert("提示", "工序下已有危险源信息，不能删除!").Show();
                return;
            }
            Ext.Msg.Confirm("提示", "是否确定删除?", new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.DelGX(" + id.Trim().Substring(1) + ");",
                    Text = "确 定"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取 消"
                }
            }).Show();
        }
    }

    [AjaxMethod]
    public void DelGX(int id)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var wt = dc.ProcessTemp.First(p => p.Processid == id);
        dc.ProcessTemp.DeleteOnSubmit(wt);
        dc.SubmitChanges();
        fatherReload();
        Ext.Msg.Alert("提示", "删除成功!").Show();
    }
    #endregion

    #region 工序 数据保存

    [AjaxMethod]
    public void SaveGX(string ProcessName, string id)
    {
        if (ProcessName.Trim() == "")
        {
            Ext.Msg.Alert("提示", "工序内容不能为空!").Show();
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        if (id.Trim().Substring(0, 1) == "a")
        {
            if (dc.ProcessTemp.Where(p => p.Name == ProcessName && p.Status == "保存" && p.Worktaskid == decimal.Parse(id.Trim().Substring(1))).Count() > 0)
            {
                Ext.Msg.Alert("提示", "已经添加的工序!").Show();
                return;
            }
            //if (dc.Process.Where(p => p.Name == ProcessName && p.Worktaskid == decimal.Parse(id.Trim().Substring(1))).Count() > 0)
            //{
            //    Ext.Msg.Alert("提示", "标准库已经存在的工序!").Show();
            //    return;
            //}
            decimal personid = -1;
            try
            {
                personid = decimal.Parse(SessionBox.GetUserSession().PersonNumber);
            }
            catch
            {
            }
            ProcessTemp wt = new ProcessTemp
            {
                Name = ProcessName,
                Dateinput = System.DateTime.Today,
                Deptnumber = SessionBox.GetUserSession().DeptNumber,
                Personid = personid,
                Worktaskid = decimal.Parse(id.Trim().Substring(1)),
                Status = "保存",
            };

            dc.ProcessTemp.InsertOnSubmit(wt);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "保存成功!").Show();
        }
        if (id.Trim().Substring(0, 1) == "b")
        {
            var wt = dc.ProcessTemp.First(p => p.Processid == decimal.Parse(id.Trim().Substring(1)));
            wt.Name = ProcessName;
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "保存成功!").Show();
        }

        if (id.Trim().Substring(0, 1) == "a")
        {
            nodeReload();
        }
        else
        {
            fatherReload();
        }

    }

    #endregion

    #endregion

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
    public void GLDXLoad()
    {
        string oracletext1 = "select * from M_OBJECT where RISK_TYPESID = " + extRISK_TYPESNUMBER.SelectedItem.Value.Trim();
        Store1.DataSource = OracleHelper.Query(oracletext1);
        Store1.DataBind();
        extM_OBJECTNUMBER.SelectedIndex = -1;
    }

    protected void RowClick(object sender, AjaxEventArgs e)//单击行事件
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            string json = e.ExtraParams["Values"];
            Dictionary<string, string>[] data = JSON.Deserialize<Dictionary<string, string>[]>(json);
            string HAZARDSID = ((Dictionary<string, string>)data[0])["HAZARDSID"].Trim();
            string ISFROM = ((Dictionary<string, string>)data[0])["ISFROM"].Trim();//AHN
            string ISPASS = ((Dictionary<string, string>)data[0])["ISPASS"].Trim();
            if (ISFROM == "A")
            {
                btn_Save.Disabled = true;
                btnApprove.Disabled = true;
                btnDelete.Disabled = true;
                btnPYes.Disabled = true;
                btnPublish.Disabled = true;
            }
            else
            {
                switch (ISPASS)
                {
                    case "不通过":
                    case "保存":
                        btn_Save.Disabled = false;
                        btnApprove.Disabled = false;
                        btnDelete.Disabled = false;
                        btnPYes.Disabled = true;
                        btnPublish.Disabled = true;
                        break;
                    case "提交":
                        btn_Save.Disabled = true;
                        btnApprove.Disabled = true;
                        btnDelete.Disabled = true;
                        btnPYes.Disabled = false;
                        btnPublish.Disabled = true;
                        break;
                    case "已审核":
                        btn_Save.Disabled = true;
                        btnApprove.Disabled = true;
                        btnDelete.Disabled = true;
                        btnPYes.Disabled = true;
                        btnPublish.Disabled = false;
                        break;
                }
            }
            LoadDetailData(HAZARDSID, ISFROM);
        }

    }

    private void LoadDetailData(string ID, string kind)//加载明细信息
    {
        string basesql = "select h.h_number,h.h_content,h.h_consequences,h.m_standards,h.m_measures,h.regulatorymeasures,h.risk_typesnumber,h.accident_typenumber,ho.risk_evelnumber,ho.m_objectnumber,ho.m_personnumber,ho.directlyresponsiblepersonsnumb,ho.regulatorypartnersnumber,ho.processingmode,ho.punishmentstandard,ho.note from hazards h inner join hazards_ore ho on h.h_number=ho.h_number where 1=1";//ho.h_bjw='通用' //通用的没有了？？
        string tempsql = "select * from hazards_temp";
        DataTable dt;
        if (kind == "A")
        {
            dt = OracleHelper.Query(basesql + " and h.HAZARDSID=" + ID).Tables[0];
        }
        else
        {
            dt = OracleHelper.Query(tempsql + " where hazards_temp.HAZARDSTEMPID=" + ID).Tables[0];
        }
        //--------------新加了风险类型 为拼音检索做准备
        //extRISK_TYPESNUMBER.SelectedItem.Value = RISK_TYPESNUMBER.ToString();
        string oracletext1 = "select * from M_OBJECT where RISK_TYPESID = " + dt.Rows[0]["RISK_TYPESNUMBER"].ToString();
        Store1.DataSource = OracleHelper.Query(oracletext1);
        Store1.DataBind();
        //--------------
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
    public void btnAdd_Click()
    {
        btn_Save.Disabled = false;
        Panel1.Show();
        ClearControlValue();
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }
        btn_Save.Disabled = false;
        btnApprove.Disabled = true;
        btnDelete.Disabled = true;
        btnPYes.Disabled = true;
        btnPublish.Disabled = true;
    }

    private void ClearControlValue()
    {
        string[] group = valuegroup();
        foreach (string r in group)
        {
            Control c = Panel1.FindControl("ext" + r);
            if (c.GetType().Name.Trim() == "ComboBox")
            {
                ComboBox cbb = (ComboBox)c;
                cbb.SelectedItem.Value = "";
            }
            if (c.GetType().Name.Trim() == "TextArea")
            {
                TextArea ta = (TextArea)c;
                ta.Text = "";
            }
        }
    }

    [AjaxMethod]
    public void btnSave_Click()//已添加--人员部门
    {
        if (extACCIDENT_TYPENUMBER.SelectedIndex == -1 || extRISK_TYPESNUMBER.SelectedIndex == -1 || extH_CONTENT.Text.Trim() == "" || extH_CONSEQUENCES.Text.Trim() == "" || extM_MEASURES.Text.Trim() == "" || extM_STANDARDS.Text.Trim() == "" || extREGULATORYMEASURES.Text.Trim() == "")
        {
            Ext.Msg.Alert("提示", "请填写必填信息!").Show();
            return;
        }
        if (extPUNISHMENTSTANDARD.Text.Trim() != "")
        {
            try
            {
                Convert.ToInt32(extPUNISHMENTSTANDARD.Text.Trim());
            }
            catch
            {
                Ext.Msg.Alert("提示", "处罚标准必须为数字!").Show();
                return;
            }
        }
        StringBuilder strSql = new StringBuilder();
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string[] group = valuegroup();
        string msg = "";
        if (sm.SelectedRows.Count > 0)
        {
            string sql = "update HAZARDS_TEMP set ";
            string part = "";
            foreach (string r in group)
            {
                part += r + "='";
                Control c = Panel1.FindControl("ext" + r);
                if (c.GetType().Name.Trim() == "ComboBox")
                {
                    ComboBox cbb = (ComboBox)c;
                    part += cbb.SelectedItem.Value.Trim() + "',";
                }
                if (c.GetType().Name.Trim() == "TextArea")
                {
                    TextArea ta = (TextArea)c;
                    part += ta.Text + "',";
                }
            }
            sql += part.Substring(0, part.Length - 1);
            sql += " where HAZARDSTEMPID='" + sm.SelectedRow.RecordID.Trim() + "'";
            OracleHelper.Query(sql);
            msg = "修改成功!";
        }
        else
        {
            if (Hidden1.Value.ToString().Trim() == "" || Hidden1.Value.ToString().Trim() == "-1")
            {
                Ext.Msg.Alert("提示", "请先选择工序!").Show();
                return;
            }
            string sql = "insert into HAZARDS_TEMP(";
            string name = "";
            string value = "";
            foreach (string r in group)
            {
                name += r + ",";
                Control c = Panel1.FindControl("ext" + r);
                if (c.GetType().Name.Trim() == "ComboBox")
                {
                    ComboBox cbb = (ComboBox)c;
                    value += "'" + cbb.SelectedItem.Value.Trim() + "',";
                }
                if (c.GetType().Name.Trim() == "TextArea")
                {
                    TextArea ta = (TextArea)c;
                    value += "'" + ta.Text + "',";
                }
            }
            name += "PROCESSNUMBER,DATEINPUT,PERSONID,DEPTNUMBER,ISFROMTEMP,ISPASS";
            value += "'" + Hidden1.Value.ToString().Trim() + "',sysdate,'" + SessionBox.GetUserSession().PersonNumber + "','" + SessionBox.GetUserSession().DeptNumber + "','" + Hidden2.Value.ToString().Trim() + "','保存'";
            sql += name;
            sql += ") values (" + value + ")";
            OracleHelper.Query(sql);
            msg = "保存成功!";
        }
        Ext.Msg.Alert("提示", msg).Show();
        GVLoad(Hidden1.Value.ToString().Trim(), Hidden2.Value.ToString().Trim());
        btn_Save.Disabled = false;
        btnApprove.Disabled = false;
        btnDelete.Disabled = false;
        btnPYes.Disabled = true;
        btnPublish.Disabled = true;
    }

    private string[] valuegroup()
    {
        return new string[] { "ACCIDENT_TYPENUMBER", "M_OBJECTNUMBER", "H_CONTENT", "M_STANDARDS", "PROCESSINGMODE", "RISK_TYPESNUMBER", "M_PERSONNUMBER", "H_CONSEQUENCES", "M_MEASURES", "PUNISHMENTSTANDARD", "RISK_EVELNUMBER", "DIRECTLYRESPONSIBLEPERSONSNUMB", "REGULATORYPARTNERSNUMBER", "REGULATORYMEASURES", "NOTE" };
    }

    [AjaxMethod]
    public void GVLoad(string processid, string kind)
    {
        GhtnTech.SEP.OraclDAL.BaseTableGet btg = new GhtnTech.SEP.OraclDAL.BaseTableGet();
        Hidden1.Value = processid;
        //Hidden1.SetValue(processid);
        Hidden2.Value = kind;
        //Hidden2.SetValue(kind);
        string strWhere = "";
        strWhere += " and (HAZARDS_TEMP.DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "'";
        if (UserHandle.ValidationHandle(PermissionTag.Verify))//拥有审批权限
        {
            List<string> li = SessionBox.GetUserSession().Role;//当前用户角色列表
            DataTable dt = UserHandle.GetProcessRole2(this.PageTag, Button3.ID);//拥有审批权限角色列表
            string rolelist = "";
            foreach (var row in dt.Select())//获取当前用户拥有审批权限角色列表
            {
                foreach (var s in li)
                {
                    if (s.Remove(s.IndexOf(',')).Trim() == row["ROLEID"].ToString().Trim())
                    {
                        rolelist += row["ROLEID"].ToString().Trim() + ",";
                    }
                }

            }
            if (rolelist.Length > 0)
            {
                rolelist = rolelist.Substring(0, rolelist.Length - 1);
            }

            strWhere += " or (ISPASS ='提交' and AUDITHAZARD.PASS is null";
            if (rolelist.Length > 0)
            {
                strWhere += " and AUDITHAZARD.DEPT in (" + rolelist + ") )";
            }
        }
        if (UserHandle.ValidationHandle(PermissionTag.Publish))//拥有发布权限
        {
            strWhere += " or HAZARDS_TEMP.ISPASS='已审核'";
        }
        strWhere += ")";
        hazardsStore.DataSource = btg.GetHAZARDSForAddNew(strWhere, kind, processid);
        hazardsStore.DataBind();
        GridPanel1.Show();
        if (string.IsNullOrEmpty(Request.QueryString["TempId"]))//隐患三违提交过来时不隐藏
        {
            Panel1.Hide();
            btn_Save.Disabled = true;
        }
        else
        {
            Panel1.Show();
            btn_Save.Disabled = false;
        }

        //RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        //sm.SelectedIndex = 0;
        //sm.UpdateSelection();

        btnAdd.Disabled = false;


        btnApprove.Disabled = true;
        btnDelete.Disabled = true;
        btnPYes.Disabled = true;
        btnPublish.Disabled = true;
    }

}
