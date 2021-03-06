﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GhtnTech.SEP.Model
{
    /// <summary>
    ///CS_BaseInfoSetHistory 的摘要说明
    /// </summary>
    public partial class CS_BaseInfoSetHistory
    {
        public CS_BaseInfoSetHistory()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region decimal HISTORYID
        private decimal _historyid;
        public decimal HISTORYID
        {
            get
            {
                return _historyid;
            }
            set
            {
                if (value != _historyid)
                {
                    _historyid = value;
                }
            }
        }
        #endregion

        #region decimal INFOID
        private decimal? _infoid;
        public decimal? INFOID
        {
            get
            {
                return _infoid;
            }
            set
            {
                if (value != _infoid)
                {
                    _infoid = value;
                }
            }
        }
        #endregion

        #region string INFOCODE
        private string _infocode;
        public string INFOCODE
        {
            get
            {
                return _infocode;
            }
            set
            {
                if (value != _infocode)
                {
                    _infocode = value;
                }
            }
        }
        #endregion

        #region string INFONAME
        private string _infoname;
        public string INFONAME
        {
            get
            {
                return _infoname;
            }
            set
            {
                if (value != _infoname)
                {
                    _infoname = value;
                }
            }
        }
        #endregion

        #region decimal FID
        private decimal? _fid;
        public decimal? FID
        {
            get
            {
                return _fid;
            }
            set
            {
                if (value != _fid)
                {
                    _fid = value;
                }
            }
        }
        #endregion

        #region string CODINGTYPE
        private string _codingtype;
        public string CODINGTYPE
        {
            get
            {
                return _codingtype;
            }
            set
            {
                if (value != _codingtype)
                {
                    _codingtype = value;
                }
            }
        }
        #endregion

        #region decimal CODINGL
        private decimal? _codingl;
        public decimal? CODINGL
        {
            get
            {
                return _codingl;
            }
            set
            {
                if (value != _codingl)
                {
                    _codingl = value;
                }
            }
        }
        #endregion

        #region string PDEPART
        private string _pdepart;
        public string PDEPART
        {
            get
            {
                return _pdepart;
            }
            set
            {
                if (value != _pdepart)
                {
                    _pdepart = value;
                }
            }
        }
        #endregion

        #region string PPERID
        private string _pperid;
        public string PPERID
        {
            get
            {
                return _pperid;
            }
            set
            {
                if (value != _pperid)
                {
                    _pperid = value;
                }
            }
        }
        #endregion

        #region DateTime PDAY
        private DateTime? _pday;
        public DateTime? PDAY
        {
            get
            {
                return _pday;
            }
            set
            {
                if (value != _pday)
                {
                    _pday = value;
                }
            }
        }
        #endregion

        #region string SPERID
        private string _sperid;
        public string SPERID
        {
            get
            {
                return _sperid;
            }
            set
            {
                if (value != _sperid)
                {
                    _sperid = value;
                }
            }
        }
        #endregion

        #region DateTime SDAY
        private DateTime? _sday;
        public DateTime? SDAY
        {
            get
            {
                return _sday;
            }
            set
            {
                if (value != _sday)
                {
                    _sday = value;
                }
            }
        }
        #endregion

        #region string REMARKS
        private string _remarks;
        public string REMARKS
        {
            get
            {
                return _remarks;
            }
            set
            {
                if (value != _remarks)
                {
                    _remarks = value;
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

        #region string HPERID
        private string _hperid;
        public string HPERID
        {
            get
            {
                return _hperid;
            }
            set
            {
                if (value != _hperid)
                {
                    _hperid = value;
                }
            }
        }
        #endregion

        #region DateTime HDAY
        private DateTime? _hday;
        public DateTime? HDAY
        {
            get
            {
                return _hday;
            }
            set
            {
                if (value != _hday)
                {
                    _hday = value;
                }
            }
        }
        #endregion

        #region string HREMARKS
        private string _hremarks;
        public string HREMARKS
        {
            get
            {
                return _hremarks;
            }
            set
            {
                if (value != _hremarks)
                {
                    _hremarks = value;
                }
            }
        }
        #endregion

        #region string HSTATUS
        private string _hstatus;
        public string HSTATUS
        {
            get
            {
                return _hstatus;
            }
            set
            {
                if (value != _hstatus)
                {
                    _hstatus = value;
                }
            }
        }
        #endregion
    }
}
