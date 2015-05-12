using System.Text.RegularExpressions;
using Markout.Input.Tags;

namespace Markout.Input.Interfaces {

    public interface ITagFactory {

        string TagRecognizer { get; set; }
        Tag CreateTagFromMatch(Match match);
    }
}