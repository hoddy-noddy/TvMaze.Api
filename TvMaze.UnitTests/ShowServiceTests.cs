using AutoMapper;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvMaze.BLL;
using TvMaze.BLL.AutoMapper;
using TvMaze.BLL.Dto;
using TvMaze.DAL;

namespace TvMaze.UnitTests
{
    [TestFixture]
    public class ShowServiceTests
    {
        [Test]
        public void GetAllShowsAsync_WithValidPageNuberAndPageSize_ReturnsListOfShowsOrderedByShowIdAscending()
        {

            var show1 = new Show
            {
                Id = 11,
                ShowId = 22,
                Name = "show1",
                Cast = ""
            };
            var show2 = new Show
            {
                Id = 22,
                ShowId = 33,
                Name = "show2",
                Cast = ""
            };

            var showList = new List<Show> { show1, show2 };

            var mocker = new AutoMocker();
            mocker.GetMock<IDatabaseService>().Setup(s => s.GetAllShowsAsync(10, 1, It.IsAny<CancellationToken>())).Returns(Task.FromResult(showList));
            var mapper = new MapperConfiguration(config => config.AddProfile<EntityToDtoProfile>()).CreateMapper();

            mocker.Use(mapper);

            var sut = mocker.CreateInstance<ShowService>();

            var actual =  sut.GetAllShowsAsync(10, 1,new CancellationToken()).Result;
           
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(11, actual.First().Id);
        }
        [Test]
        public void GetAllShowsAsync_WithValidPageNumberAndPageSize_ReturnsListOfShowsWithCastOrderedByBirthday()
        {
            var birthday1 = DateTime.Now.AddDays(-2);
            var birthday2 = DateTime.Now.AddDays(-4);

            var cast1 = new List<PersonDto>
            {
                new PersonDto
                {
                     Name = "Lau",
                      Id = 33,
                       Birthday = birthday1
                },
                new PersonDto
                {
                     Name = "Lau2",
                     Id = 44,
                      Birthday = birthday2
                }
            };
            var cast2 = new List<PersonDto>
            {
                new PersonDto
                {
                     Name = "Lau3",
                      Id = 55,
                       Birthday = birthday1
                },
                new PersonDto
                {
                     Name = "Lau4",
                     Id = 66,
                      Birthday = birthday2
                }
            };
            var show1 = new Show
            {
                Id = 11,
                ShowId = 22,
                Name = "show1",
                Cast = JsonConvert.SerializeObject(cast1)
            };
            var show2 = new Show
            {
                Id = 22,
                ShowId = 33,
                Name = "show2",
                Cast = JsonConvert.SerializeObject(cast2)
            };

            var showList = new List<Show> { show1, show2 };

            var mocker = new AutoMocker();
            mocker.GetMock<IDatabaseService>().Setup(s => s.GetAllShowsAsync(10, 1, It.IsAny<CancellationToken>())).Returns(Task.FromResult(showList));
            var mapper = new MapperConfiguration(config => config.AddProfile<EntityToDtoProfile>()).CreateMapper();

            mocker.Use(mapper);

            var sut = mocker.CreateInstance<ShowService>();

            var actual = sut.GetAllShowsAsync(10, 1, new CancellationToken()).Result;

            Assert.AreEqual(birthday1, actual.First().Cast.First().Birthday);
        }

        //More tests are obviously required here to fully test this class
    }
}
