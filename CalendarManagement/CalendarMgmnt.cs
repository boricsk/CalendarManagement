namespace CalendarManagement
{
    public class CalendarMgmnt
    {
        private List<FixHoliday>? _fixHolidays = new();
        private List<MovedWorkDays>? _movedWorkDays = new();
        private List<DateOnly>? _additionalWorkdays = new();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMgmnt"/> class with optional holiday and workday
        /// configurations.
        /// </summary>
        /// <remarks>If <paramref name="fixHolidays"/> is <see langword="null"/>, the constructor
        /// initializes a default set of fixed holidays based on Hungarian national holidays. This includes New Year's
        /// Day, Revolution Day, Labor Day, State Founding Day, Republic Day, All Saints' Day, Christmas (two days), and
        /// New Year's Eve.</remarks>
        /// <param name="fixHolidays">A list of fixed holidays to be used by the calendar. If <see langword="null"/>, a default set of Hungarian
        /// holidays is initialized.</param>
        /// <param name="movedWorkDays">A list of workdays that have been moved from their regular schedule, typically due to holiday adjustments.</param>
        /// <param name="additionalWorkdays">A list of additional workdays to be included in the calendar.</param>
        public CalendarMgmnt(List<FixHoliday>? fixHolidays = null, List<MovedWorkDays>? movedWorkDays = null, List<DateOnly>? additionalWorkdays = null)
        {
            _movedWorkDays = movedWorkDays;
            _additionalWorkdays = additionalWorkdays;

            if (fixHolidays is null)
            {
                //Fix holidays in Hungarian
                FixHoliday newYear = new FixHoliday { Month = 1, Day = 1 };
                _fixHolidays.Add(newYear);
                FixHoliday revolution = new FixHoliday { Month = 3, Day = 15 };
                _fixHolidays.Add(revolution);
                FixHoliday labourDay = new FixHoliday { Month = 5, Day = 1 };
                _fixHolidays.Add(labourDay);
                FixHoliday stateFoundingDay = new FixHoliday { Month = 8, Day = 20 };
                _fixHolidays.Add(stateFoundingDay);
                FixHoliday republicDay = new FixHoliday { Month = 10, Day = 23 };
                _fixHolidays.Add(republicDay);
                FixHoliday helloween = new FixHoliday { Month = 11, Day = 1 };
                _fixHolidays.Add(helloween);
                FixHoliday xmas1 = new FixHoliday { Month = 12, Day = 25 };
                _fixHolidays.Add(xmas1);
                FixHoliday xmas2 = new FixHoliday { Month = 12, Day = 26 };
                _fixHolidays.Add(xmas2);
                FixHoliday silvester = new FixHoliday { Month = 12, Day = 31 };
                _fixHolidays.Add(silvester);
            }
            else
            {
                _fixHolidays = fixHolidays;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Calculates the date of Easter Monday for a given year.
        /// </summary>
        /// <remarks>Easter Monday is the day following Easter Sunday, which is calculated based on the
        /// Gregorian calendar. This method uses an algorithm specific to the range of years 1901 to 2099.</remarks>
        /// <param name="year">The year for which to calculate Easter Monday. Must be between 1901 and 2099, inclusive.</param>
        /// <returns>The date of Easter Monday as a <see cref="DateOnly"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="year"/> is outside the range of 1901 to 2099.</exception>
        public DateOnly GetEasterMonday(int year)
        {
            if (year >= 1901 && year <= 2099)
            {
                int a = year % 19;
                int b = year % 4;
                int c = year % 7;
                int d = ((19 * a) + 24) % 30;
                int e = (2 * b) + (4 * c) + (6 * d) + 5;
                int h = 0;

                if (e % 7 == 6 && d == 29)
                {
                    h = 50;
                }
                else
                {
                    if (e % 7 == 6 && d == 29 && year % 19 > 10)
                    {
                        h = 49;
                    }
                    else
                    {
                        h = (e % 7) + 22 + d;
                    }
                }
                return new DateOnly(year, 3, 1).AddDays(h);
            }
            else { throw new ArgumentOutOfRangeException(nameof(year)); }
        }
        /// <summary>
        /// Calculates the date of Pentecost (Whitsunday) for the specified year.
        /// </summary>
        /// <remarks>Pentecost is calculated as 49 days after Easter Monday. This method relies on the
        /// calculation of Easter Monday for the given year.</remarks>
        /// <param name="year">The year for which to calculate Pentecost. Must be between 1901 and 2099, inclusive.</param>
        /// <returns>The date of Pentecost as a <see cref="DateOnly"/> value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="year"/> is outside the range of 1901 to 2099.</exception>
        public DateOnly GetPentecostDay(int year)
        {
            if (year >= 1901 && year <= 2099)
            {
                return GetEasterMonday(year).AddDays(49);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }
        }
        /// <summary>
        /// Calculates the date of Pentecost based on the provided Easter Monday date.
        /// </summary>
        /// <param name="easterMonday">The date of Easter Monday. The year must be between 1901 and 2099, inclusive.</param>
        /// <returns>The date of Pentecost, which is 49 days after Easter Monday.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the year of <paramref name="easterMonday"/> is outside the range of 1901 to 2099.</exception>
        public DateOnly GetPentecostDay(DateOnly easterMonday)
        {
            if (easterMonday.Year >= 1901 && easterMonday.Year <= 2099)
            {
                return easterMonday.AddDays(49);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(easterMonday.Year));
            }
        }
        /// <summary>
        /// Determines whether the specified date is a holiday.
        /// </summary>
        /// <remarks>A date is considered a holiday if it meets any of the following conditions: <list
        /// type="bullet"> <item><description>It falls on a weekend (Saturday or Sunday).</description></item>
        /// <item><description>It matches a fixed holiday defined in the system.</description></item>
        /// <item><description>It corresponds to Easter Monday, Good Friday (three days before Easter Monday), or
        /// Pentecost Day.</description></item> <item><description>It is marked as a moved holiday in the
        /// system.</description></item> </list> Conversely, a date is considered a workday if it has been explicitly
        /// moved from a holiday to a workday.</remarks>
        /// <param name="checkDate">The date to check for holiday status.</param>
        /// <returns><see langword="true"/> if the specified date is a holiday; otherwise, <see langword="false"/>.</returns>
        public bool IsHoliday(DateOnly checkDate)
        {
            int year = checkDate.Year;
            int month = checkDate.Month;
            int day = checkDate.Day;

            if (_movedWorkDays != null)
            {
                if (_movedWorkDays.Where(s => s.MovedTo == checkDate).Any()) { return false; }
                if (_movedWorkDays.Where(s => s.MovedWorkday == checkDate).Any()) { return true; }
            }

            if (_additionalWorkdays != null) { if (_additionalWorkdays.Contains(checkDate)) { return false; } }


            if (checkDate.DayOfWeek == DayOfWeek.Sunday || checkDate.DayOfWeek == DayOfWeek.Saturday) { return true; }


            if (_fixHolidays.Where(d => d.Month == month && d.Day == day).Any()) { return true; }

            if (GetEasterMonday(year) == checkDate || GetEasterMonday(year).AddDays(-3) == checkDate) { return true; }

            if (GetPentecostDay(year) == checkDate) { return true; }

            return false;
        }
        /// <summary>
        /// Retrieves a list of workdays within the specified date range.
        /// </summary>
        /// <remarks>A workday is defined as any date within the range that is not considered a holiday. 
        /// The method iterates through each date in the range, checking whether it is a holiday using the
        /// <c>IsHoliday</c> method.</remarks>
        /// <param name="startDate">The start date of the range. Must be less than or equal to <paramref name="endDate"/>.</param>
        /// <param name="endDate">The end date of the range. Must be greater than or equal to <paramref name="startDate"/>.</param>
        /// <returns>A list of <see cref="DateOnly"/> objects representing workdays within the specified range.  Workdays are
        /// dates that are not holidays.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startDate"/> is greater than <paramref name="endDate"/>.</exception>
        public List<DateOnly> GetWorkdays(DateOnly startDate, DateOnly endDate)
        {
            List<DateOnly> ret = new List<DateOnly>();
            if (startDate < endDate)
            {
                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (!IsHoliday(date)) { ret.Add(date); }
                }
                return ret;
            }
            else { throw new ArgumentException("startDate must be <= endDate"); }

        }
        /// <summary>
        /// Retrieves a list of holidays within the specified date range.
        /// </summary>
        /// <remarks>This method iterates through each date in the specified range and checks whether it
        /// is a holiday. A date is considered a holiday if it satisfies the criteria defined by the <c>IsHoliday</c>
        /// method.</remarks>
        /// <param name="startDate">The start date of the range. Must be less than or equal to <paramref name="endDate"/>.</param>
        /// <param name="endDate">The end date of the range. Must be greater than or equal to <paramref name="startDate"/>.</param>
        /// <returns>A list of <see cref="DateOnly"/> objects representing the holidays within the specified range. The list will
        /// be empty if no holidays are found.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startDate"/> is greater than <paramref name="endDate"/>.</exception>
        public List<DateOnly> GetHolidays(DateOnly startDate, DateOnly endDate)
        {
            List<DateOnly> ret = new List<DateOnly>();
            if (startDate < endDate)
            {
                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (IsHoliday(date)) { ret.Add(date); }
                }
                return ret;
            }
            else { throw new ArgumentException("startDate must be <= endDate"); }

        }
        /// <summary>
        /// Calculates the number of workdays and holidays within a specified date range.
        /// </summary>
        /// <remarks>A workday is defined as any day that is not considered a holiday. The determination
        /// of whether a day is a holiday is based on the implementation of the <c>IsHoliday</c> method.</remarks>
        /// <param name="startDate">The start date of the range. Must be less than or equal to <paramref name="endDate"/>.</param>
        /// <param name="endDate">The end date of the range. Must be greater than or equal to <paramref name="startDate"/>.</param>
        /// <returns>A tuple containing two values: <list type="bullet"> <item><description><c>workdaysNumber</c>: The number of
        /// workdays in the specified date range.</description></item> <item><description><c>holidaysNumber</c>: The
        /// number of holidays in the specified date range.</description></item> </list></returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startDate"/> is greater than <paramref name="endDate"/>.</exception>
        public (int workdaysNumber, int holidaysNumber) CountDays(DateOnly startDate, DateOnly endDate)
        {
            (int workdaysNumber, int holidaysNumber) ret = (0, 0);
            if (startDate < endDate)
            {
                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (IsHoliday(date)) { ret.holidaysNumber++; } else { ret.workdaysNumber++; }
                }
                return ret;
            }
            else
            {
                throw new ArgumentException("startDate must be <= endDate");
            }
        }
        #endregion
    }
}
