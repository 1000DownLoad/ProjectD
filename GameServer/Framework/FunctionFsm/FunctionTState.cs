using System;

namespace Framework.FunctionFsm
{
    public class FunctionTState<T> where T : Enum
    {
        public T Key;

        public Action OnEnter = null;
        public Action OnExecute = null;
        public Action OnExit = null;

    }
}
