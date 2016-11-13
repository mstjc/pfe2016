using System;

public class MonsterFactory  {

    public static IMovement CreateMovement(MovementEnum movementType)
    {
        var type = Type.GetType(typeof(IMovement).Namespace + "." + movementType.ToString(), throwOnError: false);

        if (type == null)
        {
            throw new InvalidOperationException(movementType.ToString() + " is not a known dto type");
        }

        if (!typeof(IMovement).IsAssignableFrom(type))
        {
            throw new InvalidOperationException(type.Name + " does not inherit from IMovement");
        }

        return (IMovement)Activator.CreateInstance(type);
    }
}
