using DotNet.Frameworks.NParsing.ComponentModel;



public class Department
{
    public Department()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        #region string DEPTNUMBER
        private string _DeptNumber;
    [ObConstraint(ObConstraint.PrimaryKey)]
        public string DEPTNUMBER
        {
            get
            {
                return _DeptNumber;
            }
            set
            {
                if (value != _DeptNumber)
                {
                    _DeptNumber = value;
                }
            }
        }
        #endregion

        #region string DEPTNAME
        private string _DeptName;
        public string DEPTNAME
        {
            get
            {
                return _DeptName;
            }
            set
            {
                if (value != _DeptName)
                {
                    _DeptName = value;
                }
            }
        }
        #endregion

        #region string FATHERID
        private string _FatherID;
        public string FATHERID
        {
            get
            {
                return _FatherID;
            }
            set
            {
                if (value != _FatherID)
                {
                    _FatherID = value;
                }
            }
        }
        #endregion
    

}