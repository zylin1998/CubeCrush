using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{   
    public interface ISetup
    {
        public void Setup(params object[] args);
    }

    public interface ISetup<Param1> : ISetup
    {
        public void Setup(Param1 param1);

        void ISetup.Setup(params object[] args)
            => Setup(args[0].To<Param1>());
    }

    public interface ISetup<Param1, Param2> : ISetup 
    {
        public void Setup(Param1 param1, Param2 param2);

        void ISetup.Setup(params object[] args)
            => Setup(
                args[0].To<Param1>(), 
                args[1].To<Param2>());
    }

    public interface ISetup<Param1, Param2, Param3> : ISetup
    {
        public void Setup(Param1 param1, Param2 param2, Param3 param3);

        void ISetup.Setup(params object[] args)
            => Setup(
                args[0].To<Param1>(), 
                args[1].To<Param2>(), 
                args[2].To<Param3>());
    }

    public interface ISetup<Param1, Param2, Param3, Param4> : ISetup
    {
        public void Setup(Param1 param1, Param2 param2, Param3 param3, Param4 param4);

        void ISetup.Setup(params object[] args)
            => Setup(
                args[0].To<Param1>(), 
                args[1].To<Param2>(), 
                args[2].To<Param3>(), 
                args[3].To<Param4>());
    }

    public interface ISetup<Param1, Param2, Param3, Param4, Param5> : ISetup
    {
        public void Setup(Param1 param1, Param2 param2, Param3 param3, Param4 param4, Param5 param5);

        void ISetup.Setup(params object[] args)
            => Setup(
                args[0].To<Param1>(), 
                args[1].To<Param2>(), 
                args[2].To<Param3>(), 
                args[3].To<Param4>(),
                args[4].To<Param5>());
    }
}
