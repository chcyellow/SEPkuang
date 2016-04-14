using System;
using DotNet.Frameworks.NParsing.ComponentModel;

    
public class Person
{
    public Person()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region decimal PERSONID
        private decimal _PersonID;
    [ObConstraint(ObConstraint.PrimaryKey)]
        public decimal PERSONID
        {
            get
            {
                return _PersonID;
            }
            set
            {
                if (value != _PersonID)
                {
                    _PersonID = value;
                }
            }
        }

        #endregion

        #region string PERSONNUMBER
        private string _PeronNumber;
        public string PERSONNUMBER
        {
            get
            {
                return _PeronNumber;
            }
            set
            {
                if (value != _PeronNumber)
                {
                    _PeronNumber = value;
                }
            }
        }
        #endregion

        #region string NAME
        private string _Name;
        public string NAME
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                }
            }
        }
        #endregion

        #region string SEX
        private string _Sex;
        public string SEX
        {
            get
            {
                return _Sex;
            }
            set
            {
                if (value != _Sex)
                {
                    _Sex = value;
                }
            }
        }
        #endregion

        #region string TEL
        private string _Tel;
        public string TEL
        {
            get
            {
                return _Tel;
            }
            set
            {
                if (value != _Tel)
                {
                    _Tel = value;
                }
            }
        }
        #endregion

        #region decimal POSID
        private decimal? _PosID;
        public decimal? POSID
        {
            get
            {
                return _PosID;
            }
            set
            {
                if (value != _PosID)
                {
                    _PosID = value;
                }
            }
        }
        #endregion

        #region string MAINDEPTID
        private string _MainDeptID;
        public string MAINDEPTID
        {
            get
            {
                return _MainDeptID;
            }
            set
            {
                if (value != _MainDeptID)
                {
                    _MainDeptID = value;
                }
            }
        }
        #endregion

        #region string LIGHTNUMBER
        private string _LightNumber;
        public string LIGHTNUMBER
        {
            get
            {
                return _LightNumber;
            }
            set
            {
                if (value != _LightNumber)
                {
                    _LightNumber = value;
                }
            }
        }
        #endregion

        #region decimal POSTID
        private decimal? _PostID;
        public decimal? POSTID
        {
            get
            {
                return _PostID;
            }
            set
            {
                if (value != _PostID)
                {
                    _PostID = value;
                }
            }
        }
        #endregion

        #region string DEPTID
        private string _DeptID;
        public string DEPTID
        {
            get
            {
                return _DeptID;
            }
            set
            {
                if (value != _DeptID)
                {
                    _DeptID = value;
                }
            }
        }
        #endregion
    

}
