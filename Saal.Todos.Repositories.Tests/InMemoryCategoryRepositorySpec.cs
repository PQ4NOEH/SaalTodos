using Saal.Todos.Contracts.Dto;
using Saal.Todos.Repositories.Tests.Fakes;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace Saal.Todos.Repositories.Tests
{
    public class InMemoryCategoryRepositorySpec
    {
        [Fact]
        public void GivenANullMemoryStoragethrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new InMemoryCategoryRepository(null));
        }

        [Fact]
        public async Task GivenANewAggregateWhenSavedthenReturnsItsId()
        {
            var storage = new InMemoryStorageFake<Category>();
            var newCategory = new Category
            {
                Name = "The category Name"
            };
            var sut = new InMemoryCategoryRepository(storage);
            var categoryId = await sut.Insert(newCategory);
            Assert.Equal(1, categoryId);
        }

        [Fact]
        public async Task GivenNullWhenSavedThenThrowsArgumentNullException()
        {
            var storage = new InMemoryStorageFake<Category>();
            var sut = new InMemoryCategoryRepository(storage);
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Insert(null));
        }

        [Fact]
        public async Task GivenNullWhenUpdateThenThrowsArgumentNullException()
        {
            var storage = new InMemoryStorageFake<Category>();
            var sut = new InMemoryCategoryRepository(storage);
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Update(null));
        }

        [Fact]
        public async Task GivenAnExistingRecordWhenUpdatedItgetChanged()
        {
            const string NEW_NAME = "a new Name";
            var storage = new InMemoryStorageFake<Category>();
            storage.Data.Add(new Category { Id = 1, Name = "category Name" });
            var sut = new InMemoryCategoryRepository(storage);
            await sut.Update(new Category { Id = 1, Name = NEW_NAME });
            Assert.Single(storage.Data);
            Assert.Equal(NEW_NAME, storage.Data.First().Name);
        }

        [Fact]
        public async Task GivenAnExistingRecordWhenDeletingThenReturnstrue()
        {
            var storage = new InMemoryStorageFake<Category>();
            storage.Data.Add(new Category { Id = 1, Name = "category Name" });
            var sut = new InMemoryCategoryRepository(storage);
            await sut.Remove(1);
            Assert.Empty(storage.Data);
        }
    }
}
