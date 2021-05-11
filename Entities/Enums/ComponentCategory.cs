using System.Diagnostics.CodeAnalysis;

namespace Entities.Enums
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ComponentCategory
    {
        CPU = 1,
        RAM = 2,
        SSD = 3,
        HDD = 4,
        GPU = 5,
    }
}