using System;
using System.Linq;
using Xunit;

using BtrieveWrapper.Orm.Models.CustomModels;

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
            var path = _temp.GetPath("001_person.mkd");

            var sut = new DemodataDbClient(Settings.DllPath);

            using (var personManager = sut.Person(Path.Absolute(path))) {
                personManager.Operator.Create(overwrite: true);


            }

        }


    }
}
