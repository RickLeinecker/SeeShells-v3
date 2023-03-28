using System;

namespace SeeShellsV3.Data
{
    public class Timezone : ITimezone
    {
        public string Name { get; init; }

        public string Identifier { get; init; }

        public TimeZoneInfo Information { get; init; }

        public Timezone(string name, string identifier)
        {
            Name = name;
            Identifier = identifier;

            Information = TimeZoneInfo.FindSystemTimeZoneById(name == "Coordinated Universal Time" ? "UTC" : name);
        }

        public bool Identify(string input)
        {
            return input == Name || input == Identifier;
        }

        public bool Identify(TimeZoneInfo input)
        {
            return input.Equals(Information);
        }


    }
}