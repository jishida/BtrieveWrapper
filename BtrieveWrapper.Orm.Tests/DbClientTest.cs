using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using BtrieveWrapper.Orm.Tests.Models;

namespace BtrieveWrapper.Orm.Tests
{
    public class DbClientTest : IDisposable
    {
        Temporary _temp;

        public DbClientTest() {
            _temp = new Temporary("DbClientTest");
        }

        public void Dispose() {
            _temp.Dispose();
        }

        [Fact]
        public void _001_basic_usage() {
            var path = _temp.GetPath("001.mkd");

            var sut = new DemoDbClient(Settings.DllPath);

            using (var employeeManager = sut.Employee(Path.Absolute(path))) {
                employeeManager.Operator.Create(overwrite: true);

                using (var transaction = sut.BeginTransaction()) {
                    foreach (var employee in EntityFactory.EnumerateEmployees()) {
                        employeeManager.Add(employee);
                    }
                    transaction.Commit();
                }

                var actual = employeeManager.Query().ToArray();
                var expected = EntityFactory.EnumerateEmployees().ToArray();

                Assert.Equal(EntityFactory.EnumerateEmployees(), employeeManager.Query());
                Assert.Equal(EntityFactory.EnumerateEmployees().Where(e => e.LastName.LessThan("B")), employeeManager.Query(null, e => e.LastName.LessThan("B")));
                Assert.Equal(
                    EntityFactory.EnumerateEmployees()
                        .OrderBy(e => e.Id),
                    employeeManager.Query(Employee.Keys[0]));
                Assert.Equal(
                    EntityFactory.EnumerateEmployees()
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName),
                    employeeManager.Query(Employee.Keys[1]));
                Assert.Equal(
                    EntityFactory.EnumerateEmployees()
                        .Single(e => e.Id == 5),
                    employeeManager.Get(e => e.Id == 5));
                Assert.Equal(
                    EntityFactory.EnumerateEmployees()
                        .Where(e => e.FirstName.LessThan("T"))
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName),
                    employeeManager.Query(e => e.FirstName.LessThan("T")));
            }

        }

    }
}
