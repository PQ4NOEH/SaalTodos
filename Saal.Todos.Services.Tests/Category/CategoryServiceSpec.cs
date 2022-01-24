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
using Saal.Todos.Services.Tests.Fakes;

namespace Saal.Todos.Services.Tests.Category
{
    public class CategoryServiceSpec
    {
        [Fact]
        public void GivenNullCategoryRepositoryThrowsArgumentNullException()
        {
            var categoryValidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            Assert.Throws<ArgumentNullException>(() => new CategoryService(null, categoryValidatorMoq.Object, todovalidatorMoq.Object));
        }

        [Fact]
        public void GivenNullCategoryValidatorThrowsArgumentNullException()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            Assert.Throws<ArgumentNullException>(() => new CategoryService(repositoryMoq.Object, null, todovalidatorMoq.Object));
        }

        [Fact]
        public void GivenNullTodoValidatorThrowsArgumentNullException()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            var categoryValidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            Assert.Throws<ArgumentNullException>(() => new CategoryService(repositoryMoq.Object, categoryValidatorMoq.Object, null));
        }

        

        [Fact]
        public async Task GivenInvalidCreateCategoryCommandThenCommandRejected()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validator = new CategoryValidator();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            var sut = new CategoryService(repositoryMoq.Object, validator, todovalidatorMoq.Object);
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
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            repositoryMoq.Setup(m => m.Fetch(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object, todovalidatorMoq.Object);
            var actual = await sut.Fetch(50, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task FetchASingleCategoryById()
        {
            var repositoryMoq = new Mock<ICategoryRepository>();
            var expected = new Contracts.Dto.Category
            {
                Id = 23,
                Name = "A category"
            };
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            repositoryMoq.Setup(m => m.FetchById(It.IsAny<int>())).ReturnsAsync(expected);
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object, todovalidatorMoq.Object);
            var actual = await sut.FetchById(23);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GivenAnInvalidCategoryWhenCreateThenReturnsRejectedCommand()
        {
            //moqs
            const string errorString = "Everything is wrong";
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            validatorMoq.Setup(m => m.Validate(It.IsAny<Contracts.Dto.Category>())).Returns(new ServiceValidationResultNoOK(errorString));
            
            //test
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object, todovalidatorMoq.Object);
            var command = new CreateCategoryCommand { CategoryName = "any" };
            var commandResult = await sut.Handle(command);

            //assert
            Assert.Equal(errorString, commandResult.RejectedReason.Reason);
        }

        [Fact]
        public async Task GivenAValidCategoryWhenCreateThenReturnsTheAggregate()
        {
            //moqs
            const int aggregateId = 32;
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            validatorMoq.Setup(m => m.Validate(It.IsAny<Contracts.Dto.Category>())).Returns(new ServiceValidationResultOK());
            repositoryMoq.Setup(r => r.Insert(It.IsAny<Contracts.Dto.Category>())).ReturnsAsync(aggregateId);

            //test
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object, todovalidatorMoq.Object);
            var command = new CreateCategoryCommand { CategoryName = "any" };
            var commandResult = await sut.Handle(command);

            //assert
            Assert.Equal(aggregateId, commandResult.Result.Id);
        }

        [Fact]
        public async Task GivenACategoryDoNotExistWhenChangeThenReturnsCommandrejected()
        {
            //moqs
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            validatorMoq.Setup(m => m.Validate(It.IsAny<Contracts.Dto.Category>())).Returns(new ServiceValidationResultOK());
            repositoryMoq.Setup(r => r.FetchById(It.IsAny<int>())).ReturnsAsync((Dto.Category)null);

            //test
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object, todovalidatorMoq.Object);
            var command = new ChangeCategoryCommand { CategoryName = "any" };
            var commandResult = await sut.Handle(command);

            //assert
            Assert.True(commandResult.RejectedReason.AggregateNotFound);
        }


        [Fact]
        public async Task GivenAnInvalidCategoryWhenChangeThenReturnsCommandrejected()
        {
            //moqs
            const string errorString = "Everything is wrong";
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            validatorMoq.Setup(m => m.Validate(It.IsAny<Contracts.Dto.Category>())).Returns(new ServiceValidationResultNoOK(errorString));
            repositoryMoq.Setup(r => r.FetchById(It.IsAny<int>())).ReturnsAsync(new Dto.Category());

            //test
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object, todovalidatorMoq.Object);
            var command = new ChangeCategoryCommand { CategoryName = "any" };
            var commandResult = await sut.Handle(command);

            //assert
            Assert.Equal(errorString, commandResult.RejectedReason.Reason);
        }

        [Fact]
        public async Task GivenAvalidCategoryWhenChangeThenReturnscommandAccepted()
        {
            //moqs
            var repositoryMoq = new Mock<ICategoryRepository>();
            var validatorMoq = new Mock<IServiceValidator<Contracts.Dto.Category>>();
            var todovalidatorMoq = new Mock<IServiceValidator<Contracts.Dto.Todo>>();
            validatorMoq.Setup(m => m.Validate(It.IsAny<Contracts.Dto.Category>())).Returns(new ServiceValidationResultOK());
            repositoryMoq.Setup(r => r.FetchById(It.IsAny<int>())).ReturnsAsync(new Dto.Category());

            //test
            var sut = new CategoryService(repositoryMoq.Object, validatorMoq.Object, todovalidatorMoq.Object);
            var command = new ChangeCategoryCommand { CategoryName = "any" };
            var commandResult = await sut.Handle(command);

            //assert
            Assert.Null(commandResult.RejectedReason);
        }
    }
}
