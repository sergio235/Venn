using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Venn.PropertyChanges;

namespace Venn.PropertyChanges.Tests
{
    [TestFixture]
    public class NotifyPropertyChangedWrapperTests
    {
        [Test]
        public void ValueChange_NotifiesObservable()
        {
            // Arrange
            var initialValue = 42;
            var wrapper = new NotifyPropertyChangedWrapper<int>(initialValue);

            // Utiliza reflexión para acceder a la propiedad internal Observable
            var observableProperty = typeof(NotifyPropertyChangedWrapper<int>).GetProperty("Observable", BindingFlags.Instance | BindingFlags.NonPublic);
            var observable = observableProperty.GetValue(wrapper) as IObservable<int>;

            var observerMock = new Mock<IObserver<int>>();
            observable.Subscribe(observerMock.Object);

            // Act
            wrapper.Value = 99;

            // Assert
            observerMock.Verify(x => x.OnNext(99), Times.Once);
        }

        [Test]
        public void PropertyChanged_NotifiesPropertyChangedEvent()
        {
            // Arrange
            var initialValue = "Hello";
            var wrapper = new NotifyPropertyChangedWrapper<string>(initialValue);

            var eventRaised = false;
            wrapper.PropertyChanged += (sender, args) => eventRaised = true;

            // Act
            wrapper.Value = "World";

            // Assert
            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void PauseResume_NotifiesPropertyChangedAfterResume()
        {
            // Arrange
            var initialValue = DateTime.Now;
            var wrapper = new NotifyPropertyChangedWrapper<DateTime>(initialValue);

            // Utiliza reflexión para acceder a la propiedad internal Observable
            var observableProperty = typeof(NotifyPropertyChangedWrapper<DateTime>).GetProperty("Observable", BindingFlags.Instance | BindingFlags.NonPublic);
            var observable = observableProperty.GetValue(wrapper) as IObservable<DateTime>;

            var observerMock = new Mock<IObserver<DateTime>>();
            observable.Subscribe(observerMock.Object);

            // Act
            wrapper.Pause();
            wrapper.Value = initialValue.AddSeconds(1);

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<DateTime>()), Times.Never);

            // Act (Resume)
            wrapper.Resume();

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public async Task PauseResume_DoesNotNotifyDuringPauseAsync()
        {
            // Arrange
            var initialValue = "Async";
            var wrapper = new NotifyPropertyChangedWrapper<string>(initialValue);

            // Utiliza reflexión para acceder a la propiedad internal Observable
            var observableProperty = typeof(NotifyPropertyChangedWrapper<string>).GetProperty("Observable", BindingFlags.Instance | BindingFlags.NonPublic);
            var observable = observableProperty.GetValue(wrapper) as IObservable<string>;

            var observerMock = new Mock<IObserver<string>>();
            observable.Subscribe(observerMock.Object);

            // Act
            wrapper.Pause();
            wrapper.Value = "Paused";

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<string>()), Times.Never);

            // Act (Resume after a delay)
            await Task.Delay(500);
            wrapper.Resume();

            // Assert
            observerMock.Verify(x => x.OnNext("Paused"), Times.Once);
        }
    }
}
