namespace FancyScrollView
{
    public class FancyGridRowData<TEntity>
    {
        public TEntity[] Entities { get; }
        public FancyGridRowData(TEntity[] entities) => Entities = entities;
    }
}
