using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Orm.Tests.Models;

namespace BtrieveWrapper.Orm.Tests
{
    class EntityFactory
    {
        public static IEnumerable<Company> EnumerateCanpanies() {
            yield return new Company() {
                Id = 1,
                Name = "Capmco",
                Comment = null
            };
            yield return new Company() {
                Id = 2,
                Name = "Sqeni",
                Comment = null
            };
            yield return new Company() {
                Id = 3,
                Name = "Komina",
                Comment = null
            };
        }

        public static IEnumerable<Employee> EnumerateEmployees(){
            yield return new Employee() {
                Id = 3,
                FirstName = "Naoki",
                LastName = "Yamada",
                Salary = 3000,
                CompanyId = 1,
                Comment = null
            };
            yield return new Employee() {
                Id = 1,
                FirstName = "Yasuo",
                LastName = "Sakata",
                Salary = 6000,
                CompanyId = 2,
                Comment = null
            };
            yield return new Employee() {
                Id = 8,
                FirstName = "Kazuto",
                LastName = "Tanaka",
                Salary = 4000,
                CompanyId = 2,
                Comment = null
            };
            yield return new Employee() {
                Id = 0,
                FirstName = "Toshiya",
                LastName = "Inoue",
                Salary = 2000,
                CompanyId = 3,
                Comment = null
            };
            yield return new Employee() {
                Id = 9,
                FirstName = "Tomomi",
                LastName = "Aoki",
                Salary = 2500,
                CompanyId = 1,
                Comment = null
            };
            yield return new Employee() {
                Id = 7,
                FirstName = "Kousuke",
                LastName = "Shigeta",
                Salary = 5000,
                CompanyId = 3,
                Comment = null
            };
            yield return new Employee() {
                Id = 6,
                FirstName = "Yukari",
                LastName = "Morita",
                Salary = 3000,
                CompanyId = 2,
                Comment = null
            };
            yield return new Employee() {
                Id = 5,
                FirstName = "Kenta",
                LastName = "Saitou",
                Salary = 3300,
                CompanyId = 3,
                Comment = null
            };
            yield return new Employee() {
                Id = 2,
                FirstName = "Mitsuo",
                LastName = "Sengoku",
                Salary = 4700,
                CompanyId = 2,
                Comment = null
            };
            yield return new Employee() {
                Id = 4,
                FirstName = "Tomohiro",
                LastName = "Nakamura",
                Salary = 1900,
                CompanyId = 1,
                Comment = null
            };
        }
    }
}
