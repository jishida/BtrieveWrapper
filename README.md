# BtrieveWrapper

## Summary

BtrieveWrapper is a wrapper library of Btrieve API, which operates MicroKarnel
Database Engine (MKDE) of Actian PSQL. It uses Actian PSQL and works on Actian
PSQL Client and Microsoft .NET Framework 2.0 or higher.

This library provides classes to operate MKDE without SQL.
Generally, In operating RDBMS with SQL, the parsing process can be overhead and
cause a bottleneck.
Btrieve API provides low-level functions which operate database, so using it is
one of effective choices.
However, source codes written considering performance with Btrieve API tend to
be obfuscated and spoil maintainability and versatility.
BtrieveWrapper.Orm wraps generic functions  Btrieve API contains and
provides high usability.

## Demo

This demo uses sample database which contains PSQL product. Before you try this,
make sure that demodata database is installed and backup it for safety.
BtrieveWrapper.Demo is executable project of this demo.

Execute these commands, if PSQL server and demodata database are installed in
local machine.
```
BtrieveWrapper.Orm.Models.Generator.exe --mode=0 --input=btrv://127.0.0.1/Demodata --output=Demodata.xml
BtrieveWrapper.Orm.Models.Generator.exe --mode=2 --input=Demodata.xml
```

Create a new Console Application project.
Refer BtrieveWrapper.dll and BtrieveWrapper.Orm.dll from it and add generated
files to it.
Then, edit main function like this.
```csharp
using System;
using System.Collections.Generic;
using BtrieveWrapper.Orm;
using BtrieveWrapper.Orm.Models.CustomModels;

namespace BtrieveWrapper.Demo
{
    class Program
    {
        static void Main(string[] args) {
            DemodataDbClient client = new DemodataDbClient();

            Console.WriteLine("[Read people whose last initial is 'D']");
            using (RecordManager<Person, PersonKeyCollection> people = client.Person()) {
                IEnumerable<Person> query = people.Query(p =>
                    p.Last_Name.GreaterThanOrEqual("D") &&
                    p.Last_Name.LessThan("E"));
                foreach (Person person in query) {
                    Console.WriteLine("Name: {0} {1}",
                        person.First_Name,
                        person.Last_Name);
                }
            }

            Console.WriteLine();

            Console.WriteLine("[Person CRUD]");
            using (RecordManager<Person, PersonKeyCollection> people = client.Person()) {
                using (Transaction transaction = client.BeginTransaction()) {
                    Console.Write("Create person: ");
                    Person person = new Person();
                    person.ID = 0;
                    person.First_Name = "Ieyasu";
                    person.Last_Name = "Tokugawa";
                    people.Add(person);
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.GetAndManage(p => p.ID == 0);
                    Console.WriteLine("Read person: " +
                        (person == null ? "not found" : person.First_Name + " " + person.Last_Name));

                    Console.Write("Update person: ");
                    person.First_Name = "Iemitsu";
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.GetAndManage(p => p.ID == 0);
                    Console.WriteLine("Read person: " +
                        (person == null ? "not found" : person.First_Name + " " + person.Last_Name));

                    Console.Write("Delete person: ");
                    people.Remove(person);
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.Get(p => p.ID == 0);
                    Console.WriteLine("Read person: " +
                        (person == null ? "not found" : person.First_Name + " " + person.Last_Name));

                    transaction.Commit();
                }
            }
        }
    }
}
```

## Trademarks

Btrieve is a registered trademark of Actian Corporation.
Actian PSQL is a trademark of Actian Corporation.
All other trademarks, trade names, service marks, and logos referenced herein
belong to their respective companies.

## Copyright

Copyright © 2014 Junki Ishida

## License

BtrieveWrapper is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to license.txt for more information.