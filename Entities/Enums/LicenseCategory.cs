using System.Diagnostics.CodeAnalysis;

namespace Entities.Enums
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum LicenseCategory
    {
        Graphics_Software = 1,
        Office_Software = 2,
        IDE = 3,
        VPN = 4,
        Cloud_Software = 5
    }
}