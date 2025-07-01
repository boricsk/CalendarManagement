namespace CalendarManagement
{
    public class CalendarMgmnt
    {
        private List<FixHoliday> _fixHolidays = new();
        private List<MovedWorkDays>? _movedWorkDays = new();
        private bool _isCelebrateEaster;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMgmnt"/> class with a list of fixed holidays and an
        /// optional flag indicating whether Easter is celebrated.
        /// </summary>
        /// <param name="fixHolidays">A list of fixed holidays to be managed by the calendar. Cannot be null.</param>
        /// <param name="isCelebrateEaster">A value indicating whether Easter is celebrated.  <see langword="true"/> to include Easter in holiday
        /// calculations; otherwise, <see langword="false"/>. Defaults to <see langword="true"/>.</param>
        public CalendarMgmnt(List<FixHoliday> fixHolidays, List<MovedWorkDays>? movedWorkDays = null, bool isCelebrateEaster = true)
        {
            _fixHolidays = fixHolidays;
            _isCelebrateEaster = isCelebrateEaster;
            _movedWorkDays = movedWorkDays;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMgmnt"/> class, optionally including Easter as a
        /// holiday.
        /// </summary>
        /// <remarks>This constructor initializes a predefined set of fixed holidays based on Hungarian
        /// national holidays. The holidays include New Year's Day, Revolution Day, Labor Day, State Founding Day,
        /// Republic Day, Halloween,  Christmas (two days), and New Year's Eve.</remarks>
        /// <param name="isCelebrateEaster">A value indicating whether Easter should be included as a holiday.  <see langword="true"/> to include
        /// Easter; otherwise, <see langword="false"/>.</param>
        public CalendarMgmnt(List<MovedWorkDays>? movedWorkDays = null)
        {
            _isCelebrateEaster = true;
            _movedWorkDays = movedWorkDays;

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
        #endregion

        #region Public methods
        /// <summary>
        /// Calculates the date of Easter Monday for the specified year.
        /// </summary>
        /// <remarks>Easter Monday is the day following Easter Sunday, which is determined based on the
        /// ecclesiastical approximation of the March equinox. This method uses the Computus algorithm to calculate the
        /// date of Easter Sunday and then adds one day to determine Easter Monday.</remarks>
        /// <param name="year">The year for which to calculate Easter Monday. Must be a positive integer.</param>
        /// <returns>The date of Easter Monday as a <see cref="DateOnly"/> instance.</returns>
        public DateOnly GetEasterMonday(int year)
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

        /// <summary>
        /// Calculates the date of Pentecost for the specified year.
        /// </summary>
        /// <remarks>Pentecost is calculated as 49 days after Easter Monday. Ensure the <paramref
        /// name="year"/> parameter is valid and corresponds to a Gregorian calendar year.</remarks>
        /// <param name="year">The year for which to calculate Pentecost. Must be a positive integer.</param>
        /// <returns>The date of Pentecost as a <see cref="DateOnly"/> value.</returns>
        public DateOnly GetPentecostDay(int year) => GetEasterMonday(year).AddDays(49);

        /// <summary>
        /// Calculates the date of Pentecost based on the provided Easter Monday date.
        /// </summary>
        /// <remarks>Pentecost, also known as Whitsunday, occurs 49 days after Easter Monday. Ensure the
        /// <paramref name="easterMonday"/> parameter accurately represents Easter Monday to calculate the correct
        /// Pentecost date.</remarks>
        /// <param name="easterMonday">The date of Easter Monday. This must be a valid <see cref="DateOnly"/> instance representing Easter Monday.</param>
        /// <returns>A <see cref="DateOnly"/> instance representing the date of Pentecost, which is 49 days after Easter Monday.</returns>
        public DateOnly GetPentecostDay(DateOnly easterMonday) => easterMonday.AddDays(49);

        /// <summary>
        /// Determines whether the specified date is a holiday.
        /// </summary>
        /// <remarks>Fixed holidays are predefined dates that occur annually, while movable holidays are
        /// calculated based on the year. If the <c>_isCelebrateEaster</c> flag is enabled, Easter Monday and Pentecost
        /// Day are included in the holiday checks.</remarks>
        /// <param name="checkDate">The date to check for holiday status.</param>
        /// <returns><see langword="true"/> if the specified date is a holiday; otherwise, <see langword="false"/>. A date is
        /// considered a holiday if it falls on a weekend, matches a fixed holiday, or corresponds to a celebrated
        /// movable holiday such as Easter Monday or Pentecost Day. </returns>
        public bool isHoliday(DateOnly checkDate)
        {
            int year = checkDate.Year;
            int month = checkDate.Month;
            int day = checkDate.Day;

            if (_movedWorkDays != null)
            {
                if (_movedWorkDays.Where(S => S.MovedTo == checkDate).Any()) { return false; }
                if (_movedWorkDays.Where(s => s.MovedWorkday == checkDate).Any()) { return true; }
            }
            if (checkDate.DayOfWeek == DayOfWeek.Sunday || checkDate.DayOfWeek == DayOfWeek.Saturday) { return true; }
            if (_fixHolidays.Where(d => d.Month == month && d.Day == day).Any()) { return true; }
            if (_isCelebrateEaster)
            {
                if (GetEasterMonday(year) == checkDate || GetEasterMonday(year).AddDays(-3) == checkDate) { return true; }
                if (GetPentecostDay(year) == checkDate) { return true; }
            }
            return false;
        }
        /// <summary>
        /// Retrieves a list of workdays within the specified date range.
        /// </summary>
        /// <remarks>A workday is defined as any day within the specified range that is not considered a
        /// holiday. The method assumes that the <c>isHoliday</c> function determines whether a given date is a
        /// holiday.</remarks>
        /// <param name="startDate">The start date of the range. Must be less than or equal to <paramref name="endDate"/>.</param>
        /// <param name="endDate">The end date of the range. Must be greater than or equal to <paramref name="startDate"/>.</param>
        /// <returns>A list of <see cref="DateOnly"/> objects representing the workdays within the specified range.  The list
        /// will exclude holidays and be empty if no workdays are found.</returns>
        public List<DateOnly> GetWorkdays(DateOnly startDate, DateOnly endDate)
        {
            List<DateOnly> ret = new List<DateOnly>();
            if (startDate < endDate)
            {
                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (!isHoliday(date)) { ret.Add(date); }
                }
            }
            return ret;
        }
        /// <summary>
        /// Retrieves a list of holidays within the specified date range.
        /// </summary>
        /// <remarks>This method iterates through each date in the specified range and determines whether
        /// it is a holiday. The determination of holidays is based on the implementation of the <c>isHoliday</c>
        /// method.</remarks>
        /// <param name="startDate">The start date of the range. Must be earlier than <paramref name="endDate"/>.</param>
        /// <param name="endDate">The end date of the range. Must be later than <paramref name="startDate"/>.</param>
        /// <returns>A list of <see cref="DateOnly"/> objects representing the holidays within the specified range.  Returns an
        /// empty list if no holidays are found or if <paramref name="startDate"/> is not earlier than <paramref
        /// name="endDate"/>.</returns>
        public List<DateOnly> GetHolidays(DateOnly startDate, DateOnly endDate)
        {
            List<DateOnly> ret = new List<DateOnly>();
            if (startDate < endDate)
            {
                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (isHoliday(date)) { ret.Add(date); }
                }
            }
            return ret;
        }
        /// <summary>
        /// Calculates the number of workdays and holidays within a specified date range.
        /// </summary>
        /// <remarks>A day is considered a holiday if it satisfies the conditions defined by the
        /// <c>isHoliday</c> method. Otherwise, it is considered a workday.</remarks>
        /// <param name="startDate">The start date of the range. Must be earlier than <paramref name="endDate"/>.</param>
        /// <param name="endDate">The end date of the range. Must be later than <paramref name="startDate"/>.</param>
        /// <returns>A tuple containing two values: <list type="bullet"> <item><description><c>workdaysNumber</c>: The total
        /// number of workdays in the specified date range.</description></item>
        /// <item><description><c>holidaysNumber</c>: The total number of holidays in the specified date
        /// range.</description></item> </list> If <paramref name="startDate"/> is not earlier than <paramref
        /// name="endDate"/>, both values in the tuple will be <c>0</c>.</returns>
        public (int workdaysNumber, int holidaysNumber) CountDays(DateOnly startDate, DateOnly endDate)
        {
            (int workdaysNumber, int holidaysNumber) ret = (0, 0);
            if (startDate < endDate)
            {
                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (isHoliday(date)) { ret.holidaysNumber++; } else { ret.workdaysNumber++; }
                }
            }
            return ret;
        }
        #endregion
    }
}
