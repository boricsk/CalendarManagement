<p align="center">
  <img src=https://devnullsec.hu/logo_small.svg height="64" width="64">
</p>

## Calendar Management
### C# library for handling workdays and holidays


- Usage
> The package can be used to determine which days are holidays and which are workdays. It calculates moving holidays regularly if necessary. It can create a list of holidays and workdays in a given interval.

The package uses below 2 datatype:<br>
For the fix holiday:<br>
```csharp
    public class FixHoliday
    {
        public int Month {get; set;}
        public int Day {get; set;}
    }
```
It is represent the fix holidays. Before instantiation, you need to compile a list of fixed holidays and pass it to the constructor. See examples below.

And for the moved working days:<br>
```csharp
    public class MovedWorkDays
    {
        public DateOnly MovedWorkday { get; set; }
        public DateOnly MovedTo { get; set; }
    }
```
This list must also be constructed before instantiation if you want to use it. It is not mandatory to pass it to the constructor during instantiation.

- Example

1. You should create a list for the fix holidays

```csharp
private List<FixHoliday> _fixHolidays = new();

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
```

2. You should create a list for the moved workdays, if necessary.
```csharp
List<MovedWorkDays> movedWorkdays = new List<MovedWorkDays>();
            
MovedWorkDays mwd = new MovedWorkDays()
{ 
    MovedWorkday = new DateOnly(2025,5,2),
    MovedTo = new DateOnly(2025,5,17)
};
movedWorkdays.Add(mwd);
MovedWorkDays mwd2 = new MovedWorkDays()
{
    MovedWorkday = new DateOnly(2025, 10, 24),
    MovedTo = new DateOnly(2025, 10, 18)
};
movedWorkdays.Add(mwd2);

MovedWorkDays mwd3 = new MovedWorkDays()
{
    MovedWorkday = new DateOnly(2025, 12, 24),
    MovedTo = new DateOnly(2025, 12, 13)
};
movedWorkdays.Add(mwd3);

```

3. Create an instance
```csharp
CalendarMgmnt c = new CalendarMgmnt(_fixHolidays, movedWorkdays);

CalendarMgmnt hun = new CalendarMgmnt(movedWorkdays);
// For Hungarian users, in this case the constuctor build automatically the list of fix holidays.
// movedWorkdays is optional.
```

4. Methods
```csharp
CalendarMgmnt c = new CalendarMgmnt(_fixHolidays, movedWorkdays);
// movedWorkdays is optional.

CalendarMgmnt hun = new CalendarMgmnt(movedWorkdays);
// For Hungarian users, in this case the constuctor build automatically the list of fix holidays.
// movedWorkdays is optional.

// Date of Easter Monday
Console.WriteLine(c.GetEasterMonday(2024)); // -> 2024.04.01

DateOnly start = new DateOnly(2025, 3, 1);
DateOnly end = new DateOnly(2025, 3, 31);

var workdays = c.GetWorkdays(start, end); // List of workdays
var holidays = c.GetHolidays(start, end); // List of holidays

Console.WriteLine(c.CountDays(start, end)); // -> (21,10) Number of workdays and holidays

```





<p align="center">
  <img src=./out.jpg>
</p><br>
<p align="center">
  <a href="https://www.nuget.org/packages/MifareClassic/1.0.0" target="_blank" >
    <img src="./nuget_logo.png" alt="NuGet Page">
  </a>
</p>
