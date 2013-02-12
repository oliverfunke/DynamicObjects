using System;

namespace OS.Toolbox.DinamicObjects
{
    public interface IDynamicTableColumn
    {
        string Name { get; }
        Type ValueType { get; }
        object DefaultValue { get; }
    }
}
