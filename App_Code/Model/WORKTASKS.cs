using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///WORKTASKS 的摘要说明
/// </summary>
namespace GhtnTech.SEP.Model
{

    public class WORKTASKS
    {
        public WORKTASKS()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

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
        #region string WORKTASK

        private string _worktask;

        public string WORKTASK
        {
            get
            {
                return _worktask;
            }
            set
            {
                if (value != _worktask)
                {
                    _worktask = value;
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
        #region string TASK_NUMBER

        private string _task_number;

        public string TASK_NUMBER
        {
            get
            {
                return _task_number;
            }
            set
            {
                if (value != _task_number)
                {
                    _task_number = value;
                }
            }
        }
        #endregion

    }
}
