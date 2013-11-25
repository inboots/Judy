using System;
using System.Collections.Generic;
using System.Text;

namespace JudyCore
{

    public class CommonEntity
    {
        private string _total;

        public string total
        {
            get { return _total; }
            set { _total = value; }
        }
    }

    public class Model
    {

        private string _jid;

        public string jid
        {
            get { return _jid; }
            set { _jid = value; }
        }


        private string _jtext;

        public string jtext
        {
            get { return _jtext; }
            set { _jtext = value; }
        }



        private string _jlongitude;

        public string jlongitude
        {
            get { return _jlongitude; }
            set { _jlongitude = value; }
        }




        private string _jlatitude;

        public string jlatitude
        {
            get { return _jlatitude; }
            set { _jlatitude = value; }
        }



        private string _jaddress;

        public string jaddress
        {
            get { return _jaddress; }
            set { _jaddress = value; }
        }


        private string _jdevice;

        public string jdevice
        {
            get { return _jdevice; }
            set { _jdevice = value; }
        }

        private string _jdatetime;

        public string jdatetime
        {
            get { return _jdatetime; }
            set { _jdatetime = value; }
        }


    }
}
