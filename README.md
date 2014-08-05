# BtrieveWrapper

## Summary

BtrieveWrapper is a wrapper library of Btrieve API, which operates MicroKarnel
Database Engine (MKDE) of Actian PSQL. It uses Actian PSQL and works on .NET
Framework 4.0.

This library provides classes to operate MKDE without SQL.
Generally, In operating RDBMS with SQL, the parsing process can be overhead and
cause a bottleneck.
Btrieve API provides low-level functions which operate database, so using it is
one of effective choices.
However, source codes written considering performance with Btrieve API tend to
be obfuscated and spoil maintainability and versatility.
BtrieveWrapper.Orm wraps generic functions  Btrieve API contains and
provides high usability.

BtrieveWrapper は Actian PSQL のデータベース操作システムである MicroKarnel
Database Engine (MKDE) を Btrieve API を用いて操作するラッパーライブラリです。
このライブラリは Actian PSQL を使用しており、動作には.NET Framework 4.0 が必要
です。

このライブラリは、 SQL を使用せずに、 MKDE を操作するクラスを提供します。
一般に、 SQL を用いた RDBMS の操作では SQL のパース処理がオーバーヘッドとなり、
クエリ処理能力のボトルネックになることがあります。
Btrieve API は ISAM ベースの低レベルなデータベース操作を提供しており、クエリ処
理のパフォーマンスを上げる選択肢の一つとして有効です。
しかし、高パフォーマンスを意識して Btrieve API を利用したコードは、複雑になりや
すく、コードの保守性や汎用性を損ないがちです。
BtrieveWrapper.Orm は Btrieve API の汎用的な操作をラッピングし、高いユー
ザビリティを提供します。


## Demo

This demo uses sample database which contains PSQL product. Before you try this,
make sure that demodata database is installed and backup it for safety.
BtrieveWrapper.Demo is executable project of this demo.

このデモは PSQL 製品に含まれるサンプルデータベースを使用します。このデモを実行
する前に、 PSQL に Demodata データベースがインストールされていることを確認し、
安全のためにバックアップを行って下さい。また、 BtrieveWrapper.Demo はこのデモの
実行可能なプロジェクトになっています。

Execute these commands, if PSQL server and demodata database are installed in
local machine.

ローカルマシンに PSQL サーバーと Demodata データベースがインストールされている
場合以下のコマンドを実行して下さい。
```
BtrieveWrapper.Orm.Models.Generator.exe --mode=0 --input=btrv://127.0.0.1/Demodata --output=Demodata.xml
BtrieveWrapper.Orm.Models.Generator.exe --mode=2 --input=Demodata.xml
```

Create a new Console Application project.
Refer BtrieveWrapper.dll and BtrieveWrapper.Orm.dll from it and add generated
files to it.
Then, edit main function like this.

新たに、コンソールアプリケーションプロジェクトを作成し、BtrieveWrapper.dll と
BtrieveWrapper.Orm.dll を参照したのち、 BtrieveWrapper.Orm.Models.Generator
により生成されたファイルをプロジェクトに追加して下さい。
あとは以下のようにコードを書けば実行できるはずです。
```csharp
using System;
using BtrieveWrapper.Orm;
using BtrieveWrapper.Orm.Models.CustomModels;

namespace BtrieveWrapper.Demo
{
    class Program
    {
        static void Main(string[] args) {
            DemodataDbClient client = new DemodataDbClient();

            Console.WriteLine("[Read people whose last initial is 'D']");
            using (PersonManager people = client.Person()) {
                var query = people.Query(p =>
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
            using (PersonManager people = client.Person()) {
                Person person;
                using (Transaction transaction = client.BeginTransaction()) {
                    Console.Write("Create person: ");
                    person = new Person();
                    person.ID = 0;
                    person.First_Name = "Ieyasu";
                    person.Last_Name = "Tokugawa";
                    people.Add(person);
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.GetAndManage(p => p.ID == 0);
                    Console.WriteLine("Read person: {0} {1}", person.First_Name, person.Last_Name);

                    Console.Write("Update person: ");
                    person.First_Name = "Iemitsu";
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.GetAndManage(p => p.ID == 0);
                    Console.WriteLine("Read person: {0} {1}", person.First_Name, person.Last_Name);

                    Console.Write("Delete person: ");
                    people.Remove(person);
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.Get(p => p.ID == 0);
                    Console.WriteLine(person == null ? "Person is not found" : "Person is found");

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

Btrieve は Actian Corporation の登録商標です。
Actian PSQL は Actian Corporation の商標です。
その他、会社名、製品名などは一般に各メーカーの登録商標または商標です。

## Copyright

Copyright © 2014 Junki Ishida

## License

BtrieveWrapper is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to license.txt for more information.