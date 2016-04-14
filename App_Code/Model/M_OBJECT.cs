using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///M_OBJECT 的摘要说明
/// </summary>
/// 
namespace GhtnTech.SEP.Model
{
    public class M_OBJECT
    {
        public M_OBJECT()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region decimal M_OBJECTID

        private decimal _m_objectid;

        public decimal M_OBJECTID
        {
            get
            {
                return _m_objectid;
            }
            set
            {
                if (value != _m_objectid)
                {
                    _m_objectid = value;
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
        #region decimal RISK_TYPESID

        private decimal _risk_typesid;

        public decimal RISK_TYPESID
        {
            get
            {
                return _risk_typesid;
            }
            set
            {
                if (value != _risk_typesid)
                {
                    _risk_typesid = value;
                }
            }
        }
        #endregion

    }
}
