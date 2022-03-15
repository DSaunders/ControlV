using System.Collections.Generic;
using ControlV.Actions;

namespace ControlV
{
    internal static class ActionPipeline
    {
        public static List<IAction> Actions = new List<IAction>
        {
            new JsonFormat()
        };
    }
}