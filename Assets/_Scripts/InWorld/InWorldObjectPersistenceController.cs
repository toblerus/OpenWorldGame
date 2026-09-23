using _Scripts.Injection;

namespace _Scripts.InWorld
{
    public class InWorldObjectPersistenceController
    {
        private readonly InWorldObjectPersistenceModel _model;

        public InWorldObjectPersistenceController()
        {
            _model = ServiceLocator.Resolve<InWorldObjectPersistenceModel>();
        }

        public void Setup(InWorldObjectPersistenceView inWorldObjectPersistenceView)
        {
            _model.Register(inWorldObjectPersistenceView);
        }
    }
}
