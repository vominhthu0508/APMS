using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.Utilities.SampleData
{
    public class SampleData
    {
        private static readonly SampleTextGenerator TextGenerator = new SampleTextGenerator();
        private static readonly List<string> _names;

        public static bool Initialized { get; set; }

        static SampleData()
        {
            _names = new List<string>
                {
                    "Typography Techniques",
                    "Creative Illustrations",
                    "Image Magic",
                    "Media Publishing"
                };
        }

        public static string GetModuleName()
        {
            return RandomSelector.Deal(_names, 1)[0];
        }

        public static List<string> GetAllModules()
        {
            return _names;
        }
    }
}
