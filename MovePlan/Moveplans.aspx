<%@ Page Language="C#" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.Xsl" %>
<%@ Import Namespace="GhtnTech.SecurityFramework.BLL" %>
<%@ Import Namespace="GhtnTech.SEP.DAL" %>
<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            df_select.SelectedDate = System.DateTime.Today;
            cbb_yearmonth.SelectedDate = System.DateTime.Today;
            storeload();
            loadpos();
            cbb_person.Disabled = true;
            cbb_place.Disabled = true;
            Ext.DoScript("#{Store4}.reload();");
        }
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }
    private void storeload()
    {
        DateTime start = new DateTime(df_select.SelectedDate.Year, df_select.SelectedDate.Month, 1);
        DateTime end = new DateTime(df_select.SelectedDate.Year, df_select.SelectedDate.Month, DateTime.DaysInMonth(df_select.SelectedDate.Year, df_select.SelectedDate.Month));
        DBSCMDataContext dc = new DBSCMDataContext();
        var data = from m in dc.VMoveplan
                   where m.Maindept == SessionBox.GetUserSession().DeptNumber && (start <= m.Starttime.Value && end >= m.Starttime.Value)
                   select new
                   {
                       Name = m.Name,
                       PlaceName = m.Placename,
                       DeptName = m.Deptname,
                       PosName = m.Posname,
                       ID = m.Id,
                       PersonID = m.Personid,
                       StartTime = m.Starttime,
                       EndTime = m.Endtime,
                       MoveState = m.Movestate
                   };
        #region 直接linq查询-数据搜索速度慢，先改成上述视图
        //var data = from m in dc.Moveplan
        //           from p in dc.Person
        //           from pl in dc.Place
        //           from d in dc.Department
        //           from pos in dc.Position
        //           where m.Personid == p.Personnumber && m.Placeid == pl.Placeid && p.Deptid == d.Deptnumber && p.Posid == pos.Posid && (m.Starttime.Value.Year == df_select.SelectedDate.Year && m.Starttime.Value.Month == df_select.SelectedDate.Month)
        //           select new
        //           {
        //               Name = p.Name,
        //               PlaceName = pl.Placename,
        //               DeptName = d.Deptname,
        //               PosName = pos.Posname,
        //               ID = m.Id,
        //               PersonID = m.Personid,
        //               StartTime = m.Starttime,
        //               EndTime = m.Endtime,
        //               MoveState = m.Movestate
        //           };
        #endregion
        Store1.DataSource = data;
        Store1.DataBind();
        btn_update.Disabled = true;
        btn_delete.Disabled = true;
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        sm.SelectedRows.Clear();
        sm.UpdateSelection();
    }
    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            DBSCMDataContext dc = new DBSCMDataContext();
            var mp1 = dc.Moveplan.Where(p => p.Id == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
            foreach (var r in mp1)
            {
                if (r.Movestate.Trim() == "未走动")
                {
                    btn_update.Disabled = false;
                    btn_delete.Disabled = false;
                }
                else
                {
                    btn_update.Disabled = true;
                    btn_delete.Disabled = true;
                }
            }
        }
        else
        {
            btn_update.Disabled = true;
            btn_delete.Disabled = true;
        }
    }
    [AjaxMethod]
    public void LoadData(string action)
    {
        if (action == "edit")
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            hdnid.Value = sm.SelectedRows[0].RecordID.Trim();
            DBSCMDataContext dc = new DBSCMDataContext();
            var data = from m in dc.Moveplan
                       from p in dc.Person
                       where m.Personid == p.Personnumber && m.Id == Convert.ToInt32(hdnid.Value)
                       select new
                       {
                           PersonID = m.Personid,
                           PlaceID = m.Placeid,
                           StartTime = m.Starttime,
                           EndTime = m.Endtime,
                           PosID = p.Posid
                       };
            foreach (var d in data)
            {
                ComboBox1.SelectedItem.Value = d.PosID.Value.ToString();
                poschange();
                ComboBox2.SelectedItem.Value = d.PersonID.ToString();
                ComboBox3.SelectedItem.Value = d.PlaceID.Value.ToString();
                DateField1.SelectedDate = d.StartTime.Value;
                DateField2.SelectedDate = d.EndTime.Value;
                break;
            }
        }
        else if (action == "insert")
        {
            hdnid.Value = "";
            ComboBox1.SelectedItem.Value=null;
            ComboBox2.Disabled = true;
            //ComboBox3.Disabled = true;
            ComboBox2.SelectedItem.Value = null;
            ComboBox3.SelectedItem.Value = null;
            DateField1.SelectedDate = System.DateTime.Today;
            DateField2.SelectedDate = System.DateTime.Today;
        }
        Window1.Show();
    }
    [AjaxMethod]
    public void loadpos()
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var pos = from p in dc.Position
                  where p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  select new
                  {
                      PosID = p.Posid,
                      PosName = p.Posname
                  };
        Store3.DataSource = pos;
        Store3.DataBind();
    }
    [AjaxMethod]
    public void Datechange()
    {
        storeload();
        cbb_yearmonth.SelectedDate = df_select.SelectedDate;
    }

    [AjaxMethod]
    public void createshow()
    {
        Ext.Msg.Confirm("提示", "系统将生成选择月份的走动计划，已生成计划的人员不会重复生成，是否继续?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.CreateAllPlan();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void CreateAllPlan()
    {
        //定义计划条数，生成计划人数，已生成计划人数
        int plancount = 0, planperson = 0, noplanperson = 0;
        int year = df_select.SelectedDate.Year;
        int month = df_select.SelectedDate.Month;
        //本月天数
        int days = DateTime.DaysInMonth(year, month);
        DBSCMDataContext dc = new DBSCMDataContext();
        //取出全部模板
        var mf = from m in dc.Movefrequency
                 where m.Maindept==SessionBox.GetUserSession().DeptNumber
                 select m;
        foreach (var r in mf)
        {
            //单个模板下所有人
            var persons = from per in dc.Person
                          where per.Areadeptid == r.Deptid && per.Posid == r.Posid
                          select per;
            foreach (var person in persons)
            {
                //是否已生成过
                if (dc.Moveplan.Count(
                    p => p.Starttime.Value.Month == df_select.SelectedDate.Month
                        && p.Starttime.Value.Year == df_select.SelectedDate.Year
                        && p.Personid == person.Personnumber
                        && p.Placeid == r.Placeid
                        ) > 0)
                {
                    noplanperson++;
                }
                else
                {
                    //循环生成个人计划
                    for (int i = 1; i + r.Frequency.Value - 1 <= days; i = i + int.Parse(r.Frequency.ToString()))
                    {
                        Moveplan mp1 = new Moveplan
                        {
                            Personid = person.Personnumber,
                            Placeid = r.Placeid,
                            Starttime = new DateTime(year, month, i),
                            Endtime = new DateTime(year, month, i + int.Parse(r.Frequency.ToString()) - 1),
                            Movestate = "未走动",
                            Maindept = SessionBox.GetUserSession().DeptNumber
                        };
                        dc.Moveplan.InsertOnSubmit(mp1);
                        dc.SubmitChanges();
                        plancount++;
                    }
                    planperson++;
                }
            }
        }
        storeload();
        Ext.Msg.Alert("提示", "<b>生成计划完成!共计：</b><br />  生成计划：" + plancount.ToString() + "条；<br />  全部计划人数：" + Convert.ToString(planperson + noplanperson) + "；<br />  本次生成计划人数：" + planperson.ToString() + "。").Show();
    }
    
    [AjaxMethod]
    public void CreatePlan()
    {
        if (cbb_zhiwu.SelectedIndex==-1 || cbb_person.SelectedIndex==-1 || cbb_place.SelectedIndex==-1)
        {
            show("提示", "请选择完整信息!");
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        var mp = dc.Moveplan.Count(p => p.Starttime.Value.Month == cbb_yearmonth.SelectedDate.Month && p.Starttime.Value.Year == cbb_yearmonth.SelectedDate.Year && p.Personid == cbb_person.SelectedItem.Value && p.Placeid == Convert.ToInt32(cbb_place.SelectedItem.Value));
        if (mp > 0)
        {
            show("提示", "当月此人此地计划不能多次生成");
            return;
        }
        Person person = dc.Person.First(p => p.Personnumber == cbb_person.SelectedItem.Value);
        Movefrequency mf = dc.Movefrequency.First(p => p.Posid == Convert.ToInt32(cbb_zhiwu.SelectedItem.Value) && p.Placeid == Convert.ToInt32(cbb_place.SelectedItem.Value) && p.Deptid == person.Deptid);
        int days = DateTime.DaysInMonth(cbb_yearmonth.SelectedDate.Year, cbb_yearmonth.SelectedDate.Month);
        for (int i = 1; i + mf.Frequency.Value - 1 <= days; i = i + int.Parse(mf.Frequency.ToString()))
        {
            Moveplan mp1 = new Moveplan
            {
                Personid = cbb_person.SelectedItem.Value,
                Placeid = Convert.ToInt32(cbb_place.SelectedItem.Value),
                Starttime = new DateTime(cbb_yearmonth.SelectedDate.Year, cbb_yearmonth.SelectedDate.Month, i),
                Endtime = new DateTime(cbb_yearmonth.SelectedDate.Year, cbb_yearmonth.SelectedDate.Month, i + int.Parse(mf.Frequency.ToString()) - 1),
                Movestate = "未走动",
                Maindept = SessionBox.GetUserSession().DeptNumber
            };
            dc.Moveplan.InsertOnSubmit(mp1);
            dc.SubmitChanges();
        }
        storeload();
        show("提示", "生成成功!");
    }

    [AjaxMethod]
    public void Savedata()
    {
        if (ComboBox1.SelectedIndex == -1 || ComboBox2.SelectedIndex == -1 || ComboBox3.SelectedIndex == -1)
        {
            show("提示", "请选择完整信息!");
            return;
        }
        if (DateField1.SelectedDate > DateField2.SelectedDate)
        {
            show("提示", "日期选择有误!");
            return;
        }
        DBSCMDataContext dc = new DBSCMDataContext();
        if (hdnid.Value.ToString().Trim() == "")
        {
            Moveplan mp1 = new Moveplan
            {
                Personid = ComboBox2.SelectedItem.Value,
                Placeid = Convert.ToInt32(ComboBox3.SelectedItem.Value),
                Starttime = DateField1.SelectedDate,
                Endtime = DateField2.SelectedDate,
                Movestate = "未走动",
                Maindept = SessionBox.GetUserSession().DeptNumber
            };
            dc.Moveplan.InsertOnSubmit(mp1);
            dc.SubmitChanges();
            hdnid.Value = mp1.Id.ToString();
        }
        else
        {
            var mp1 = dc.Moveplan.Where(p => p.Id == Convert.ToInt32(hdnid.Value));
            foreach (var r in mp1)
            {
                r.Personid = ComboBox2.SelectedItem.Value;
                r.Placeid = Convert.ToInt32(ComboBox3.SelectedItem.Value);
                r.Starttime = DateField1.SelectedDate;
                r.Endtime = DateField2.SelectedDate;
            }
            dc.SubmitChanges();
        }
        storeload();
        Ext.Msg.Alert("提示", "保存成功!").Show();
    }

    [AjaxMethod]
    public void delshow()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        Ext.Msg.Confirm("提示", "是否确定删除?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Detail_Del('" + sm.SelectedRows[0].RecordID.Trim() + "');",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void Detail_Del(string ID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
            var mp1 = dc.Moveplan.Where(p => p.Id == Convert.ToInt32(ID));
            dc.Moveplan.DeleteAllOnSubmit(mp1);
            dc.SubmitChanges();
        storeload();
        Ext.Msg.Alert("提示", "删除成功!").Show();
    }

    [AjaxMethod]
    public void poschange()
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var q = dc.Person.Where(p => p.Posid == Convert.ToInt32(ComboBox1.SelectedItem.Value));
        Store2.DataSource = q;
        Store2.DataBind();
        ComboBox2.Disabled = q.Count() > 0 ? false : true;
        var mp = from m in dc.Movefrequency
                 from n in dc.Place
                 where m.Placeid == n.Placeid && m.Posid == Convert.ToInt32(ComboBox1.SelectedItem.Value)
                 select new
                 {
                     PlaceID = n.Placeid,
                     PlaceName = n.Placename
                 };
        Store4.DataSource = mp;
        Store4.DataBind();
        ComboBox3.Disabled = mp.Count() > 0 ? false : true;
    }
    protected void PersonRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var q = dc.Person.Where(p => p.Posid == Convert.ToInt32(ComboBox1.SelectedItem.Value));
        var f = from p in dc.Person
                where p.Posid == Convert.ToInt32(ComboBox1.SelectedItem.Value)
                select new
                {
                    p.Personnumber,
                    p.Name
                };
        Store2.DataSource = q;
        Store2.DataBind();
        ComboBox2.Disabled = q.Count() > 0 ? false : true;
    }
    protected void PlaceRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var mp = from n in dc.Place
                 where n.Maindeptid == SessionBox.GetUserSession().DeptNumber
                 select new
                 {
                     PlaceID = n.Placeid,
                     PlaceName = n.Placename
                 };
        Store4.DataSource = mp;
        Store4.DataBind();
        ComboBox3.Disabled = mp.Count() > 0 ? false : true;
    }
    private void show(string title, string detail)
    {
        Ext.Notification.Show(new Notification.Config
        {
            Title = title,
            BringToFront = true,
            AlignCfg = new Notification.AlignConfig
            {
                ElementAnchor = AnchorPoint.Center,
                TargetAnchor = AnchorPoint.Center,
                OffsetX = -10,
                OffsetY = -10,
                El = FormPanel1.ClientID
            },
            ShowFx = new Frame { Color = "C3DAF9", Count = 1, Options = new Fx.Config { Duration = 0.2F } },
            HideFx = new SwitchOff(),
            Width = 150,
            Html = detail
        });
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        var saveData = function () {
        GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false))); 
        }
    </script>
    <link href="../style/examples.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b><img src="{2}" height="22px" width="22px" /></span>';

        var change = function(value) {
            var color, url;
            if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '已走动') {
                color = 'green';
                url = '../Images/yzd.gif';
            }
            else if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '未走动') {
                color = '#cc0000';
                url = '../Images/wzd.gif';
            }
            else {
                color = 'red';
                url = '../Images/wzd.gif';
            }
            return String.format(template, color, value, url);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Hidden ID="GridData" runat="server" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PersonID" Type="Int" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="DeptName" />
                    <ext:RecordField Name="PosName" />
                    <ext:RecordField Name="PlaceName" />
                    <ext:RecordField Name="StartTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="EndTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="MoveState" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="false" OnRefreshData="PersonRefresh">
        <AjaxEventConfig>
            <EventMask ShowMask="false" />
        </AjaxEventConfig>
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <%--<Listeners>
            <Load Handler="#{ComboBox2}.setValue(#{ComboBox2}.store.getAt(0).get('PersonID'));" />
        </Listeners>--%>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="PosID">
                <Fields>
                    <ext:RecordField Name="PosID" Type="Int" />
                    <ext:RecordField Name="PosName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server" AutoLoad="false" OnRefreshData="PlaceRefresh">
        <AjaxEventConfig>
            <EventMask ShowMask="false" />
        </AjaxEventConfig>
        <Reader>
            <ext:JsonReader ReaderID="PlaceID">
                <Fields>
                    <ext:RecordField Name="PlaceID" Type="Int" />
                    <ext:RecordField Name="PlaceName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <%--<Listeners>
            <Load Handler="#{ComboBox3}.setValue(#{ComboBox3}.store.getAt(0).get('PlaceID'));" />
        </Listeners>--%>
    </ext:Store>
 
    <ext:Window 
        ID="FormPanel1" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="生成计划"
        AutoHeight="true"
        Width="275px"
        Modal="true"
        ShowOnLoad="false"
        X="100" Y="60">
        <Body>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="65">
                <ext:Anchor Horizontal="95%">
                    <ext:DateField ID="cbb_yearmonth" runat="server" FieldLabel="计划时间" Format="yyyy年MM月" Disabled="true">
                    </ext:DateField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                    ID="cbb_zhiwu" 
                    runat="server" 
                    FieldLabel="职务" 
                    BlankText="职务不能为空!" 
                    AllowBlank="false" 
                    EmptyText="请选择职务...."
                    DisplayField="PosName" 
                    ValueField="PosID" 
                    StoreID="Store3" 
                    Editable="false" 
                    TypeAhead="true" 
                    Mode="Local"
                    ForceSelection="true" 
                    TriggerAction="All" 
                    SelectOnFocus="true"
                    >
                        <Listeners>
                            <Select Handler="#{cbb_person}.clearValue(); #{Store2}.reload();" />
                        </Listeners>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_person" 
                        runat="server" 
                        FieldLabel="走动人员" 
                        BlankText="人员不能为空!" 
                        AllowBlank="false" 
                        EmptyText="请选择人员...."
                        DisplayField="Name" 
                        ValueField="Personnumber" 
                        StoreID="Store2"
                        TypeAhead="true" 
                        Mode="Local"
                        ForceSelection="true" 
                        TriggerAction="All"
                        >
                        <Listeners>
                            <Select Handler="#{cbb_place}.clearValue(); #{Store4}.reload();" />
                        </Listeners>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_place" 
                        runat="server" 
                        FieldLabel="走动地点" 
                        BlankText="地点不能为空!" 
                        AllowBlank="false" 
                        EmptyText="请选择点...."
                        DisplayField="PlaceName" 
                        ValueField="PlaceID" 
                        StoreID="Store4"
                        TypeAhead="true" 
                        Mode="Local"
                        ForceSelection="true" 
                        TriggerAction="All"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="btn_createplan" runat="server" Icon="Disk" Text="生成计划">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.CreatePlan();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    
    <ext:Window 
        ID="Window1" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="信息维护"
        AutoHeight="true"
        Width="275px"
        Modal="true"
        ShowOnLoad="false"
        X="140" Y="60">
        <Body>
            <ext:Hidden ID="hdnid" runat="server">
            </ext:Hidden>
            <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="65">
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                    ID="ComboBox1" 
                    runat="server" 
                    FieldLabel="职务" 
                    BlankText="职务不能为空!" 
                    AllowBlank="false" 
                    EmptyText="请选择职务...."
                    DisplayField="PosName" 
                    ValueField="PosID" 
                    StoreID="Store3" 
                    Editable="false" 
                    TypeAhead="true" 
                    Mode="Local"
                    ForceSelection="true" 
                    TriggerAction="All" 
                    SelectOnFocus="true"
                    >
                    <Listeners>
                        <Select Handler="#{ComboBox2}.clearValue(); #{Store2}.reload();" />
                    </Listeners>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="ComboBox2" 
                        runat="server" 
                        FieldLabel="走动人员" 
                        BlankText="人员不能为空!" 
                        AllowBlank="false" 
                        EmptyText="请选择人员...."
                        DisplayField="Name" 
                        ValueField="Personnumber" 
                        StoreID="Store2"
                        TypeAhead="true" 
                        Mode="Local"
                        ForceSelection="true" 
                        TriggerAction="All"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="ComboBox3" 
                        runat="server" 
                        FieldLabel="走动地点" 
                        BlankText="地点不能为空!" 
                        AllowBlank="false" 
                        EmptyText="请选择点...."
                        DisplayField="PlaceName" 
                        ValueField="PlaceID" 
                        StoreID="Store4"
                        TypeAhead="true" 
                        Mode="Local"
                        ForceSelection="true" 
                        TriggerAction="All"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:DateField ID="DateField1" runat="server" FieldLabel="计划开始" Format="yyyy-MM-dd">
                    </ext:DateField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:DateField ID="DateField2" runat="server" FieldLabel="计划截止" Format="yyyy-MM-dd">
                    </ext:DateField>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Icon="Disk" Text="保存">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.Savedata();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true"
        Title="走动计划"
         
        Collapsible="false"
        Width="890px"
        >
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
		        <ext:Column ColumnID="NIid" Header="编号" Width="50" DataIndex="PersonID" />
                <ext:Column Header="走动人员" Width="80" Sortable="true" DataIndex="Name" />
                <ext:Column Header="部门" Width="100" Sortable="true" DataIndex="DeptName" />
                <ext:Column Header="职务" Width="150" Sortable="true" DataIndex="PosName" />
                <ext:Column Header="走动地点" Width="200" Sortable="true" DataIndex="PlaceName" />
                <ext:Column Header="计划开始" Width="80" Sortable="true" DataIndex="StartTime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="计划截止" Width="80" Sortable="true" DataIndex="EndTime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="走动状态" Width="150" Sortable="true" DataIndex="MoveState" >
                <Renderer Fn="change" />
                </ext:Column>
		    </Columns>
        </ColumnModel>
        <Plugins>
            <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                 <Filters>
                       <ext:StringFilter DataIndex="PersonID" />
                       <ext:StringFilter DataIndex="Name" />
                       <ext:StringFilter DataIndex="DeptName" />
                       <ext:StringFilter DataIndex="PosName" />
                       <ext:StringFilter DataIndex="PlaceName" />
                       <ext:DateFilter DataIndex="StartTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:DateFilter DataIndex="EndTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:ListFilter DataIndex="MoveState" Options="未走动,走动中,已走动" />
                 </Filters>
             </ext:GridFilters>
        </Plugins>
        <LoadMask ShowMask="true" Msg="数据加载中..." />
        <BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
         </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:Label runat="server" ID="label1" Text="选择时间："></ext:Label>
                    <ext:DateField runat="server" ID="df_select" Format="yyyy年MM月">
                    <Listeners>
                        <Select Handler="Coolite.AjaxMethods.Datechange();" />
                    </Listeners>
                    </ext:DateField>
                    <ext:ToolbarSeparator />
                    <ext:Button runat="server" ID="ben_plan" Icon="Add" Text="生成计划" >
                        <Listeners>
                            <%--<Click Handler="#{FormPanel1}.show();" />--%>
                            <Click Handler="Coolite.AjaxMethods.createshow();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button runat="server" ID="btn_add" Icon="ReportAdd" Text="新增">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.LoadData('insert');" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_update" Icon="ReportEdit" Text="修改" Disabled="true">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.LoadData('edit');" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_delete" Icon="ReportDelete" Text="删除" Disabled="true">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.delshow();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
        </SelectionModel>
        <AjaxEvents>
            <Click OnEvent="RowClick"></Click>
        </AjaxEvents>
        <Listeners>
            <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height - 5); }" />
        </Listeners>
     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </div>
    </form>
</body>
</html>
