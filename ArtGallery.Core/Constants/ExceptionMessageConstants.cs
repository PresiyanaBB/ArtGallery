using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Constants
{
    public static class ExceptionMessageConstants
    {
        public const string GuardNullException = "{0} is required.";
        public const string GuardStringException = "{0} is empty or missing.";
        public const string GuardOutOfRangeException = "{0} is out of range.";
        public const string AppointmentInvalidDateException = "Appointment date cannot be in the past.";
        public const string UnexpectedErrorMessage = "An unexpected error has occured.";
    }
}
