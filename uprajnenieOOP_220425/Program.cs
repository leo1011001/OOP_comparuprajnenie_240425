using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace uprajnenieOOP_220425
{
    /// Represents a person with name and age properties
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return $"{Name} ({Age} years)";
        }
    }

    /// Compares Person objects by Age in ascending order
    public class AgeComparer : IComparer<Person>
    {
        public int Compare(Person x, Person y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return x.Age.CompareTo(y.Age);
        }
    }

    /// Compares Person objects by Name in alphabetical order
    public class NameComparer : IComparer<Person>
    {
        public int Compare(Person x, Person y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SortTimerWithReflection Application");
            Console.WriteLine("===================================");

            // Generate a list of 1000 random Person objects
            List<Person> people = GenerateRandomPeople(1000);
            Console.WriteLine($"Generated {people.Count} random people.");

            // Create instances of our comparers
            var ageComparer = new AgeComparer();
            var nameComparer = new NameComparer();

            // Sort by Age and measure time
            MeasureSortPerformance(people, ageComparer, "Age");

            // Sort by Name and measure time
            MeasureSortPerformance(people, nameComparer, "Name");

            // Use Reflection to inspect the comparer types
            InspectComparersWithReflection();

            Console.ReadKey();
        }

        /// Generates a list of random Person objects
        static List<Person> GenerateRandomPeople(int count)
        {
            var random = new Random();
            var firstNames = new[] { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Jennifer", "William", "Lisa" };
            var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson" };

            var people = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                string name = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
                int age = random.Next(18, 80);
                people.Add(new Person(name, age));
            }

            return people;
        }

        /// Measures sorting performance using a specific comparer
        static void MeasureSortPerformance(List<Person> people, IComparer<Person> comparer, string sortType)
        {
            // Create a copy of the original list to sort
            var peopleToSort = new List<Person>(people);

            Console.WriteLine($"\nSorting by {sortType}...");

            // Start the stopwatch
            var stopwatch = Stopwatch.StartNew();

            // Perform the sort
            peopleToSort.Sort(comparer);

            // Stop the stopwatch
            stopwatch.Stop();

            Console.WriteLine($"Sorted {peopleToSort.Count} people by {sortType} in {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"First 5 items after sorting by {sortType}:");
            for (int i = 0; i < 5 && i < peopleToSort.Count; i++)
            {
                Console.WriteLine($"  {peopleToSort[i]}");
            }
        }
        /// Uses Reflection to inspect the comparer types
        static void InspectComparersWithReflection()
        {
            Console.WriteLine("\nReflection Analysis of Comparers:");
            Console.WriteLine("--------------------------------");

            Type[] comparerTypes = { typeof(AgeComparer), typeof(NameComparer) };

            foreach (var type in comparerTypes)
            {
                Console.WriteLine($"\nInspecting {type.Name}:");

                // Get type information
                Console.WriteLine($"  Full Name: {type.FullName}");
                Console.WriteLine($"  Implements IComparer<Person>: {typeof(IComparer<Person>).IsAssignableFrom(type)}");

                // Get method information
                var compareMethod = type.GetMethod("Compare");
                Console.WriteLine($"  Compare method parameters:");
                foreach (var param in compareMethod.GetParameters())
                {
                    Console.WriteLine($"    {param.ParameterType.Name} {param.Name}");
                }
            }
        }
    }
}
