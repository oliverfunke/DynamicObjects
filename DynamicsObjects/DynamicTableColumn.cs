using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS.Toolbox.DynamicObjects
{
    public class DynamicTableColumn<T> : IDynamicTableColumn
    {
        #region Constructor

        public DynamicTableColumn(
            string name)
        {
            _name = name;
            _defaultValue = default(T);
        }

        /// <summary>
        /// create a new column
        /// Exceptions
        ///     ArgumentException: if type of default value does not match with given type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valueType"></param>
        /// <param name="defaultValue"></param>
        public DynamicTableColumn(
            string name,
            T defaultValue)
        {
            _name = name;
            _defaultValue = defaultValue;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public Type ValueType
        {
            get { return typeof(T); }
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        #endregion

        #region internal

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        #endregion

        #region Member

        private string _name;
        private T _defaultValue;

        #endregion
    }
}
