using Markout.Common.DataModel.Attribute;

namespace Markout.Input.Interfaces {
    public interface IExternalTagResolver {

        /// <summary>
        /// Resolves the specified text attribute, or returns null if that is not possible.
        /// </summary>
        /// <param name="textAttributeExternal"></param>
        /// <returns></returns>
        string Resolve(TextAttributeExternal textAttributeExternal);
    }
}
