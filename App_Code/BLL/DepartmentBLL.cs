using System.Collections.Generic;
using System.Data;
using DotNet.Frameworks.NParsing.Factory;
using DotNet.Frameworks.NParsing.Interface;


    public class DepartmentBLL
    {
        private readonly IObHelper<Person> _PersonDAL = new Person().Helper();//ObHelper.Create<Employe>();
        private readonly IObHelper<Department> _DeptDAL = new Department().Helper();
        /// <summary>
        /// 获取每个部门员工的平均年龄
        /// </summary>
        /// <returns></returns>
        public IList<Person> GetAverageageList()
        {
            //创建分组
            var group = new Department().Group("DepartmentID");//ObGroup.Create<Department>("DepartmentID");

            //增加分组聚合属性
            group.ObProperties.Add(new Person().Avg("Age")/*ObFun.Avg<Employe>("Age")*/);

            //获取数据
            return _PersonDAL.Query(group).ToList();//_PersonDAL.GetList(group);
        }

        public DataTable GetDeptList()
        {
            return _DeptDAL.Query().ToTable();
        }
        public DataTable GetDept(string deptID)
        {
            if (deptID.Trim() != "")
            {
                IObParameter p = new Department().Property("DEPTNUMBER").LikeRight(deptID + "%");
                return _DeptDAL.Query(p).ToTable();
            }
            else
            {
                return _DeptDAL.Query().ToTable();
            }
        }

    }

