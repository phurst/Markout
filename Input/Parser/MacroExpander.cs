using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Markout.Input.Parser {

    public class MacroExpander : ParserBase {

        private Dictionary<string, string> _macros;

        public MacroExpander(Dictionary<string, string> macros) {
            if(macros == null) throw new ArgumentNullException("macros");
            _macros = macros;
        }

        public string ExpandMacros(string s) {
            if(s == null) throw new ArgumentNullException("s");
            StringBuilder rv = new StringBuilder();
            int index = 0;
            Regex regex = new Regex(RegexHead + string.Join("|", _macros.Keys) + RegexTail);
            MatchCollection matches = regex.Matches(s);
            foreach (Match match in matches) {
                if (match.Success) {
                    if (match.Index > 0) {
                        rv.Append(s.Substring(index, match.Index - index));
                    }
                    Group tagGroup = match.Groups["tag"];
                    if (tagGroup != null) {
                        string macroValue;
                        if (_macros.TryGetValue(tagGroup.Value, out macroValue)) {
                            rv.Append(macroValue);
                        }
                    }
                    index = match.Index + match.Length;
                }
            }
            if (index < s.Length) {
                rv.Append(s.Substring(index));
            }
            return rv.ToString();
        }
    }
}
