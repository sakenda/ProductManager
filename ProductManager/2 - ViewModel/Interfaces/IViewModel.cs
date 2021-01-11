namespace ProductManager.ViewModel
{
    public interface IViewModel<T> where T : class
    {
        void UndoChanges();
        void AcceptChanges();
        T GetModel();
    }
}