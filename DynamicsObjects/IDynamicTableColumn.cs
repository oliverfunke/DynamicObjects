using System;

namespace OS.Toolbox.DynamicObjects
{
    public interface IDynamicTableColumn
    {
        string Name { get; }
        Type ValueType { get; }
        object DefaultValue { get; }
    }
}
