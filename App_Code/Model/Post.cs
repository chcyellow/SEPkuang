using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GhtnTech.SEP.Model
{
    /// <summary>
    ///Post 的摘要说明
    /// </summary>
    public class Post
    {
        public Post()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region decimal POSTID

        private decimal _postid;

        public decimal POSTID
        {
            get
            {
                return _postid;
            }
            set
            {
                if (value != _postid)
                {
                    _postid = value;
                }
            }
        }
        #endregion

        #region string POSTNAME

        private string _postname;

        public string POSTNAME
        {
            get
            {
                return _postname;
            }
            set
            {
                if (value != _postname)
                {
                    _postname = value;
                }
            }
        }
        #endregion

        #region decimal ZYID

        private decimal? _zyid;

        public decimal? ZYID
        {
            get
            {
                return _zyid;
            }
            set
            {
                if (value != _zyid)
                {
                    _zyid = value;
                }
            }
        }
        #endregion

        #region string SANXIANG

        private string _sanxiang;

        public string SANXIANG
        {
            get
            {
                return _sanxiang;
            }
            set
            {
                if (value != _sanxiang)
                {
                    _sanxiang = value;
                }
            }
        }
        #endregion

        #region string PTYPE

        private string _ptype;

        public string PTYPE
        {
            get
            {
                return _ptype;
            }
            set
            {
                if (value != _ptype)
                {
                    _ptype = value;
                }
            }
        }
        #endregion

        #region string STATUS

        private string _status;

        public string STATUS
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                }
            }
        }
        #endregion
    }
}
