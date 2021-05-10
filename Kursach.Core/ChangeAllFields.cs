using System;

namespace ISTraining_Part.Core
{

    public static class ChangeAllFields
    {
        public static void SetAllFields<T>(this T source, T newObject)
        {
            if (source == null || newObject == null)
                return;

            var props = source.GetType().GetProperties();
            var newProps = newObject.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                var currentProp = props[i];
                var newProp = newProps[i];

                if (Attribute.IsDefined(currentProp, typeof(ChangeIgnoreAttribute)))
                    continue;

                currentProp.SetValue(source, newProp.GetValue(newObject));
            }
        }
    }
}
