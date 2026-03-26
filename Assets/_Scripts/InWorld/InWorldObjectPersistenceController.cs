using _Scripts.Injection;

namespace _Scripts.InWorld
{
    public class InWorldObjectPersistenceController
    {
        public void Setup(InWorldObjectPersistenceView inWorldObjectPersistenceView)
        {
            var inWorldObjectPersistenceModel = ServiceLocator.Resolve<InWorldObjectPersistenceModel>();
            inWorldObjectPersistenceModel.Register(inWorldObjectPersistenceView);
        }
    }
}
