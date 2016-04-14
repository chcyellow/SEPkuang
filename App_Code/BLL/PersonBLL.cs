using System;
using System.Collections.Generic;
using System.Data;
using DotNet.Frameworks.NParsing.Factory;
using DotNet.Frameworks.NParsing.Interface;
using DotNet.Utilities;
    public class PersonBLL
    {
        private readonly IObHelper<Person> _PersonDAL = new Person().Helper();//ObHelper.Create<Employe>();

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="model">员工信息</param>
        /// <returns>
        /// 0 添加失败
        /// 1 添加成功
        /// 2 员工编号已存在
        /// 3 员工已存在
        /// </returns>
        public int Add(Person model)
        {
            try
            {
                //判断EmployeID是否存在
                IObParameter p = new Person().Property("PERSONID") == model.PERSONID;//ObParameter.Create<Employe>("EmployeID", DbSymbol.Equal, model.EmployeID);
                if (_PersonDAL.Query(p).Exists()/*_PersonDAL.Exists(p)*/)
                    return 2;

                //判断员工是否存在(假设以员工姓名和性别为确一个员工)
                p = new Person().Property("NAME") == model.NAME;//ObParameter.Create<Employe>("EmployeName", DbSymbol.Equal, model.EmployeName);
                p.And(new Person().Property("SEX") == model.SEX/*ObParameter.Create<Employe>("Gender", DbSymbol.Equal, model.Gender)*/);

                if (_PersonDAL.Query(p).Exists()/*_PersonDAL.Exists(p)*/)
                    return 3;

                //添加员工信息
                return Convert.ToInt32(_PersonDAL.Add(model)) == 1 ? 1 : 0; //_PersonDAL.Add(model) ? 1 : 0;
            }
            catch (Exception ex)
            {
                //记录错误日志
                LogUtil.WriteLog(ex);
                return 0;
            }
        }

        public int GetRecordCount()
        {
            return _PersonDAL.Query().Count();
        }
        /// <summary>
        /// 删除一个员工信息
        /// </summary>
        /// <param name="employeID">员工编号</param>
        /// <returns></returns>
        public bool Delete(string employeID)
        {
            try
            {
                IObParameter p = new Person().Property("EmployeID") == employeID; //ObParameter.Create<Employe>("EmployeID", DbSymbol.Equal, employeID);
                return Convert.ToInt32(_PersonDAL.Delete(p)) == 1;//_PersonDAL.Delete(p)
            }
            catch (Exception ex)
            {
                //记录错误日志
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取员工分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="count">总记录数</param>
        /// <returns></returns>
        public IList<Person> GetList(int pageSize, int pageIndex, out int count)
        {
            //排序
            var s = new Person().Sort("PERSONID", false);//ObSort.Create<Employe>("CreateTime", false);
            //获取员工分页列表)
            return _PersonDAL.Query(s).ToList(pageSize, pageIndex, out count);//_PersonDAL.GetList(pageSize, pageIndex, s, out count);
        }

        /// <summary>
        /// 获取所有员工DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            return _PersonDAL.Query().ToTable();
        }

        public DataTable GetPerson(string personNumber)
        {
            IObParameter p = new Person().Property("PERSONNUMBER") == personNumber;
            return _PersonDAL.Query(p).ToTable();
        }
        public DataTable GetPersonByDept(string deptid)
        {
            if (deptid.Trim() != "")
            {
                IObParameter p = new Person().Property("MAINDEPTID") == deptid;
                return _PersonDAL.Query(p).ToTable();
            }
            else
            {
                return _PersonDAL.Query().ToTable();
            }
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="model">员工信息</param>
        /// <returns>
        /// 0 修改失败
        /// 1 修改成功
        /// 2 员工已存在
        /// </returns>
        public int Update(Person model)
        {
            try
            {
                //取出未修改的信息
                IObParameter p = new Person().Property("PERSONID") == model.PERSONID; //ObParameter.Create<Employe>("EmployeID", DbSymbol.Equal, model.EmployeID);
                Person oldModel = _PersonDAL.Query(p).ToModel();//_EmployeDAL.GetModel(p);
                //复盖不能修改的信息
                model.PERSONNUMBER = oldModel.PERSONNUMBER;
                model.NAME = oldModel.NAME;
                model.SEX = model.SEX;
                model.POSTID = model.POSTID;
                model.MAINDEPTID = model.MAINDEPTID;
                model.DEPTID = model.DEPTID;

                if (model.NAME != oldModel.NAME ||
                    model.PERSONNUMBER != oldModel.PERSONNUMBER)
                {
                    //如果修改了员工姓名或性别，就需要判断唯一性
                    IObParameter pExists = new Person().Property("PERSONID") != model.PERSONID;//ObParameter.Create<Employe>("EmployeID", DbSymbol.NotEqual, model.EmployeID);
                    pExists.And(new Person().Property("NAME") == model.NAME/*ObParameter.Create<Employe>("EmployeName", DbSymbol.Equal, model.EmployeName)*/);
                    pExists.And(new Person().Property("PERSONNUMBER") == model.PERSONNUMBER/*ObParameter.Create<Employe>("Gender", DbSymbol.Equal, model.Gender)*/);
                    if (_PersonDAL.Query(pExists).Exists()/*_EmployeDAL.Exists(pExists)*/)
                        return 2;
                }

                //修改员工信息
                return Convert.ToInt32(_PersonDAL.Update(model, p)) == 1 ? 1 : 0; //_EmployeDAL.Update(model, p) ? 1 : 0;
            }
            catch (Exception ex)
            {
                //记录错误日志
                LogUtil.WriteLog(ex);
                return 0;
            }
        }
    }