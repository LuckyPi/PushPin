/*******************************************************************************************************************************/
// Project: PushPin 
// Filename: SettingsClass.cs 
// Description: PushPin is designed to provide a visual interface wrapper to pcileech
// PushPin author: JT, jtestman@gmail.com
// PCILeech author: Ulf Frisk, pcileech@frizk.net
// Dependencies: PCILeech v4.6 - https://github.com/ufrisk and it's dependencies
/*******************************************************************************************************************************/

namespace PushPin
{
    class Class_syspid
    {
        private static string m_syspid = string.Empty;

        public static string Syspid
        {
            get
            {
                return m_syspid;
            }
            set
            {
                m_syspid = value;
            }
        }
    }

    class Class_userpid
    {
        private static string m_userpid = string.Empty;

        public static string Userpid
        {
            get
            {
                return m_userpid;
            }
            set
            {
                m_userpid = value;
            }
        }
    }

    class Class_pidfind
    {
        private static string m_pidfind = string.Empty;

        public static string Pidfind

        {
            get
            {
                return m_pidfind;
            }
            set
            {
                m_pidfind = value;
            }
        }
    }

    class Class_kmdaddress
    {
        private static string m_kmdaddress = "";

        public static string Kmdaddress
        {
            get
            {
                return m_kmdaddress;
            }
            set
            {
                m_kmdaddress = value;
            }
        }
    }

    class Class_process_output
    {
        private static string m_process = "";

        public static string Process
        {
            get
            {
                return m_process;
            }
            set
            {
                m_process = value;
            }
        }
    }

    class Class_source_path
    {
        private static string m_source = "";

        public static string Source
        {
            get
            {
                return m_source;
            }
            set
            {
                m_source = value;
            }
        }
    }

    class Class_destination_path
    {
        private static string m_destination = "";

        public static string Destination
        {
            get
            {
                return m_destination;
            }
            set
            {
                m_destination = value;
            }
        }
    }

    class Class_action
    {
        private static string m_action = "";

        public static string Action
        {
            get
            {
                return m_action;
            }
            set
            {
                m_action = value;
            }
        }
    }

    class Class_status
    {
        private static string m_status = "";

        public static string Status
        {
            get
            {
                return m_status;
            }
            set
            {
                m_status = value;
            }
        }
    }

    class Class_status1
    {
        private static string m_status = "";

        public static string Status
        {
            get
            {
                return m_status;
            }
            set
            {
                m_status = value;
            }
        }
    }
}
