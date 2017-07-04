using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed.UnitTesting
{
    /// <summary>Marks a TestMethod as not to be run, description is unused, but kept for
    ///        documentation purposes.  Added for compatability to NUnit Tests.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExplicitAttribute : Attribute
    {
        /// <summary>Human-readable description/reason for ignoring the TestMethod.
        ///        This property is ignored by the implementation.</summary>
        public string Description { get; set; }

        public ExplicitAttribute(string description)
        {
            Description = description;
        }
    }
}