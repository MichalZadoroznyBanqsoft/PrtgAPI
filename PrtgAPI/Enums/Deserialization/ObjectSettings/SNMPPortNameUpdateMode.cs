﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PrtgAPI
{
    public enum SNMPPortNameUpdateMode
    {
        [XmlEnum("0")]
        Manual,

        [XmlEnum("1")]
        Automatic
    }
}
