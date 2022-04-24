using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Fluxor.Forms.Utilities
{
    internal static class ObjectExtensions
    {
#if NET5_0_OR_GREATER
        [return: NotNullIfNotNull("source")]
#endif
        public static T? DeepCopy<T>(this T? source)
            => source is null ?
                default :
                JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source));

    }
}
