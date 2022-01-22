using Moq;
using Saal.Todos.Repositories;
using Saal.Todos.Services.Category;
using Saal.Todos.Services.Core;
using Dto = Saal.Todos.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Saal.Todos.Repositories.Base;

namespace Saal.Todos.Services.Tests.Category
{
    public class CategoryServiceSpec
    {
        [Fact]
        public void GivenNullCategoryRepositoryThrowsArgumentNullException()
        {
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            Assert.Throws<ArgumentNullException>(() => new CategoryService(null, validatorMoq.Object));
        }

        [Fact]
        public void GivenNullCategoryValidatorThrowsArgumentNullException()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            Assert.Throws<ArgumentNullException>(() => new CategoryService(repositoryMoq.Object, null));
        }

        [Fact]
        public async Task GivenANullCommandThrowsArgumentNullException()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null));
        }

        [Fact]
        public async Task GivenInvalidCreateCategoryCommandThenCommandRejected()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validator = new CategoryValidator();
            var sut = new CategoryService(repositoryMoq.Object, validator);
            var createCategory = new CreateCategoryCommand();
            var commandResult = await sut.Handle(createCategory);
            Assert.NotNull(commandResult.RejectedReason);
        }

        [Fact]
        public async Task FetchesCategories()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            var categories = new List<Dto.Category>();
            var expected = new PaginatedResult<Dto.Category>(1200, 1, 50, categories);
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            repositoryMoq.Setup(m => m.Fetch(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object);
            var actual = await sut.Fetch(50, 1);
            Assert.Equal(expected, actual);
        }
    }
}
