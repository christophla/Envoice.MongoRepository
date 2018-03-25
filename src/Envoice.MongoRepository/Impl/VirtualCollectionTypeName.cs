using System;
using Envoice.Conditions;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// Defines a virtual collection type for an entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VirtualCollectionTypeName : Attribute
    {
        /// <summary>
        /// The virtual collection type name.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Applies a virtual collection type name for this entity.
        /// </summary>
        /// <param name="name">The virtual collection type name</param>
        public VirtualCollectionTypeName(string typeName)
        {
            Condition.Requires(typeName, "typeName").IsNotNullOrWhiteSpace();

            this.TypeName = typeName;
        }
    }
}
