using System;
using System.Linq.Expressions;

using BtrieveWrapper.Orm.Tests.Models;
using Xunit;

namespace BtrieveWrapper.Orm.Tests
{
    public class QueryParameterTest
    {
        [Fact]
        public void _001_Case_of_PrimaryKeyQuery() {
            var sut = new QueryParameter<Employee>(e => e.Id == 10);
            Assert.Equal(0, sut.Key.KeyNumber);
            Assert.Equal(1, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            var filer = sut.ApiFilter[0][0];
            Assert.Equal("Id", filer.Field.Name);
            Assert.Equal(10, filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            sut = new QueryParameter<Employee>(e => e.Id != 3);
            Assert.Equal(0, sut.Key.KeyNumber);
            Assert.Equal(1, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            filer = sut.ApiFilter[0][0];
            Assert.Equal("Id", filer.Field.Name);
            Assert.Equal(3, filer.Value);
            Assert.Equal(FilterType.NotEqual, filer.Type);

            sut = new QueryParameter<Employee>(e => e.Id >= 4 && e.Id < 8);
            Assert.Equal(0, sut.Key.KeyNumber);
            Assert.Equal(2, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            filer = sut.ApiFilter[0][0];
            Assert.Equal("Id", filer.Field.Name);
            Assert.Equal(4, filer.Value);
            Assert.Equal(FilterType.GreaterThanOrEqual, filer.Type);

            filer = sut.ApiFilter[1][0];
            Assert.Equal("Id", filer.Field.Name);
            Assert.Equal(8, filer.Value);
            Assert.Equal(FilterType.LessThan, filer.Type);
        }

        [Fact]
        public void _002_Case_of_SecondaryKeyQuery() {
            var sut = new QueryParameter<Employee>(e => e.FirstName == "Ryoma");
            Assert.Equal(1, sut.Key.KeyNumber);
            Assert.Equal(1, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            var filer = sut.ApiFilter[0][0];
            Assert.Equal("FirstName", filer.Field.Name);
            Assert.Equal("Ryoma", filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            sut = new QueryParameter<Employee>(e => e.LastName != "Sakamoto");
            Assert.Equal(1, sut.Key.KeyNumber);
            Assert.Equal(1, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            filer = sut.ApiFilter[0][0];
            Assert.Equal("LastName", filer.Field.Name);
            Assert.Equal("Sakamoto", filer.Value);
            Assert.Equal(FilterType.NotEqual, filer.Type);

            sut = new QueryParameter<Employee>(e => e.LastName.GreaterThanOrEqual("S") && e.LastName.LessThan("T"));
            Assert.Equal(1, sut.Key.KeyNumber);
            Assert.Equal(2, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            filer = sut.ApiFilter[0][0];
            Assert.Equal("LastName", filer.Field.Name);
            Assert.Equal("S", filer.Value);
            Assert.Equal(FilterType.GreaterThanOrEqual, filer.Type);

            filer = sut.ApiFilter[1][0];
            Assert.Equal("LastName", filer.Field.Name);
            Assert.Equal("T", filer.Value);
            Assert.Equal(FilterType.LessThan, filer.Type);

            sut = new QueryParameter<Employee>(e => e.FirstName == e.LastName);
            Assert.Equal(1, sut.Key.KeyNumber);
            Assert.Equal(1, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            filer = sut.ApiFilter[0][0];
            Assert.Equal("FirstName", filer.Field.Name);
            Assert.Equal("LastName", filer.ComparedField.Name);
            Assert.Equal(FilterType.Equal, filer.Type);

        }

        [Fact]
        public void _003_Case_of_NullableFieldQuery() {
            var sut = new QueryParameter<Employee>(e => e.Comment == "I am a person who raises himself up to the next level, rather than becoming discouraged.");
            Assert.Equal(null, sut.Key);
            Assert.Equal(2, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            var filer = sut.ApiFilter[0][0];
            Assert.Equal("N_Comment", filer.Field.Name);
            Assert.Equal(false, filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            filer = sut.ApiFilter[1][0];
            Assert.Equal("Comment", filer.Field.Name);
            Assert.Equal("I am a person who raises himself up to the next level, rather than becoming discouraged.", filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            sut = new QueryParameter<Employee>(e => e.Comment == null);
            Assert.Equal(null, sut.Key);
            Assert.Equal(1, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            filer = sut.ApiFilter[0][0];
            Assert.Equal("N_Comment", filer.Field.Name);
            Assert.Equal(true, filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            sut = new QueryParameter<Employee>(e => e.Comment == e.Comment);
            Assert.Equal(null, sut.Key);
            Assert.Equal(2, sut.ApiFilter.FilterCount);
            Assert.Equal(null, sut.AdditionalFilter);

            filer = sut.ApiFilter[0][0];
            Assert.Equal("N_Comment", filer.Field.Name);
            Assert.Equal("N_Comment", filer.ComparedField.Name);
            Assert.Equal(FilterType.Equal, filer.Type);

            filer = sut.ApiFilter[1][0];
            Assert.Equal("Comment", filer.Field.Name);
            Assert.Equal("Comment", filer.ComparedField.Name);
            Assert.Equal(FilterType.Equal, filer.Type);
        }

        [Fact]
        public void _004_Case_of_MixedQuery() {
            Expression<Func<Employee, bool>> whereExpression = e =>
                (e.Id == 1 || e.Id == 2 || e.Id == 3) &&
                e.FirstName != "Takamori" &&
                e.LastName.GreaterThanOrEqual("R") &&
                e.Salary > 1000 &&
                String.IsNullOrEmpty(e.Comment);
            var sut = new QueryParameter<Employee>(whereExpression);
            Assert.Equal(1, sut.Key.KeyNumber);
            Assert.Equal(6, sut.ApiFilter.FilterCount);
            Assert.NotEqual(null, sut.AdditionalFilter);

            var filer = sut.ApiFilter[0][0];
            Assert.Equal("Id", filer.Field.Name);
            Assert.Equal(1, filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            filer = sut.ApiFilter[0][1];
            Assert.Equal("Id", filer.Field.Name);
            Assert.Equal(2, filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            filer = sut.ApiFilter[0][2];
            Assert.Equal("Id", filer.Field.Name);
            Assert.Equal(3, filer.Value);
            Assert.Equal(FilterType.Equal, filer.Type);

            filer = sut.ApiFilter[1][0];
            Assert.Equal("FirstName", filer.Field.Name);
            Assert.Equal("Takamori", filer.Value);
            Assert.Equal(FilterType.NotEqual, filer.Type);

            filer = sut.ApiFilter[2][0];
            Assert.Equal("LastName", filer.Field.Name);
            Assert.Equal("R", filer.Value);
            Assert.Equal(FilterType.GreaterThanOrEqual, filer.Type);

            filer = sut.ApiFilter[3][0];
            Assert.Equal("Salary", filer.Field.Name);
            Assert.Equal(1000m, filer.Value);
            Assert.Equal(FilterType.GreaterThan, filer.Type);

            sut = new QueryParameter<Employee>(null, whereExpression);
            Assert.Equal(null, sut.Key);
            Assert.Equal(6, sut.ApiFilter.FilterCount);
            Assert.NotEqual(null, sut.AdditionalFilter);

            sut = new QueryParameter<Employee>(Employee.Keys[0], whereExpression);
            Assert.Equal(0, sut.Key.KeyNumber);
            Assert.Equal(6, sut.ApiFilter.FilterCount);
            Assert.NotEqual(null, sut.AdditionalFilter);

            sut = new QueryParameter<Employee>(Employee.Keys[1], whereExpression);
            Assert.Equal(1, sut.Key.KeyNumber);
            Assert.Equal(6, sut.ApiFilter.FilterCount);
            Assert.NotEqual(null, sut.AdditionalFilter);
        }
    }
}
