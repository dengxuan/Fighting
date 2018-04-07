using System;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Aspects
{
    public class AspectsAttribute : Attribute
    {
        public bool Disable { get; set; } = false;
    }
}
