using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AulaLinQ
{
    public class Adm
    {

        public static List<Person> LoadData()
        {
            var people = new List<Person>();
            people.Add(new Person() { Id = 1, Name = "Alice A", Age = 30, Telephone = "11 912345678" });
            people.Add(new Person() { Id = 2, Name = "Bob B", Age = 25, Telephone = "11 923456789" });
            people.Add(new Person() { Id = 3, Name = "Charlie C", Age = 28, Telephone = "11 934567890" });
            people.Add(new Person() { Id = 4, Name = "David D", Age = 35, Telephone = "11 945678901" });
            people.Add(new Person() { Id = 5, Name = "Eve E", Age = 22, Telephone = "11 956789012" });
            people.Add(new Person() { Id = 6, Name = "Frank F", Age = 27, Telephone = "11 967890123" });
            people.Add(new Person() { Id = 7, Name = "Grace G", Age = 32, Telephone = "11 978901234" });
            people.Add(new Person() { Id = 8, Name = "Hank H", Age = 29, Telephone = "11 989012345" });

            return people;
        }

        public static void PrintData(List<Person> person)
        {
            foreach (Person p in person)
            {
                Console.WriteLine(p);
            }

        }

        public static List<Person> FilterByAgeLinq(List<Person> person) => person.Where(p => p.Age >= 18).ToList();

        public static List<Person> FilterByLetterLinq(List<Person> person) => person.Where(p => p.Name.StartsWith("A", StringComparison.OrdinalIgnoreCase)).ToList();

        public static List<Person> OrderByName(List<Person> person) => (person.OrderBy(p => p.Name)).ToList();

        public static List<Person> OrderByNameInv(List<Person> person) => (person.OrderByDescending(p => p.Name)).ToList();

        public static List<Person> OrderByLetterAAndSize(List<Person> person) => (person.Where(p => p.Name.Contains('A', StringComparison.OrdinalIgnoreCase) && p.Name.Length > 3 )).ToList();

    }
}
