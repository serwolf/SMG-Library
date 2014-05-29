using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibKo.WAPI
{
    public class Parameters1
    {
        private String _Name = "";

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private Object _Value = new Object();

        public Object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        private String _ValueString = "";

        public String ValueString
        {
            get
            {
                ValueString = "";
                return _ValueString;
            }
            private set
            {
                var type = Value.GetType();
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.DateTime:
                        value = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", Value);
                        break;

                    default:

                        try
                        {
                            value = Value.ToString();
                        }
                        catch (Exception)
                        {
                            value = "";
                        }
                        break;


                }
                _ValueString = value;
            }
        }

    }

}
