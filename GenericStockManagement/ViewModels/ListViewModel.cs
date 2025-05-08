namespace GenericStockManagement.ViewModels
{
    public class ListViewModel<T> where T : class
    {
        public IEnumerable<T> List { get; set; }

        public ListViewModel(IEnumerable<T> _list)
        {
            List = _list;
        }
    }
}
