using Gameserver.Interfaces;
using Gameserver.Movement;

namespace TMovement
{
    [Binding]
    public class ДвижениеОбъекта
    {
        private readonly Mock<IMovement> _mockMovable;
        private ICommand? _movecommand;
        public ДвижениеОбъекта()
        {
            _mockMovable = new Mock<IMovement>();
        }

        [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
        public void ДопустимКосмическийКорабльНаходитсяВТочкеПространстваСКоординатами(int p0, int p1)
        {
            _mockMovable.Setup(m => m.position).Returns(new Vector(new int[] { p0, p1 }));
        }

        [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
        public void ДопустимКосмическийКорабльПоложениеВПространствеКоторогоНевозможноОпределить()
        {
            _mockMovable.SetupGet(m => m.position).Throws(new ArgumentException()).Verifiable();
        }

        [Given(@"скорость корабля определить невозможно")]
        public void ДопустимСкоростьКорабляОпределитьНевозможно()
        {
            _mockMovable.SetupGet(m => m.speed).Throws(new ArgumentException()).Verifiable();
        }

        [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
        public void ДопустимИмеетМгновеннуюСкорость(int p0, int p1)
        {
            _mockMovable.Setup(m => m.speed).Returns(new Vector(new int[] { p0, p1 }));
        }

        [Given(@"изменить положение в пространстве космического корабля невозможно")]
        public void ДопустимИзменитьПоложениеВПространствеКосмическогоКорабляНевозможно()
        {
            _mockMovable.SetupSet(x => x.position = It.IsAny<Vector>()).Throws(new ArgumentException()).Verifiable();
        }

        [When(@"происходит прямолинейное равномерное движение без деформации")]
        public void КогдаПроисходитПрямолинейноеРавномерноеДвижениеБезДеформации()
        {
            _movecommand = new Movement(_mockMovable.Object);
        }

        [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
        public void ТоКосмическийКорабльПеремещаетсяВТочкуПространстваСКоординатами(int p0, int p1)
        {
            _movecommand!.Execute();
            _mockMovable.VerifySet(m => m.position = new Vector(new int[] { p0, p1 }), Times.Once);
            _mockMovable.VerifyAll();
        }

        [Then(@"возникает ошибка Exception")]
        public void ТоВозникаетОшибкаException()
        {
            Assert.Throws<ArgumentException>(() => _movecommand!.Execute());
        }
    }
}