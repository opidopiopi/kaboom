using Kaboom.Adapters;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Drawing;

namespace Kaboom.Testing.Adapters
{
    [TestClass]
    public class WindowMapperTests
    {

        [TestMethod]
        public void domain_to_adapter_is_null_when_not_previously_mapped_other_way_around_before()
        {
            //Arrange
            Window window = new Window(new Bounds(5, 5, 55, 55), "some very cool name");
            WindowMapper mapper = new WindowMapper();

            //Act
            var adapter = mapper.MapToIWindow(window);

            //Assert
            Assert.IsNull(adapter);
        }


        [TestMethod]
        public void domain_to_adapter_after_mapping_other_way_around_before()
        {
            //Arrange
            var iWindowMock = new Mock<IWindow>();
            iWindowMock.Setup(mock => mock.WindowName()).Returns("amazing name");
            iWindowMock.Setup(mock => mock.GetActualWindowRect()).Returns(new Rectangle(5, 5, 55, 55));

            var mapper = new WindowMapper();
            var window = mapper.MapToDomain(iWindowMock.Object);

            //Act
            var iWindow = mapper.MapToIWindow(window);

            //Assert
            Assert.AreEqual(iWindowMock.Object, iWindow);
        }


        [TestMethod]
        public void adapter_to_domain()
        {
            //Arrange
            var iWindowMock = new Mock<IWindow>();
            iWindowMock.Setup(mock => mock.WindowName()).Returns("amazing name");
            iWindowMock.Setup(mock => mock.GetActualWindowRect()).Returns(new Rectangle(5, 5, 55, 55));

            var mapper = new WindowMapper();

            //Act
            var window = mapper.MapToDomain(iWindowMock.Object);

            //Assert
            Assert.AreEqual("amazing name", window.Title);
            Assert.AreEqual(new Bounds(5, 5, 55, 55), window.Bounds);
        }


        [TestMethod]
        public void adapter_to_domain_multiple()
        {
            //Arrange
            var mockA = new Mock<IWindow>();
            mockA.Setup(mock => mock.WindowName()).Returns("nameA");
            mockA.Setup(mock => mock.GetActualWindowRect()).Returns(new Rectangle(5, 5, 55, 55));

            var mockB = new Mock<IWindow>();
            mockB.Setup(mock => mock.WindowName()).Returns("nameB");
            mockB.Setup(mock => mock.GetActualWindowRect()).Returns(new Rectangle(-5, -5, 55, 55));

            var mapper = new WindowMapper();

            //Act
            var windowA = mapper.MapToDomain(mockA.Object);
            var windowB = mapper.MapToDomain(mockB.Object);

            //Assert
            Assert.AreEqual("nameA", windowA.Title);
            Assert.AreEqual(new Bounds(5, 5, 55, 55), windowA.Bounds);

            Assert.AreEqual("nameB", windowB.Title);
            Assert.AreEqual(new Bounds(-5, -5, 55, 55), windowB.Bounds);
        }


        [TestMethod]
        public void adapter_to_window_always_returns_same_entity()
        {
            //Arrange
            var iWindowMock = new Mock<IWindow>();
            iWindowMock.Setup(mock => mock.WindowName()).Returns("amazing name");
            iWindowMock.Setup(mock => mock.GetActualWindowRect()).Returns(new Rectangle(5, 5, 55, 55));

            var mapper = new WindowMapper();

            //Act
            var window = mapper.MapToDomain(iWindowMock.Object);
            var anotherTime = mapper.MapToDomain(iWindowMock.Object);

            //Assert
            Assert.AreEqual(window, anotherTime);
        }


        [TestMethod]
        public void can_remove_mapping()
        {
            //Arrange
            var iWindowMock = new Mock<IWindow>();
            iWindowMock.Setup(mock => mock.WindowName()).Returns("amazing name");
            iWindowMock.Setup(mock => mock.GetActualWindowRect()).Returns(new Rectangle(5, 5, 55, 55));

            var mapper = new WindowMapper();

            //Act
            var window = mapper.MapToDomain(iWindowMock.Object);

            mapper.RemoveMappingForWindow(iWindowMock.Object);

            var anotherTime = mapper.MapToDomain(iWindowMock.Object);

            //Assert
            Assert.AreNotEqual(window, anotherTime);
        }
    }
}