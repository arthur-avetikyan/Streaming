
using System;
using KioskStream.Core.Enums;

namespace KioskStream.Core.Extensions
{
    public static class DomainStateExtensions
    {
        public static string GetDomainStateName(this int state)
        {
            return Enum.GetName(typeof(EDomainState), state);
        }

        public static string GetDomainStateName(this EDomainState state)
        {
            return Enum.GetName(typeof(EDomainState), state);
        }

        public static string GenerateDomainStateColor(this int stateId)
        {
            var state = (EDomainState) stateId;
            return state switch
            {
                EDomainState.Pending => "#ffef96",
                EDomainState.Processing => "#b7d7e8",
                EDomainState.Completed => "#b5e7a0",
                EDomainState.Approved => "#f9ccac",
                EDomainState.Reopened => "#92a8d1",
                EDomainState.Rejected => "#eea29a",
                _ => "#FFFFFF"
            };
        }

        public static string GenerateDomainStateIconName(this int stateId)
        {
            var state = (EDomainState)stateId;
            return state switch
            {
                EDomainState.Pending => "warning",
                EDomainState.Processing => "autorenew",
                EDomainState.Completed => "check_circle_outline",
                EDomainState.Approved => "check_box",
                EDomainState.Reopened => "settings_backup_restore",
                EDomainState.Rejected => "cancel",
                _ => "error"
            };
        }
    }
}
