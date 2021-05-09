using System.Diagnostics.CodeAnalysis;

namespace Entities.Enums
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ComponentCategory
    {
        CPU,
        RAM,
        SSD,
        HDD,
        GPU,
    }
}