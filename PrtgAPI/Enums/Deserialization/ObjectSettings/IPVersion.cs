﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PrtgAPI
{
    public enum IPVersion
    {
        [XmlEnum("0")]
        IPv4,

        [XmlEnum("1")]
        IPv6
    }
}
