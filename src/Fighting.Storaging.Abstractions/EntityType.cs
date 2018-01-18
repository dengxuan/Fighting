using System;

namespace Fighting.Storaging
{
    public class EntityType
    {
        /// <summary>
        /// Type of the entity.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// DbContext type that has DbSet property.
        /// </summary>
        public Type DeclaringType { get; }

        public EntityType(Type type, Type declaringType)
        {
            Type = type;
            DeclaringType = declaringType;
        }
    }
}
