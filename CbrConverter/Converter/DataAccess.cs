using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace CbrConverter
{
    public class DataAccess
    {
        private static DataAccess _instance;

        private static string _output_dir;  
        private static string _working_dir;                                 //current selected directory   
        private static string _working_file;    //current processing file
        private static bool _processing;
        private static double _curProgress;
        private static double _totProgress;
        private static bool _ReduceSize;

        public string g_Output_dir
        {
            get
            {
                return _output_dir;
            }
            set
            {
                _output_dir = value;
            }
        }

        public string g_WorkingDir
        {
            get
            {
                return _working_dir;
            }
            set
            {
                _working_dir = value;
            }
        }

        public string g_WorkingFile
        {
            get
            {
                return _working_file;
            }
            set
            {
                _working_file = value;
            }
        }

        public bool g_Processing
        {
            get
            {
                return _processing;
            }
            set
            {
                _processing = value;
            }
        }

        public double g_curProgress
        {
            get
            {
                return _curProgress;
            }
            set
            {
                _curProgress = value;
            }
        }

        public double g_totProgress
        {
            get
            {
                return _totProgress;
            }
            set
            {
                _totProgress = value;
            }
        }

        public bool g_ReduceSize
        {
            get
            {
                return _ReduceSize;
            }
            set
            {
                _ReduceSize = value;
            }
        }

        public static DataAccess Instance
        {

            get
            {
                if (_instance == null)
                    _instance = new DataAccess();

                return _instance;
            }
        }

        private DataAccess()
        {


        }
    }
}
