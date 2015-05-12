using System.Text.RegularExpressions;
using Markout.Input.Interfaces;

namespace Markout.Input.Tags.TagFactories {

    public abstract class BaseTagFactory : ITagFactory {

        public string TagRecognizer { get; set; }

        public abstract Tag CreateTagFromMatch(Match match);
    }
}