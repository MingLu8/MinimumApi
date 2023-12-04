using FluentAssertions;
using Microsoft.Data.SqlClient;
using MinimalApi.UnitTests.db;
using MinimumApi.Entities;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit.Extensions.AssemblyFixture;

namespace MinimalApi.UnitTests.db.RepoDbGenericRepositoryTests
{
    public class GetById : IAssemblyFixture<DatabaseFixture>
    {
        private readonly TestDataRepoDbRepository _repo;
        private readonly DatabaseFixture _fixture;

        public GetById(DatabaseFixture fixture)
        {
            _repo = new TestDataRepoDbRepository(fixture.Db);
            _fixture = fixture;
        }

        [Fact]
        public void GetById_with_non_existing_record_returns_null()
        {
            var result = _repo.GetById(1);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_with_non_existing_record_returns_null()
        {
            var result = await _repo.GetByIdAsync(1);
            result.Should().BeNull();
        }

        [Fact]
        public void GetById_with_existing_record_returns_the_record()
        {
            using var tran = new TransactionScope();
            var data = new TestData { Age = 1, Name = "x", CreatedDateUtc = DateTime.Today };
            _repo.Insert(data);

            var result = _repo.GetById(data.Id);
            result.Should().BeEquivalentTo(data);
            result.Should().NotBeNull();
            result?.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetByIdAsync_with_existing_record_returns_the_record()
        {
            using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var data = new TestData { Age = 1, Name = "x", CreatedDateUtc = DateTime.Today };
            _repo.Insert(data);

            var result = await _repo.GetByIdAsync(data.Id);
            result.Should().BeEquivalentTo(data);
            result.Should().NotBeNull();
            result?.Id.Should().BeGreaterThan(0);
        }
    }
}
