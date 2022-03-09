
using System.ComponentModel;

namespace KioskStream.Core.Enums
{
    public enum EEmployeeAbsenceType
    {
        [Description("Vacation")]
        Vacation,
        [Description("Day Off")]
        DayOff,
        [Description("Emergency")]
        Emergency,
        [Description("Sick Leave")]
        SickLeave,
        [Description("Maternity Leave")]
        MaternityLeave,
        [Description("Paternity Leave")]
        PaternityLeave,
        [Description("Other")]
        Other,
    }
}
