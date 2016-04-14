using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GhtnTech.SEP.Model
{
    /// <summary>
    ///PROCESS 的摘要说明
    /// </summary>
    public class PROCESS
    {
        public PROCESS()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region decimal PROCESSID

        private decimal _processid;

        public decimal PROCESSID
        {
            get
            {
                return _processid;
            }
            set
            {
                if (value != _processid)
                {
                    _processid = value;
                }
            }
        }
        #endregion

        #region string NAME

        private string _name;

        public string NAME
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                }
            }
        }
        #endregion

        #region decimal WORKTASKID

        private decimal _worktaskid;

        public decimal WORKTASKID
        {
            get
            {
                return _worktaskid;
            }
            set
            {
                if (value != _worktaskid)
                {
                    _worktaskid = value;
                }
            }
        }
        #endregion

        #region decimal PROFESSIONALID

        private decimal _professionalid;

        public decimal PROFESSIONALID
        {
            get
            {
                return _professionalid;
            }
            set
            {
                if (value != _professionalid)
                {
                    _professionalid = value;
                }
            }
        }
        #endregion

        #region decimal DANWEIID

        private decimal _danweiid;

        public decimal DANWEIID
        {
            get
            {
                return _danweiid;
            }
            set
            {
                if (value != _danweiid)
                {
                    _danweiid = value;
                }
            }
        }
        #endregion

        #region string ISOMUX

        private string _isomux;

        public string ISOMUX
        {
            get
            {
                return _isomux;
            }
            set
            {
                if (value != _isomux)
                {
                    _isomux = value;
                }
            }
        }
        #endregion

        #region decimal SERIALNUMBER

        private decimal _serialnumber;
        public decimal SERIALNUMBER
        {
            get
            {
                return _serialnumber;
            }
            set
            {
                if (value != _serialnumber)
                {
                    _serialnumber = value;
                }
            }


        }
        #endregion
    }
}