namespace LexiFlow.API.Extensions
{
    /// <summary>
    /// Extension methods for DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get the start of week for a given date
        /// </summary>
        /// <param name="dt">The date</param>
        /// <param name="startOfWeek">First day of week (default: Monday)</param>
        /// <returns>Date representing start of week</returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Get the end of week for a given date
        /// </summary>
        /// <param name="dt">The date</param>
        /// <param name="startOfWeek">First day of week (default: Monday)</param>
        /// <returns>Date representing end of week</returns>
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return dt.StartOfWeek(startOfWeek).AddDays(6);
        }

        /// <summary>
        /// Get the start of month for a given date
        /// </summary>
        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// Get the end of month for a given date
        /// </summary>
        public static DateTime EndOfMonth(this DateTime dt)
        {
            return dt.StartOfMonth().AddMonths(1).AddDays(-1);
        }
    }
}