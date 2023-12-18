using System;

namespace Framework.FunctionFsm
{
    public class FunctionState
    {
        public int Key { get; set; } = 0;

        public Action OnEnter = null;
        public Action OnExecute = null;
        public Action OnExit = null;

    }
}
