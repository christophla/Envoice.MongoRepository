using System;
using Envoice.Conditions;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// Defines a virtual collection for an entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VirtualCollectionName : Attribute
    {
        /// <summary>
        /// The virtual collection name.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Applies a virtual collection for this entity.
        /// </summary>
        /// <param name="name">The virtual collection name</param>
        public VirtualCollectionName(string name)
        {
            Condition.Requires(name, "name").IsNotNullOrWhiteSpace();

            this.Name = name;
        }
    }
}
