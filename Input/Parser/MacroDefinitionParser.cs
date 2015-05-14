using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Markout.Input.Parser {
    
    public class MacroDefinitionParser {

        public Dictionary<string, string> ParseMacroDefinitionsFromFile(string path) {
            return ParseMacroDefinitionsFromText(File.ReadAllText(path));
        }

        public Dictionary<string, string> ParseMacroDefinitionsFromText(string text) {
            Dictionary<string, string> macros = new Dictionary<string, string>();
            string[] lines = text.Split('\n');
            lines.Select(l => l.Trim())
                .Where(l => l.Length > 0 && l[0] != ';') // No empty lines and no ;-delimited comments
                .ToList()
                .ForEach(l => {
                    string[] parts = l.Split(new char[] {' ', '\t'}, 2);
                    parts[1] = parts[1].Trim();
                    if (parts[0].Length > 0 && parts[1].Length > 0) {
                        macros.Add(parts[0], parts[1]);
                    }
                });
            return macros;
        }
    }
}
