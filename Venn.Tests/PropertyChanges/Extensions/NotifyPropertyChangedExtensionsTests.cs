using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using Venn.Extensions;
using Venn.PropertyChanges;

namespace Venn.Tests.Extensions
{
    [TestFixture]
    public class NotifyPropertyChangedExtensionsTests
    {
        [Test]
        public void WhenAny_SourcePropertyChanged_NotifiesObservable()
        {
            // Arrange
            var sourceMock = new Mock<VennClass>();
            var observable = sourceMock.Object.WhenAny(x => x.Property1);

            var observerMock = new Mock<IObserver<string>>();
            observable.Subscribe(observerMock.Object);

            // Act
            sourceMock.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("Property1"));

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void WhenAny_SourcePropertyChanged_DoesNotNotifyForOtherProperties()
        {
            // Arrange
            var sourceMock = new Mock<VennClass>();
            var observable = sourceMock.Object.WhenAny(x => x.Property1);

            var observerMock = new Mock<IObserver<string>>();
            observable.Subscribe(observerMock.Object);

            // Act
            sourceMock.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("OtherProperty"));

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void WhenAny_SourceIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => NotifyPropertyChangedExtensions.WhenAny((VennClass)null, x => x.Property1));
        }

        [Test]
        public void WhenAny_PropertySelectorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var sourceMock = new Mock<VennClass>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => NotifyPropertyChangedExtensions.WhenAny(sourceMock.Object, (Expression<Func<VennClass, string>>)null));
        }

        [Test]
        public void WhenAnyWrapper_SourcePropertyChanged_NotifiesObservable()
        {
            // Arrange
            var sourceMock = new Mock<NotifyPropertyChangedWrapper<string>>();
            sourceMock.SetupGet(x => x.Value).Returns("InitialValue");

            // Accede a la propiedad Observable utilizando reflexión
            var observableProperty = typeof(NotifyPropertyChangedWrapper<string>).GetProperty("Observable", BindingFlags.Instance | BindingFlags.NonPublic);
            var observable = (IObservable<string>)observableProperty.GetValue(sourceMock.Object);

            var observerMock = new Mock<IObserver<string>>();
            observable.Subscribe(observerMock.Object);

            // Act
            sourceMock.Raise(x => x.PropertyChanged += null, EventArgs.Empty);

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void WhenAnyWrapper_SourcePropertyChanged_DoesNotNotifyForOtherProperties()
        {
            // Arrange
            var sourceMock = new Mock<NotifyPropertyChangedWrapper<string>>();
            sourceMock.SetupGet(x => x.Value).Returns("InitialValue");

            // Accede a la propiedad Observable utilizando reflexión
            PropertyInfo observableProperty = typeof(NotifyPropertyChangedWrapper<string>).GetProperty("Observable", BindingFlags.Instance | BindingFlags.NonPublic);
            var observable = (IObservable<string>)observableProperty.GetValue(sourceMock.Object);

            var observerMock = new Mock<IObserver<string>>();
            observable.Subscribe(observerMock.Object);

            // Act
            sourceMock.Raise(x => x.PropertyChanged += null, EventArgs.Empty);

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void WhenAnyWrapper_SourceIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => NotifyPropertyChangedExtensions.WhenAny((NotifyPropertyChangedWrapper<string>)null, x => x.Length));
        }

        [Test]
        public void WhenAnyWrapper_PropertySelectorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var sourceMock = new Mock<NotifyPropertyChangedWrapper<string>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => NotifyPropertyChangedExtensions.WhenAny<string, string>(sourceMock.Object, null));
        }

        [Test]
        public void WhenAnyWrapper_SourceValueIsNull_DoesNotNotifyObservable()
        {
            // Arrange
            var sourceMock = new Mock<NotifyPropertyChangedWrapper<string>>();
            sourceMock.SetupGet(x => x.Value).Returns((string)null);

            // Accede a la propiedad Observable utilizando reflexión
            var observableProperty = typeof(NotifyPropertyChangedWrapper<string>).GetProperty("Observable", BindingFlags.Instance | BindingFlags.NonPublic);
            var observable = (IObservable<string>)observableProperty.GetValue(sourceMock.Object);

            var observerMock = new Mock<IObserver<string>>();
            observable.Subscribe(observerMock.Object);

            // Act
            sourceMock.Raise(x => x.PropertyChanged += null, EventArgs.Empty);

            // Assert
            observerMock.Verify(x => x.OnNext(It.IsAny<string>()), Times.Never);
        }
    }

    public class VennClass : INotifyPropertyChanged
    {
        private string _property1;

        public string Property1
        {
            get { return _property1; }
            set
            {
                if (_property1 != value)
                {
                    _property1 = value;
                    OnPropertyChanged(nameof(Property1));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
